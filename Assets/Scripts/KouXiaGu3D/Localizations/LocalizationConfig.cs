using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Localizations
{

    /// <summary>
    /// 本地化配置;
    /// </summary>
    [XmlType("LocalizationConfiguration"), Serializable]
    public sealed class LocalizationConfig
    {

        /// <summary>
        /// 配置文件名;
        /// </summary>
        const string DESCRIPTION_NAME = "Localization.xml";


        static readonly XmlSerializer serializer = new XmlSerializer(typeof(LocalizationConfig));


        public static string ConfigFilePath
        {
            get { return Path.Combine(Resources.ResDirectoryPath, DESCRIPTION_NAME); }
        }

        public static XmlSerializer Serializer
        {
            get { return serializer; }
        }


        /// <summary>
        /// 从文件读取到配置信息;
        /// </summary>
        public static LocalizationConfig Read()
        {
            var descr = (LocalizationConfig)Serializer.DeserializeXiaGu(ConfigFilePath);
            return descr;
        }

        /// <summary>
        /// 保存配置到文件;
        /// </summary>
        public static void Write(LocalizationConfig config)
        {
            Serializer.SerializeXiaGu(ConfigFilePath, config);
        }



        /// <summary>
        /// 是否跟随系统语言;
        /// </summary>
        [XmlElement("IsFollowSystemLanguage")]
        public bool IsFollowSystemLanguage;

        /// <summary>
        /// 指定使用的语言;
        /// </summary>
        [XmlArray("LanguagePriority"), XmlArrayItem("Language")]
        public string[] LanguagePrioritys;


        /// <summary>
        /// 系统语言;
        /// </summary>
        public static string SysLanguage
        {
            get { return Application.systemLanguage.ToString(); }
        }


        /// <summary>
        /// 获取到定于的语言优先顺序;
        /// </summary>
        public IEnumerable<string> GetLanguages()
        {
            if (IsFollowSystemLanguage)
            {
                List<string> newlanguage = new List<string>();
                newlanguage.Add(SysLanguage);
                newlanguage.AddRange(LanguagePrioritys);

                return newlanguage;
            }
            else
            {
                return LanguagePrioritys;
            }
        }

    }


}
