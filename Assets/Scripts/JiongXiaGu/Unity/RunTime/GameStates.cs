namespace JiongXiaGu.Unity.RunTime
{
    /// <summary>
    /// 表示游戏生命周期中的当前阶段;
    /// </summary>
    public enum GameStates
    {
        /// <summary>
        /// 等待模块初始化;
        /// </summary>
        WaitingForModule,

        /// <summary>
        /// 等待世界创建;
        /// </summary>
        WaitingCreateWorld,

        /// <summary>
        /// 世界正在运行;
        /// </summary>
        Running,
    }
}
