namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可加载资源描述;
    /// </summary>
    public struct LoadableContentDescription
    {
        /// <summary>
        /// 唯一标识;
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 模组名称;
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版本;
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 标签;
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        public string Message { get; set; }

        internal LoadableContentDescription(string id, string name) : this()
        {
            ID = id;
            Name = name;
        }
    }
}
