namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 控制台方法描述;
    /// </summary>
    public struct ConsoleMethodDescription
    {
        /// <summary>
        /// 完全名,如 ConsoleMethod.Info.Run ;
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 参数描述;
        /// </summary>
        public string[] Parameter { get; set; }
    }
}
