using System;
using System.IO;
using KouXiaGu.Collections;
using System.Collections;
using System.Xml.Serialization;
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
            Language = SystemLanguage.English.ToString(),
            SecondLanguage = SystemLanguage.ChineseSimplified.ToString(),
        };


        static readonly TextDictionary textDictionary = new TextDictionary();

        static readonly HashSet<ITextObserver> textObservers = new HashSet<ITextObserver>();

        public static bool IsLoading { get; private set; }

        /// <summary>
        /// 重复加入舍弃的;
        /// </summary>
        static readonly List<TextPack> invalidTexts = new List<TextPack>();


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
        }

        public static void SetConfig(LocalizationConfig config)
        {
            Config = config;
            LoadLanguage();
            UpdateTextObservers();
        }


        public static void ReadConfigFile()
        {
            var config = Resources.ReadConfig();
            if (config != null)
            {
                Config = config;
            }
        }

        public static void WriteConfigFile()
        {
            Resources.WriteConfig(Config);
        }


        static void LoadLanguage()
        {
            string language;
            string secondLanguage;

            if (Config.FollowSystemLanguage)
            {
                language = SysLanguage;
                secondLanguage = Config.Language;
            }
            else
            {
                language = Config.Language;
                secondLanguage = Config.SecondLanguage;
            }

            LoadLanguage(language, secondLanguage);
        }

        static void LoadLanguage(string language, string secondLanguage)
        {
            var readers = Resources.GetTextReader(language, secondLanguage);
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
            }
            textObserver.OnTextNotFound();
        }


        static void Clear()
        {
            textDictionary.Clear();
            invalidTexts.Clear();
        }


        #region 实例部分;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        #endregion


    }

}
