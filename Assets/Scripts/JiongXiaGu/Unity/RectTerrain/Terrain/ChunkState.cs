namespace JiongXiaGu.Unity.RectTerrain
{
    /// <summary>
    /// 块当前的状态;
    /// </summary>
    public enum ChunkState
    {
        /// <summary>
        /// 未创建;
        /// </summary>
        None,

        /// <summary>
        /// 正在创建中;
        /// </summary>
        Creating,

        /// <summary>
        /// 创建完成;
        /// </summary>
        Completed,

        /// <summary>
        /// 正在更新中;
        /// </summary>
        Updating,

        /// <summary>
        /// 正在销毁中;
        /// </summary>
        Destroying,
    }
}
