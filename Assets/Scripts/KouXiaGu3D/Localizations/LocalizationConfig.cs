using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Localizations
{

    /// <summary>
    /// 配置信息;
    /// </summary>
    [XmlType("LocalizationConfiguration"), Serializable]
    public sealed class LocalizationConfig
    {

        static readonly XmlSerializer serializer = new XmlSerializer(typeof(LocalizationConfig));

        public static XmlSerializer Serializer
        {
            get { return serializer; }
        }

        /// <summary>
        /// 跟随系统语言;
        /// </summary>
        [XmlElement("FollowSystemLanguage")]
        public bool FollowSystemLanguage;

        /// <summary>
        /// 指定使用的语言;
        /// </summary>
        [XmlArray("LanguagePriority"), XmlArrayItem("Language")]
        public string[] Language;

    }


}
