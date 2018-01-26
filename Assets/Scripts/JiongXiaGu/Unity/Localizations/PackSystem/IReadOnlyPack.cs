namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 只读的语言文本字典;
    /// </summary>
    public interface IReadOnlyPack
    {
        LanguagePackDescription Description { get; }
        IReadOnlyLanguageDictionary LanguageDictionary { get; }
    }
}
