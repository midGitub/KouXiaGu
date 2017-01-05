using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Localizations
{

    public static class Localization
    {

        /// <summary>
        /// 文本订阅合集;
        /// </summary>
        static readonly ObservableText observableText = new ObservableText();

        /// <summary>
        /// 文本字典;
        /// </summary>
        static readonly CustomDictionary<string, string> textDictionary = new CustomDictionary<string, string>();

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
        /// 加入到文本字典;
        /// </summary>
        public static bool Add(TextPack pack)
        {
            return textDictionary.Add(pack);
        }

        public static bool Add(this Dictionary<string, string> dictionary, TextPack pack)
        {
            if (dictionary.ContainsKey(pack.Key) && !pack.IsUpdate)
            {
                Debug.LogWarning("[本地化]存在相同的字符串:" + pack.ToString());
                return false;
            }

            dictionary.AddOrUpdate(pack.Key, pack.Value);
            return true;
        }



        static Configuration DefaultConfig = new Configuration()
        {
            Language = SystemLanguage.English,
            SecondLanguage = SystemLanguage.ChineseSimplified,
        };

        static Configuration config;

        public static Configuration Config
        {
            get{ return config ?? (config = LoadConfiguration()); }
        }

        const string DESCRIPTION_NAME = "Description.xml";

        /// <summary>
        /// 读取到配置文件,若不存在则返回默认的配置;
        /// </summary>
        public static Configuration LoadConfiguration()
        {
            try
            {
                string filePath = Path.Combine(ResPath, DESCRIPTION_NAME);
                return LoadConfiguration(filePath);
            }
            catch (FileNotFoundException)
            {
                Debug.LogWarning("缺少语言配置文件!");
                return DefaultConfig;
            }
        }

        public static Configuration LoadConfiguration(string filePath)
        {
            var descr = (Configuration)Configuration.Serializer.DeserializeXiaGu(filePath);
            return descr;
        }

        /// <summary>
        /// 保存配置文件,若不存在则保存默认的配置;
        /// </summary>
        public static void SaveConfiguration()
        {
            string filePath = Path.Combine(ResPath, DESCRIPTION_NAME);
            SaveConfiguration(filePath);
        }

        /// <summary>
        /// 保存配置文件,若不存在则保存默认的配置;
        /// </summary>
        public static void SaveConfiguration(string filePath)
        {
            Configuration.Serializer.SerializeXiaGu(filePath, Config);
        }

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



        /// <summary>
        /// 当前系统的语言;
        /// </summary>
        public static SystemLanguage SystemLanguage
        {
            get { return Application.systemLanguage; }
        }



        #region 语言包文件管理;

        /// <summary>
        /// 语言包存放的文件夹;
        /// </summary>
        public const string RES_DIRECTORY = "Localization";

        public static string ResPath
        {
            get { return Path.Combine(ResourcePath.ConfigurationDirectoryPath, RES_DIRECTORY); }
        }



        static readonly Dictionary<string, SystemLanguage> languageDictionary = GetLanguageDictionary();

        static Dictionary<string, SystemLanguage> GetLanguageDictionary()
        {
            var languagesArray = Enum.GetValues(typeof(SystemLanguage));
            var languageDictionary = new Dictionary<string, SystemLanguage>(languagesArray.Length);

            foreach (SystemLanguage language in languagesArray)
            {
                string key = language.ToString();
                languageDictionary.AddOrUpdate(key, language);
            }

            return languageDictionary;
        }


        /// <summary>
        /// 语言文件匹配的搜索字符串;
        /// </summary>
        public const string LANGUAGE_PACK_SEARCH_PATTERN = "*" + XmlFile.FILE_EXTENSION;

        /// <summary>
        /// 寻找目录下存在的语言和其路径;
        /// </summary>
        public static IEnumerable<KeyValuePair<SystemLanguage, string>> LanguagePackExists(string directoryPath, SearchOption searchOption)
        {
            var paths = Directory.GetFiles(directoryPath, LANGUAGE_PACK_SEARCH_PATTERN, searchOption);

            foreach (var path in paths)
            {
                SystemLanguage language;
                string fileName = Path.GetFileNameWithoutExtension(path);

                if (languageDictionary.TryGetValue(fileName, out language))
                    yield return new KeyValuePair<SystemLanguage, string>(language, path);
            }
        }

        #endregion


    }

}
