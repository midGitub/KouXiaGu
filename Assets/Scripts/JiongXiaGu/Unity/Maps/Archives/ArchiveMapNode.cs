namespace JiongXiaGu.Unity.Maps
{
    public struct ArchiveMapNode
    {
        public MapNode? Node { get; set; }
        public bool IsRemove => !Node.HasValue;

        public ArchiveMapNode(MapNode value)
        {
            Node = value;
        }

        public bool ShouldSerializeValue()
        {
            return Node != null;
        }
    }
}
