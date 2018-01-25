using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 语言包描述;
    /// </summary>
    public struct LanguagePackDescription
    {
        /// <summary>
        /// 语言包名;
        /// </summary>
        [XmlElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 语言类型;
        /// </summary>
        [XmlElement("Language")]
        public string Language { get; set; }

        /// <summary>
        /// 是否为默认的语言包?
        /// </summary>
        [XmlElement("IsDefault")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 是否为补充包?
        /// </summary>
        [XmlElement("IsSupplementary")]
        public bool IsSupplementary { get; set; }

        /// <summary>
        /// 版本;
        /// </summary>
        [XmlElement("Version")]
        public string Version { get; set; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        [XmlElement("Message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("[Name:{0}, Language:{1}, Version:{2}, Message:{3}]", Name, Language, Version, Message);
        }
    }
}
