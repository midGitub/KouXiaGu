using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 语言配置信息;
    /// </summary>
    public struct LocalizationConfig
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
    }
}
