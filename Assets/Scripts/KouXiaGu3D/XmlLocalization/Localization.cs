using System;
using KouXiaGu.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.XmlLocalization
{

    /// <summary>
    /// 文本内容;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Localization : UnitySington<Localization>
    {
        static Localization()
        {
            Initialized = false;
            LanguageIndex = -1;
        }

        Localization() { }

        /// <summary>
        /// 配置信息;
        /// </summary>
        [SerializeField]
        LocalizationConfig config = new LocalizationConfig();


        static readonly TextDictionary textDictionary = new TextDictionary();

        static readonly HashSet<ITextObserver> textObservers = new HashSet<ITextObserver>();


        public static IReadOnlyDictionary<string, string> TextDictionary
        {
            get { return textDictionary; }
        }

        /// <summary>
        /// 是否初始化完毕?
        /// </summary>
        public static bool Initialized { get; private set; }

        public static LocalizationConfig Config
        {
            get { return GetInstance.config; }
            private set { GetInstance.config = value; }
        }

        /// <summary>
        /// 所有语言包(只读);
        /// </summary>
        public static List<XmlLanguageFile> ReadOnlyLanguageFiles { get; private set; }

        /// <summary>
        /// 所有语言(只读),和 ReadOnlyLanguageFiles 对应;
        /// </summary>
        public static List<string> ReadOnlyLanguages { get; private set; }

        /// <summary>
        /// 当前读取的语言下标;
        /// </summary>
        public static int LanguageIndex { get; private set; }


        /// <summary>
        /// 当前读取的语言信息;
        /// </summary>
        public static Language Language
        {
            get { return ReadOnlyLanguageFiles[LanguageIndex].Language; }
        }

        /// <summary>
        /// 当前读取的语言;
        /// </summary>
        public static XmlLanguageFile LanguageFile
        {
            get { return ReadOnlyLanguageFiles[LanguageIndex]; }
        }


        /// <summary>
        /// 初始化所有信息;
        /// </summary>
        public static void Init()
        {
            Config = LocalizationConfig.Read();

            ReadOnlyLanguageFiles = Resources.FindLanguageFiles().ToList();
            ReadOnlyLanguages = ReadOnlyLanguageFiles.Select(item => item.Language.Name).ToList();

            Initialized = true;
        }

        /// <summary>
        /// 是否已经订阅?O(1)
        /// </summary>
        public static bool IsSubecribe(ITextObserver observer)
        {
            return textObservers.Contains(observer);
        }

        /// <summary>
        /// 订阅到文本更新;
        /// </summary>
        public static IDisposable Subscribe(ITextObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException("订阅者为null;");
            if (!textObservers.Add(observer))
                throw new ArgumentException("重复订阅;");

            UpdateTextObserver(observer);

            return new CollectionUnsubscriber<ITextObserver>(textObservers, observer);
        }


        /// <summary>
        /// 更新所有文本内容,在主线程内调用;
        /// </summary>
        public static void UpdateTextObservers()
        {
            foreach (var textObserver in textObservers)
            {
                UpdateTextObserver(textObserver);
            }
        }

        /// <summary>
        /// 更新文本信息,若无法获取到文本内容则返回false;
        /// </summary>
        static void UpdateTextObserver(ITextObserver textObserver)
        {
            textObserver.UpdateTexts(textDictionary);
        }


        /// <summary>
        /// 设置优先读取语言,并且重新读取所有文本;
        /// </summary>
        public static void SetConfig(int languageIndex)
        {
            var languageFile = ReadOnlyLanguageFiles[languageIndex];
            LocalizationConfig config = new LocalizationConfig(languageFile.Language.Name);
            SetConfig(config);
        }

        /// <summary>
        /// 更新配置信息,并且重新读取所有文本;
        /// </summary>
        public static void SetConfig(LocalizationConfig config)
        {
            LocalizationConfig.Write(config);
            Config = config;
            Read(config);
        }

        /// <summary>
        /// 读取到所有文本;
        /// </summary>
        public static void Read()
        {
            Read(Config);
        }

        /// <summary>
        /// 读取\重新读取 所有文本;
        /// </summary>
        static void Read(LocalizationConfig config)
        {
            ITextReader reader = FindReader(config);

            ClearTexts();
            ReadTexts(reader);
            UpdateTextObservers();
        }

        /// <summary>
        /// 寻找到当前最适合读取的语言接口;
        /// </summary>
        static ITextReader FindReader(LocalizationConfig config)
        {
            LanguageIndex = config.FindIndex(ReadOnlyLanguages);
            var reader = Resources.GetReader(LanguageFile);
            return reader;
        }

        static void ReadTexts(ITextReader reader)
        {
            foreach (var item in reader.ReadTexts())
            {
                if (!textDictionary.Add(item))
                    Debug.LogWarning("重复加入的文本条目:" + item);
            }

            Debug.Log("语言读取完毕:" + reader.ToString());
        }

        public static void ClearTexts()
        {
            textDictionary.Clear();
        }


        /// <summary>
        /// 添加文本到字典;
        /// </summary>
        public static void AddTexts(ITextReader reader)
        {
            ReadTexts(reader);
        }


        protected override void Awake()
        {
            base.Awake();
            Init();
            Read();
        }

    }

}
