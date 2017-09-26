using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 语言包;
    /// </summary>
    public class LanguagePack
    {
        public LanguagePack(string name, string language)
        {
            Name = name;
            Language = language;
        }

        /// <summary>
        /// 语言包名;
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 语言类型;
        /// </summary>
        [XmlAttribute("language")]
        public string Language { get; set; }

        /// <summary>
        /// 文本字典;
        /// </summary>
        [XmlElement]
        public LanguageDictionary LanguageDictionary { get; set; }
    }
}
