using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using KouXiaGu.Collections;
using KouXiaGu.Resources;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 本地化语言文本管理;
    /// </summary>
    public static class Localization
    {
        static Localization()
        {
            IsInitializing = false;
            LanguageIndex = -1;
        }

        static string ConfigFilePath
        {
            get { return Path.Combine(Resource.ConfigDirectoryPath, "Localization/Config.xml"); }
        }

        static string PackDirectoryPath
        {
            get { return Path.Combine(Resource.ConfigDirectoryPath, "Localization"); }
        }

        static readonly LanguagePackReader packReader = new XmlLanguagePackReader();
        static readonly XmlLocalizationConfigReader configReader = new XmlLocalizationConfigReader();
        static readonly HashSet<ITextObserver> textObservers = new HashSet<ITextObserver>();

        /// <summary>
        /// 是否正在读取/初始化?
        /// </summary>
        public static bool IsInitializing { get; private set; }

        /// <summary>
        /// 当前使用的语言;
        /// </summary>
        static LanguagePack LanguagePack { get; set; }

        /// <summary>
        /// 语言列表;
        /// </summary>
        public static ReadOnlyCollection<LanguageFile> Languages { get; private set; }

        /// <summary>
        /// 若不存在,则设为-1;
        /// </summary>
        public static int LanguageIndex { get; private set; }

        /// <summary>
        /// 当前使用的文本字典;
        /// </summary>
        public static IDictionary<string, string> textDictionary
        {
            get { return LanguagePack == null ? null : LanguagePack.TextDictionary; }
        }

        /// <summary>
        /// 当前使用的语言;
        /// </summary>
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
            observer.UpdateTexts(textDictionary);
        }

        /// <summary>
        /// 需要在Unity线程内调用,通知到所有观察者,文本内容发生变化;
        /// </summary>
        public static void TrackAll()
        {
            if (textDictionary == null)
                throw new ArgumentNullException("textDictionary");

            ITextObserver[] observerArray = textObservers.ToArray();
            foreach (var observer in observerArray)
            {
                TrackObserver(observer);
            }
        }


        static void OnStartInitialize()
        {
            if (IsInitializing)
                throw new ArgumentException();

            IsInitializing = true;
        }

        static void OnEndInitialize()
        {
            IsInitializing = false;
        }

        /// <summary>
        /// 同步的初始化;
        /// </summary>
        public static void Initialize()
        {
            OnStartInitialize();
            Initialize_Read();
            TrackAll();
            OnEndInitialize();
        }

        /// <summary>
        /// 初始化方法允许多线程部分;
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
            catch (Exception ex)
            {
                Languages = originalLanguages;
                LanguageIndex = originalLanguageIndex;
                LanguagePack = originalLanguagePack;
                throw ex;
            }
        }

        /// <summary>
        /// 异步的初始化;
        /// </summary>
        public static IAsyncOperation InitializeAsync()
        {
            OnStartInitialize();
            return new AsyncInitializer();
        }

        class AsyncInitializer : ThreadOperation
        {
            public AsyncInitializer()
            {
                this.SubscribeCompleted(this, OnCompleted);
                Start();
            }

            protected override void Operate()
            {
                Initialize_Read();
            }

            void OnCompleted(AsyncInitializer item)
            {
                TrackAll();
                OnEndInitialize();
            }
        }


        /// <summary>
        /// 设置到语言;传入语言下标;
        /// </summary>
        public static void SetLanguage(int languageIndex)
        {
            OnStartInitialize();
            SetLanguage_Read(languageIndex);
            TrackAll();
            OnEndInitialize();
        }

        /// <summary>
        /// 变更语言允许多线程部分;
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
            OnStartInitialize();
            return new AsyncLanguageChanger(languageIndex);
        }

        class AsyncLanguageChanger : ThreadOperation
        {
            public AsyncLanguageChanger(int index)
            {
                this.index = index;
                this.SubscribeCompleted(this, OnCompleted);
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
                OnEndInitialize();
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
