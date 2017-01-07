
using System;
using System.IO;
using KouXiaGu.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace KouXiaGu.Localizations
{

    /// <summary>
    /// 负责固定内容的文本;
    /// </summary>
    public sealed class Localization : UnitySington<Localization>
    {

        static Localization()
        {
            IsLoading = true;
        }

        [SerializeField]
        LocalizationConfig config = new LocalizationConfig()
        {
            FollowSystemLanguage = true,
            Language = new string[]
            {
                SystemLanguage.English.ToString(),
                SystemLanguage.ChineseSimplified.ToString(),
            },
        };


        static readonly TextDictionary textDictionary = new TextDictionary();

        static readonly HashSet<ITextObserver> textObservers = new HashSet<ITextObserver>();

#if COLLECT_KEYS
        static readonly HashSet<string> collectKeys = new HashSet<string>();
#endif

        public static bool IsLoading { get; private set; }

        /// <summary>
        /// 重复加入舍弃的;
        /// </summary>
        static readonly List<TextItem> invalidTexts = new List<TextItem>();


        public static IReadOnlyDictionary<string, string> TextDictionary
        {
            get { return textDictionary; }
        }

        public static LocalizationConfig Config
        {
            get { return GetInstance.config; }
            private set { GetInstance.config = value; }
        }

        public static string SysLanguage
        {
            get { return Application.systemLanguage.ToString(); }
        }


        static void Initialize()
        {
            ReadConfigFile();
            LoadLanguage();
            UpdateTextObservers();
            IsLoading = false;
        }

        public static void SetConfig(LocalizationConfig config)
        {
            if (config == null)
                throw new ArgumentNullException();

            Config = config;
            LoadLanguage();
            UpdateTextObservers();
            IsLoading = false;
        }


        /// <summary>
        /// 从磁盘读取配置文件,若不存在,则创建到;
        /// </summary>
        public static void ReadConfigFile()
        {
            try
            {
                var config = Resources.ReadConfig();
                if (config != null)
                {
                    Config = config;
                }
            }
            catch (FileNotFoundException)
            {
                Resources.WriteConfig(Config);
            }
        }

        public static void WriteConfigFile()
        {
            Resources.WriteConfig(Config);
        }


        static void LoadLanguage()
        {
            if (Config.FollowSystemLanguage)
            {
                string language = SysLanguage;
                LoadLanguage(language, Config.Language);
            }
            else
            {
                LoadLanguage(Config.Language);
            }
        }


        static void LoadLanguage(IEnumerable<string> languages)
        {
            var readers = Resources.GetReader(languages);
            ReadTexts(readers);
        }

        static void LoadLanguage(string language, IEnumerable<string> languages)
        {
            List<string> newlanguage = new List<string>();
            newlanguage.Add(language);
            newlanguage.AddRange(languages);

            var readers = Resources.GetReader(newlanguage);
            ReadTexts(readers);
        }


        static void ReadTexts(IEnumerable<ITextReader> readers)
        {
            foreach (var reader in readers)
            {
                ReadTexts(reader);
            }
        }

        static void ReadTexts(ITextReader reader)
        {
            foreach (var item in reader.ReadTexts())
            {
                if (!textDictionary.Add(item))
                    invalidTexts.Add(item);
            }
        }


        public static IDisposable Subscribe(ITextObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException("订阅者为null;");
            if (!textObservers.Add(observer))
                throw new ArgumentException("重复订阅;");

            if (!IsLoading)
                UpdateTextObserver(observer);

#if COLLECT_KEYS
            collectKeys.Add(observer.Key);
#endif

            return new CollectionUnsubscriber<ITextObserver>(textObservers, observer);
        }

        static void UpdateTextObservers()
        {
            foreach (var textObserver in textObservers)
            {
                UpdateTextObserver(textObserver);
            }
        }

        static void UpdateTextObserver(ITextObserver textObserver)
        {
            string text;
            if (textDictionary.TryGetValue(textObserver.Key, out text))
            {
                textObserver.SetText(text);
                return;
            }
            textObserver.OnTextNotFound();
        }


#if COLLECT_KEYS
        /// <summary>
        /// 获取到到现在所有订阅的Key;
        /// </summary>
        public static IEnumerable<string> GetAllSubscribedKeys()
        {
            return collectKeys;
        }
#endif


        static void Clear()
        {
            textDictionary.Clear();
            invalidTexts.Clear();
        }





        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }


    }

}
