namespace JiongXiaGu.Unity.GameConsoles
{
    /// <summary>
    /// 控制台事件;
    /// </summary>
    public struct ConsoleEvent
    {
        /// <summary>
        /// 事件类型;
        /// </summary>
        public ConsoleEventType EventType { get; set; }

        /// <summary>
        /// 消息;
        /// </summary>
        public string Message { get; set; }

        public ConsoleEvent(ConsoleEventType type, string message)
        {
            EventType = type;
            Message = message;
        }

        public ConsoleEvent(ConsoleEventType type, string format, params object[] args)
        {
            EventType = type;
            Message = string.Format(format, args);
        }
    }
}
