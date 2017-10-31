namespace JiongXiaGu.Unity.GameConsoles
{
    /// <summary>
    /// 控制台事件类型;
    /// </summary>
    public enum ConsoleEventType
    {
        /// <summary>
        /// 正常消息;
        /// </summary>
        Normal,
        
        /// <summary>
        /// 成功消息;
        /// </summary>
        Successful,

        /// <summary>
        /// 警告消息;
        /// </summary>
        Warning,

        /// <summary>
        /// 错误消息;
        /// </summary>
        Error,

        /// <summary>
        /// 执行对应方法;
        /// </summary>
        Method,
    }
}
