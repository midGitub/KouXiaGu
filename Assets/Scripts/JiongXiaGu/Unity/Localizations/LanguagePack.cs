using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
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

        LanguagePack()
        {
        }

        public LanguagePack(string name, string language) : this(name, language, new LanguageDictionary())
        {
        }

        public LanguagePack(string name, string language, LanguageDictionary languageDictionary)
        {
            Name = name;
            Language = language;
            LanguageDictionary = languageDictionary;
        }

        public static implicit operator LanguagePackInfo(LanguagePack info)
        {
            return new LanguagePackInfo(info.Name, info.Language);
        }
    }
}
