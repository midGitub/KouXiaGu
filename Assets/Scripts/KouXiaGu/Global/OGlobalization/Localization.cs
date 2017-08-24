using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using KouXiaGu.Collections;
using KouXiaGu.Resources;
using KouXiaGu.Concurrent;

namespace KouXiaGu.OGlobalization
{

    /// <summary>
    /// 本地化语言文本管理;
    /// </summary>
    [Obsolete("Use KouXiaGu.Globalization.Localization")]
    public static class Localization
    {
        static Localization()
        {
            IsInitialized = false;
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

        static readonly HashSet<ITextObserver> textObservers = new HashSet<ITextObserver>();

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
        /// 通知到所有观察者,文本内容发生变化,若在非Unity线程调用,则为延迟通知;
        /// </summary>
        public static void TrackAll()
        {
            if (textDictionary == null)
                throw new ArgumentNullException("textDictionary");

            ITextObserver[] observerArray = textObservers.ToArray();

            if (XiaGu.IsUnityThread)
            {
                foreach (var observer in observerArray)
                {
                    TrackObserver(observer);
                }
            }
            else
            {
                UnityAsyncRequestDispatcher dispatcher = UnityAsyncRequestDispatcher.Instance;
                TrackAllRequest operation = new TrackAllRequest(observerArray, textDictionary);
                dispatcher.Add(operation);
            }
        }

        class TrackAllRequest : IAsyncRequest
        {
            public TrackAllRequest(IEnumerable<ITextObserver> observers, IDictionary<string, string> textDictionary)
            {
                this.observers = observers;
                this.textDictionary = textDictionary;
            }

            IEnumerable<ITextObserver> observers;
            IDictionary<string, string> textDictionary;

            void IAsyncRequest.OnAddQueue()
            {
            }

            bool IAsyncRequest.Prepare()
            {
                return true;
            }

            bool IAsyncRequest.Operate()
            {
                foreach (var observer in observers)
                {
                    observer.UpdateTexts(textDictionary);
                }
                return false;
            }

            void IAsyncRequest.OnQuitQueue()
            {
            }
        }


        public static bool IsInitialized { get; private set; }

        internal static void Initialize()
        {
            if (IsInitialized)
                throw new ArgumentException("已经初始化;");

            ConfigReader configReader = new ConfigReader();
            var config = configReader.Read();
            var originalLanguages = Languages;
            var originalLanguagePack = LanguagePack;
            var originalLanguageIndex = LanguageIndex;
            try
            {
                Languages = SearchLanguagePacks();
                LanguageIndex = FindIndex(Languages, config);
                LanguagePack = ReadPack(Languages[LanguageIndex]);
                TrackAll();
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
        /// 获取到所有语言文件;
        /// </summary>
        public static ReadOnlyCollection<LanguageFile> SearchLanguagePacks()
        {
            return SearchLanguagePacks(PackDirectoryPath);
        }

        /// <summary>
        /// 获取到所有语言文件;
        /// </summary>
        public static ReadOnlyCollection<LanguageFile> SearchLanguagePacks(string dirPath)
        {
            var packs = packReader.SearchLanguagePacks(dirPath);
            IList<LanguageFile> packList = packs as IList<LanguageFile> ?? packs.ToArray();
            var collection = new ReadOnlyCollection<LanguageFile>(packList);
            return collection;
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

        static readonly LanguagePackReader packReader = new XmlLanguagePackReader();

        public static LanguagePack ReadPack(LanguageFile file)
        {
            return packReader.Read(file);
        }

        public static LanguageFile CreateAndWrite(LanguagePack pack, string filePath)
        {
            return packReader.CreateAndWrite(pack, filePath);
        }

        /// <summary>
        /// 设置到语言;传入语言下标;
        /// </summary>
        public static void SetLanguage(int languageIndex)
        {
            var originalLanguagePack = LanguagePack;
            var originalLanguageIndex = LanguageIndex;
            try
            {
                LanguagePack = ReadPack(Languages[languageIndex]);
                LanguageIndex = languageIndex;
                TrackAll();
            }
            catch (Exception ex)
            {
                LanguagePack = originalLanguagePack;
                LanguageIndex = originalLanguageIndex;
                throw ex;
            }
        }
    }
}
