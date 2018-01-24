using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 语言包;
    /// </summary>
    public class LanguagePack : IReadOnlyPack
    {
        /// <summary>
        /// 描述;
        /// </summary>
        public LanguagePackDescription Description { get; set; }

        /// <summary>
        /// 文本字典;
        /// </summary>
        public ILanguageDictionary LanguageDictionary { get; set; }

        /// <summary>
        /// 语言名;
        /// </summary>
        public string Language => Description.Language;

        public LanguagePack(LanguagePackDescription description) : this(description, null)
        {
        }

        public LanguagePack(LanguagePackDescription description, ILanguageDictionary languageDictionary)
        {
            Description = description;
            LanguageDictionary = languageDictionary;
        }

        public bool TryTranslate(string key, out string value)
        {
            return LanguageDictionary.TryGetValue(key, out value);
        }
    }
}
