namespace JiongXiaGu.Unity.Maps
{
    public struct ArchiveMapNode
    {
        public MapNode? Value { get; set; }
        public bool IsRemove => !Value.HasValue;

        public ArchiveMapNode(MapNode value)
        {
            Value = value;
        }

        public bool ShouldSerializeValue()
        {
            return Value != null;
        }
    }
}
