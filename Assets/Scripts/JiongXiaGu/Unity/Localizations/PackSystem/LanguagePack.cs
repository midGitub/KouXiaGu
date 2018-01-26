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
        public LanguagePackDescription Description { get; private set; }

        /// <summary>
        /// 文本字典;
        /// </summary>
        public ILanguageDictionary LanguageDictionary { get; private set; }

        IReadOnlyLanguageDictionary IReadOnlyPack.LanguageDictionary => LanguageDictionary;

        public LanguagePack(LanguagePackDescription description) : this(description, null)
        {
        }

        public LanguagePack(LanguagePackDescription description, ILanguageDictionary languageDictionary)
        {
            Description = description;
            LanguageDictionary = languageDictionary;
        }
    }
}
