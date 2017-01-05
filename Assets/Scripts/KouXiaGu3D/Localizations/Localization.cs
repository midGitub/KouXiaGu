using System;
using System.IO;
using KouXiaGu.Collections;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Localizations
{


    public sealed class Localization : UnitySington<Localization>
    {

        /// <summary>
        /// 文本订阅合集;
        /// </summary>
        static readonly ObservableText observableText = new ObservableText();

        /// <summary>
        /// 文本字典;
        /// </summary>
        static readonly TextDictionary textDictionary = new TextDictionary();

        /// <summary>
        /// 监视文本变化;
        /// </summary>
        public static IObservableText ObservableText
        {
            get { return observableText; }
        }

        /// <summary>
        /// 文本字典;
        /// </summary>
        public static IReadOnlyDictionary<string, string> TextDictionary
        {
            get { return textDictionary; }
        }

        /// <summary>
        /// 当前系统的语言;
        /// </summary>
        public static SystemLanguage SystemLanguage
        {
            get { return Application.systemLanguage; }
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

    }

}
