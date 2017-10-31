namespace JiongXiaGu.Unity.GameConsoles
{
    /// <summary>
    /// 控制台事件;
    /// </summary>
    public struct ConsoleEvent
    {
        public ConsoleEvent(ConsoleEventType type, string message)
        {
            EventType = type;
            Message = message;
        }

        /// <summary>
        /// 事件类型;
        /// </summary>
        public ConsoleEventType EventType { get; set; }

        /// <summary>
        /// 消息;
        /// </summary>
        public string Message { get; set; }
    }
}
