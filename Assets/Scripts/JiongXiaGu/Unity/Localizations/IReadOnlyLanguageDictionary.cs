namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 只读的语言文本字典;
    /// </summary>
    public interface IReadOnlyLanguageDictionary
    {
        /// <summary>
        /// 语言类型;
        /// </summary>
        string Language { get; }

        /// <summary>
        /// 尝试获取到翻译文本,若未能获取到返回false;
        /// </summary>
        bool TryTranslate(string key, out string value);
    }
}
