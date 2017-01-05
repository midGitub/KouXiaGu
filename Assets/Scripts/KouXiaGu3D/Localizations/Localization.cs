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

    public class Localization : UnitySington<Localization>
    {

        #region 语言包文件管理;

        /// <summary>
        /// 语言文件匹配的搜索字符串;
        /// </summary>
        public const string LANGUAGE_PACK_SEARCH_PATTERN = "*" + XmlFile.FILE_EXTENSION;

        static readonly Dictionary<string, SystemLanguage> languageDictionary = GetLanguageDictionary();

        /// <summary>
        /// 语言包存放的文件夹;
        /// </summary>
        public const string RES_DIRECTORY = "Localization";

        public static string resPath
        {
            get { return Path.Combine(ResourcePath.ConfigurationDirectoryPath, RES_DIRECTORY); }
        }


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
        /// 寻找目录下存在的语言和其路径;
        /// </summary>
        public static IEnumerable<KeyValuePair<SystemLanguage, string>> LanguagePackExists(string directoryPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
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


        #region 本地化文本订阅器;

        static readonly Dictionary<string, string> textDictionary = new Dictionary<string, string>();

        static readonly HashSet<ITextObserver> textObservers = new HashSet<ITextObserver>();

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
        /// 取消订阅文本更新;
        /// </summary>
        public static bool Unsubscribe(ITextObserver observer)
        {
            return textObservers.Remove(observer);
        }

        /// <summary>
        /// 更新所有文本观察者(应该在Unity线程中);
        /// </summary>
        public static void UpdateTextObservers()
        {
            foreach (var textObserver in textObservers)
            {
                UpdateTextObserver(textObserver);
            }
        }

        /// <summary>
        /// 更新文本观察者内容;
        /// </summary>
        public static void UpdateTextObserver(ITextObserver textObserver)
        {
            string text;
            if (textDictionary.TryGetValue(textObserver.Key, out text))
            {
                textObserver.SetText(text);
            }
            Debug.LogWarning("[本地化]未知字符串:" + textObserver.Key);
        }

        /// <summary>
        /// 加入到文本字典;
        /// </summary>
        public static bool AddText(TextPack pack)
        {
            if (textDictionary.ContainsKey(pack.Key) && !pack.IsUpdate)
            {
                Debug.LogWarning("[本地化]存在相同的字符串:" + pack.ToString());
                return false;
            }

            textDictionary.AddOrUpdate(pack.Key, pack.Value);
            return true;
        }

        #endregion


        #region 初始化信息;

        /// <summary>
        /// 当前系统的语言;
        /// </summary>
        public static SystemLanguage SystemLanguage
        {
            get { return Application.systemLanguage; }
        }


        [XmlType("Localization")]
        public struct LocalizationDescr
        {

            static readonly XmlSerializer serializer = new XmlSerializer(typeof(LocalizationDescr));

            public static XmlSerializer Serializer
            {
                get { return serializer; }
            }

            /// <summary>
            /// 指定使用的语言;
            /// </summary>
            [XmlElement("Language")]
            public SystemLanguage Language;
        }

        #endregion

    }

}
