namespace JiongXiaGu.Unity.GameConsoles
{
    /// <summary>
    /// 控制台消息条目;
    /// </summary>
    public struct ConsoleRecordItem
    {
        public ConsoleRecordItem(ConsoleRecordTypes type, string message)
        {
            Type = type;
            Message = message;
        }

        /// <summary>
        /// 消息类型;
        /// </summary>
        public ConsoleRecordTypes Type { get; private set; }

        /// <summary>
        /// 消息;
        /// </summary>
        public string Message { get; private set; }
    }
}
