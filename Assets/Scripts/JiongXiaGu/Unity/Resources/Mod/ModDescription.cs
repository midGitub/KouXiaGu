namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组描述;
    /// </summary>
    public struct ModDescription
    {
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
    }
}
