using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 语言包;
    /// </summary>
    public class LanguagePack
    {
        /// <summary>
        /// 描述;
        /// </summary>
        public LanguagePackDescription Description { get; set; }

        /// <summary>
        /// 文本字典;
        /// </summary>
        public LanguageDictionary LanguageDictionary { get; set; }

        public LanguagePack(LanguagePackDescription description) : this(description, new LanguageDictionary())
        {
        }

        public LanguagePack(LanguagePackDescription description, LanguageDictionary languageDictionary)
        {
            Description = description;
            LanguageDictionary = languageDictionary;
        }
    }
}
