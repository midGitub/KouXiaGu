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

        /// <summary>
        /// 文本字典;
        /// </summary>
        static readonly TextDictionary textDictionary = new TextDictionary();

        /// <summary>
        /// 文本字典;
        /// </summary>
        public static IReadOnlyDictionary<string, string> TextDictionary
        {
            get { return textDictionary; }
        }

        static void Clear()
        {
            textDictionary.Clear();
        }

        #region 外部资源;

        /// <summary>
        /// 语言包存放的文件夹;
        /// </summary>
        const string RES_DIRECTORY = "Localization";

        const string DESCRIPTION_NAME = "Description.xml";

        static Configuration DefaultConfig = new Configuration()
        {
            Language = SystemLanguage.English,
            SecondLanguage = SystemLanguage.ChineseSimplified,
        };

        static Configuration config;

        public static string ResPath
        {
            get { return Path.Combine(ResourcePath.ConfigurationDirectoryPath, RES_DIRECTORY); }
        }

        public static Configuration Config
        {
            get{ return config ?? (config = LoadConfiguration()); }
        }

        public static string ConfigFilePath
        {
            get { return Path.Combine(ResPath, DESCRIPTION_NAME); }
        }

        /// <summary>
        /// 读取到配置文件,若不存在则返回默认的配置;
        /// </summary>
        public static Configuration LoadConfiguration()
        {
            try
            {
                var descr = (Configuration)Configuration.Serializer.DeserializeXiaGu(ConfigFilePath);
                return descr;
            }
            catch (FileNotFoundException)
            {
                Debug.LogWarning("缺少语言配置文件!");
                return DefaultConfig;
            }
        }

        /// <summary>
        /// 保存配置文件,若不存在则保存默认的配置;
        /// </summary>
        public static void SaveConfiguration()
        {
            Configuration.Serializer.SerializeXiaGu(ConfigFilePath, Config);
        }

        /// <summary>
        /// 配置信息;
        /// </summary>
        [XmlType("LocalizationConfiguration"), Serializable]
        public sealed class Configuration
        {

            static readonly XmlSerializer serializer = new XmlSerializer(typeof(Configuration));

            public static XmlSerializer Serializer
            {
                get { return serializer; }
            }

            /// <summary>
            /// 指定使用的语言;
            /// </summary>
            [XmlElement("Language")]
            public SystemLanguage Language;

            /// <summary>
            /// 备用语言;
            /// </summary>
            [XmlElement("SecondLanguage")]
            public SystemLanguage SecondLanguage;

        }

        #endregion


        #region 实例部分;

        /// <summary>
        /// 文本订阅合集;
        /// </summary>
        static readonly HashSet<ITextObserver> textObservers = new HashSet<ITextObserver>();

        static bool isLoading = false;


        /// <summary>
        /// 当前系统的语言;
        /// </summary>
        public static SystemLanguage SystemLanguage
        {
            get { return Application.systemLanguage; }
        }


        void Start()
        {
            StartLoadPack();
        }

        public void StartLoadPack()
        {
            if(!isLoading)
                StartCoroutine(LoadPack());
        }

        IEnumerator LoadPack()
        {
            isLoading = true;
            Clear();

            ITextReader reader = GetTextReader();

            Add(reader);
            UpdateTextObservers();

            isLoading = false;
            yield break;
        }

        void Add(ITextReader reader)
        {
            foreach (var item in reader.ReadTexts())
            {
                if (!textDictionary.Add(item))
                {
                    Debug.LogWarning("存在相同的字符:" + item.ToString());
                }
            }
        }

        ITextReader GetTextReader()
        {
            List<LanguagePack> pack = new List<LanguagePack>(XmlFile.LanguagePackExists(ResPath, SearchOption.TopDirectoryOnly));
            return pack[0].Reader;
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

            return new CollectionUnsubscriber<ITextObserver>(textObservers, observer);
        }

        /// <summary>
        /// 更新所有文本(应该在Unity线程中);
        /// </summary>
        void UpdateTextObservers()
        {
            foreach (var textObserver in textObservers)
            {
                UpdateTextObserver(textObserver);
            }
        }

        /// <summary>
        /// 更新文本观察者内容;
        /// </summary>
        void UpdateTextObserver(ITextObserver textObserver)
        {
            string text;
            if (textDictionary.TryGetValue(textObserver.Key, out text))
            {
                textObserver.SetText(text);
            }
            textObserver.OnTextNotFound();
        }

        #endregion


    }

}
