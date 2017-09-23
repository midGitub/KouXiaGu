namespace JiongXiaGu.RectTerrain
{

    /// <summary>
    /// 地形更新操作;
    /// </summary>
    public interface IRectTerrainUpdateHandle
    {
        /// <summary>
        /// 内容是否全部更新完成;
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// 地形更新;
        /// </summary>
        void TerrainUpdate();
    }
}
