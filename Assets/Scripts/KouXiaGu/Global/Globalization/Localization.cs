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
        public static LocalizationConfig Config { get; private set; }
        public static ReadOnlyCollection<LanguagePackFile> Languages { get; private set; }
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
        /// 设置新的 文本字典, 需要手动 TrackAll() 所有订阅者;
        /// </summary>
        public static void SetLanguagePack(LanguagePack pack)
        {
            if (pack == null)
                throw new ArgumentNullException();

            LanguagePack = pack;
        }

        /// <summary>
        /// 设置到语言;传入语言下标;
        /// </summary>
        public static void SetLanguage(int languageIndex)
        {

        }


        public static IAsyncOperation InitializeAsync()
        {
            return new LanguageInitializer();
        }

        class LanguageInitializer : ThreadOperation
        {
            public LanguageInitializer()
            {
                this.Subscribe(OnCompleted, OnFaulted);
                Start();
            }

            protected override void Operate()
            {
                Languages = SearchLanguagePacks();
                Config = ReadConfig();
                LanguageIndex = FindIndex(Languages, Config);
                LanguagePack = ReadPack(Languages[LanguageIndex]);
            }

            void OnCompleted(LanguageInitializer item)
            {
                TrackAll();
            }

            void OnFaulted(LanguageInitializer item)
            {
                Debug.LogError("语言读取失败;Ex:" + item.Exception);
            }

            /// <summary>
            /// 获取到所有语言包文件;
            /// </summary>
            public ReadOnlyCollection<LanguagePackFile> SearchLanguagePacks()
            {
                return SearchLanguagePacks(PackDirectoryPath);
            }

            /// <summary>
            /// 获取到所有语言包文件;
            /// </summary>
            public ReadOnlyCollection<LanguagePackFile> SearchLanguagePacks(string dirPath)
            {
                var packs = packReader.SearchLanguagePacks(dirPath);
                IList<LanguagePackFile> packList = packs as IList<LanguagePackFile> ?? packs.ToArray();
                var collection = new ReadOnlyCollection<LanguagePackFile>(packList);
                return collection;
            }

            public LocalizationConfig ReadConfig()
            {
                return configReader.Read(ConfigFilePath);
            }

            public int FindIndex(IList<LanguagePackFile> files, LocalizationConfig config)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    LanguagePackFile file = files[i];

                    if (file.LocName == config.LocName)
                        return i;
                }
                throw new FileNotFoundException();
            }

            public LanguagePack ReadPack(LanguagePackFile file)
            {
                return packReader.Read(file);
            }

        }

    }

    /// <summary>
    /// 语言文件读取方法;
    /// </summary>
    public class LocalizationInfo
    {
        static readonly LanguagePackReader languagerReader = new XmlLanguagePackReader();
        static readonly XmlLocalizationConfigReader configReader = new XmlLocalizationConfigReader();

        const string LocalizationConfigFileName = "Localization/Config.xml";
        const string LanguagePackDirectoryName = "Localization";

        public static IAsyncOperation<LocalizationInfo> ReadAsync()
        {
            throw new NotImplementedException();
        }

        public static Dictionary<string, string> Read(LanguagePackFile pack)
        {
            throw new NotImplementedException();
        }

        public static IAsyncOperation<Dictionary<string, string>> ReadAsync(LanguagePackFile pack)
        {
            throw new NotImplementedException();
        }

        LocalizationInfo()
        {
        }

        public Dictionary<string, string> Texts { get; private set; }
        public LocalizationConfig Config { get; private set; }
        public ReadOnlyCollection<LanguagePackFile> Languages { get; private set; }
        public int LanguageIndex { get; private set; }

        public LanguagePackFile LanguagePack
        {
            get { return Languages[LanguageIndex]; }
        }

        /// <summary>
        /// 获取到所有语言包文件;
        /// </summary>
        public ReadOnlyCollection<LanguagePackFile> SearchLanguagePacks()
        {
            return SearchLanguagePacks(LanguagePackDirectoryName);
        }

        /// <summary>
        /// 获取到所有语言包文件;
        /// </summary>
        public ReadOnlyCollection<LanguagePackFile> SearchLanguagePacks(string dirPath)
        {
            var packs = languagerReader.SearchLanguagePacks(dirPath);
            IList<LanguagePackFile> packList = packs as IList<LanguagePackFile> ?? packs.ToArray();
            var collection = new ReadOnlyCollection<LanguagePackFile>(packList);
            return collection;
        }



    }

    [XmlType("LocalizationConfig")]
    public struct LocalizationConfig
    {
        [XmlElement("LocName")]
        public string LocName;
    }


    public class XmlLocalizationConfigReader
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(LocalizationConfig));

        public LocalizationConfig Read(string filePath)
        {
            return (LocalizationConfig)serializer.DeserializeXiaGu(filePath);
        }

        public void Write(LocalizationConfig item, string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath);

            if (Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            serializer.SerializeXiaGu(filePath, item);
        }
    }

}
