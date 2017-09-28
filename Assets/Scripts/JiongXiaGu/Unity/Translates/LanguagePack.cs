using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Translates
{

    /// <summary>
    /// 语言包;
    /// </summary>
    [XmlRoot(LanguagePackReader.LanguagePackInfoRootName)]
    public class LanguagePack
    {
        /// <summary>
        /// 语言包名;
        /// </summary>
        [XmlAttribute(LanguagePackReader.NameXmlAttributeName)]
        public string Name { get; set; }

        /// <summary>
        /// 语言类型;
        /// </summary>
        [XmlAttribute(LanguagePackReader.LanguageXmlAttributeName)]
        public string Language { get; set; }

        /// <summary>
        /// 文本字典;
        /// </summary>
        [XmlElement]
        public LanguageDictionary LanguageDictionary { get; set; }

        public LanguagePack(string name, string language)
        {
            Name = name;
            Language = language;
            LanguageDictionary = new LanguageDictionary();
        }

        public static implicit operator LanguagePackInfo(LanguagePack info)
        {
            return new LanguagePackInfo(info.Name, info.Language);
        }
    }
}
