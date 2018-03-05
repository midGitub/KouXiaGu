namespace JiongXiaGu.Unity.Maps
{
    /// <summary>
    /// 地图变化事件;
    /// </summary>
    public struct MapEvent<TKey>
    {
        public Map<TKey> Map { get; set; }
        public DictionaryEventType EventType { get; set; }
        public TKey Key { get; set; }
        public MapNode OriginalValue { get; set; }
        public MapNode NewValue { get; set; }
        public MapNodeChangeContents ChangeContents { get; set; }
    }
}
