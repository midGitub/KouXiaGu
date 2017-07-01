using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 语言包信息;
    /// </summary>
    [XmlType(LanguagePackXmlSerializer.RootName)]
    public class LanguagePackHead
    {
        LanguagePackHead()
        {
        }

        public LanguagePackHead(string name, string language)
        {
            Name = name;
            Language = language.ToLower();
        }

        [XmlAttribute(LanguagePackXmlSerializer.LanguageNameAttributeName)]
        public string Name { get; internal set; }

        string language;

        /// <summary>
        /// 为小写;
        /// </summary>
        [XmlAttribute(LanguagePackXmlSerializer.LanguageAttributeName)]
        public string Language
        {
            get { return language; }
            internal set { language = value.ToLower(); }
        }

        public override string ToString()
        {
            return "[Name:" + Name + ", Language:" + Language + "]";
        }
    }
}
