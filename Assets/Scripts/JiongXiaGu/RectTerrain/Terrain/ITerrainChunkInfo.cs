namespace JiongXiaGu.RectTerrain
{
    /// <summary>
    /// 块信息;
    /// </summary>
    public interface ITerrainChunkInfo<TPoint, TChunk>
    {
        TPoint Point { get; }
        TChunk Chunk { get; }
        ChunkState State { get; }
    }
}
