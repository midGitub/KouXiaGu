using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using KouXiaGu.Collections;
using KouXiaGu.Rx;
using UnityEngine;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 本地化语言文本管理;
    /// </summary>
    public static class Localization
    {
        static Localization()
        {
            LanguageIndex = -1;
        }

        static string ConfigFilePath
        {
            get { return ResourcePath.CombineConfiguration("Localization/Config.xml"); }
        }

        static string PackDirectoryPath
        {
            get { return ResourcePath.CombineConfiguration("Localization"); }
        }

        static readonly LanguagePackReader packReader = new XmlLanguagePackReader();
        static readonly XmlLocalizationConfigReader configReader = new XmlLocalizationConfigReader();
        static readonly HashSet<ITextObserver> textObservers = new HashSet<ITextObserver>();

        static LanguagePack LanguagePack { get; set; }
        public static ReadOnlyCollection<LanguageFile> Languages { get; private set; }
        public static int LanguageIndex { get; private set; }

        public static IDictionary<string, string> textDictionary
        {
            get { return LanguagePack == null ? null : LanguagePack.TextDictionary; }
        }

        public static Language CurrentLanguage
        {
            get { return LanguagePack; }
        }

        /// <summary>
        /// 词条总数;
        /// </summary>
        public static int EntriesCount
        {
            get { return textDictionary != null ? textDictionary.Count : 0; }
        }

        /// <summary>
        /// 确认是否存在这个观察者;
        /// </summary>
        public static bool Contains(ITextObserver observer)
        {
            return textObservers.Contains(observer);
        }

        /// <summary>
        /// 订阅到文本变化;
        /// </summary>
        public static IDisposable Subscribe(ITextObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");
            if (!textObservers.Add(observer))
                throw new ArgumentException();

            if(textDictionary != null)
                TrackObserver(observer);

            return new CollectionUnsubscriber<ITextObserver>(textObservers, observer);
        }

        static void TrackObserver(ITextObserver observer)
        {
            if (textDictionary == null)
                throw new ArgumentNullException("textDictionary");

            observer.UpdateTexts(textDictionary);
        }

        /// <summary>
        /// 需要在Unity线程内调用,通知到所有观察者,文本内容发生变化;
        /// </summary>
        public static void TrackAll()
        {
            ITextObserver[] observerArray = textObservers.ToArray();
            foreach (var observer in observerArray)
            {
                TrackObserver(observer);
            }
        }

        /// <summary>
        /// 同步的初始化;
        /// </summary>
        public static void Initialize()
        {
            Initialize_Read();
            TrackAll();
        }

        /// <summary>
        /// 异步的初始化;
        /// </summary>
        public static IAsyncOperation InitializeAsync()
        {
            return new AsyncInitializer();
        }

        /// <summary>
        /// 初始化方法的多线程部分;
        /// </summary>
        static void Initialize_Read()
        {
            var config = ReadConfig();
            var originalLanguages = Languages;
            var originalLanguageIndex = LanguageIndex;
            var originalLanguagePack = LanguagePack;

            try
            {
                Languages = SearchLanguagePacks();
                LanguageIndex = FindIndex(Languages, config);
                LanguagePack = ReadPack(Languages[LanguageIndex]);
            }
            catch(Exception ex)
            {
                Languages = originalLanguages;
                LanguageIndex = originalLanguageIndex;
                LanguagePack = originalLanguagePack;
                throw ex;
            }
        }

        class AsyncInitializer : ThreadOperation
        {
            public AsyncInitializer()
            {
                this.SubscribeCompleted(OnCompleted);
                Start();
            }

            protected override void Operate()
            {
                Initialize_Read();
            }

            void OnCompleted(AsyncInitializer item)
            {
                TrackAll();
            }
        }


        /// <summary>
        /// 设置到语言;传入语言下标;
        /// </summary>
        public static void SetLanguage(int languageIndex)
        {
            SetLanguage_Read(languageIndex);
            TrackAll();
        }

        /// <summary>
        /// 变更语言的多线程部分;
        /// </summary>
        static void SetLanguage_Read(int languageIndex)
        {
            var originalLanguagePack = LanguagePack;
            var originalLanguageIndex = LanguageIndex;
            try
            {
                LanguagePack = ReadPack(Languages[languageIndex]);
                LanguageIndex = languageIndex;
            }
            catch(Exception ex)
            {
                LanguagePack = originalLanguagePack;
                LanguageIndex = originalLanguageIndex;
                throw ex;
            }
        }

        /// <summary>
        /// 异步设置到语言;
        /// </summary>
        public static IAsyncOperation SetLanguageAsync(int languageIndex)
        {
            return new AsyncLanguageChanger(languageIndex);
        }

        class AsyncLanguageChanger : ThreadOperation
        {
            public AsyncLanguageChanger(int index)
            {
                this.index = index;
                this.SubscribeCompleted(OnCompleted);
                Start();
            }

            int index;

            protected override void Operate()
            {
                SetLanguage_Read(index);
            }

            void OnCompleted(AsyncLanguageChanger item)
            {
                TrackAll();
            }
        }


        /// <summary>
        /// 获取到所有语言包文件;
        /// </summary>
        public static ReadOnlyCollection<LanguageFile> SearchLanguagePacks()
        {
            return SearchLanguagePacks(PackDirectoryPath);
        }

        /// <summary>
        /// 获取到所有语言包文件;
        /// </summary>
        public static ReadOnlyCollection<LanguageFile> SearchLanguagePacks(string dirPath)
        {
            var packs = packReader.SearchLanguagePacks(dirPath);
            IList<LanguageFile> packList = packs as IList<LanguageFile> ?? packs.ToArray();
            var collection = new ReadOnlyCollection<LanguageFile>(packList);
            return collection;
        }


        public static LanguagePack ReadPack(LanguageFile file)
        {
            return packReader.Read(file);
        }

        public static LanguageFile CreateAndWrite(LanguagePack pack, string filePath)
        {
            return packReader.CreateAndWrite(pack, filePath);
        }

        public static LocalizationConfig ReadConfig()
        {
            var config = configReader.Read(ConfigFilePath);
            return config;
        }

        public static void WriteConfig(LocalizationConfig config, string filePath)
        {
            configReader.Write(config, filePath);
        }


        /// <summary>
        /// 获取到对应的语言文件下标;
        /// </summary>
        static int FindIndex(IList<LanguageFile> files, LocalizationConfig config)
        {
            for (int i = 0; i < files.Count; i++)
            {
                LanguageFile file = files[i];

                if (file.LocName == config.LocName)
                    return i;
            }
            throw new FileNotFoundException();
        }

    }

}
