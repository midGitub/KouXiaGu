namespace JiongXiaGu.Unity.Localizations
{
    /// <summary>
    /// 语言变化事件;
    /// </summary>
    public struct LanguageChangedEvent
    {
        /// <summary>
        /// 文本字典;
        /// </summary>
        public IReadOnlyPack LanguageDictionary { get; set; }
    }
}
