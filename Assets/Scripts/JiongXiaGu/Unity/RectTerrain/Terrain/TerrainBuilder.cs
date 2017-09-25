using System.Collections.Generic;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 块创建器;
    /// </summary>
    public abstract class TerrainBuilder<TPoint, TChunk>
    {
        public TerrainBuilder()
        {
            chunks = new Dictionary<TPoint, TerrainChunkInfo<TPoint, TChunk>>();
        }

        readonly object asyncLock = new object();
        readonly Dictionary<TPoint, TerrainChunkInfo<TPoint, TChunk>> chunks;

        /// <summary>
        /// 所有在创建或者已经创建的坐标;
        /// </summary>
        public IEnumerable<TPoint> Points
        {
            get { return chunks.Keys; }
        }

        /// <summary>
        /// 所有在创建或者已经创建的块信息;
        /// </summary>
        public IEnumerable<TerrainChunkInfo<TPoint, TChunk>> Chunks
        {
            get { return chunks.Values; }
        }

        /// <summary>
        /// 获取到块信息,若不存在则创建到;
        /// </summary>
        protected TerrainChunkInfo<TPoint, TChunk> GetOrCreate(TPoint chunkPos)
        {
            TerrainChunkInfo<TPoint, TChunk> info;
            if (!chunks.TryGetValue(chunkPos, out info))
            {
                info = Create(chunkPos);
                chunks.Add(chunkPos, info);
            }
            return info;
        }

        /// <summary>
        /// 创建空的块信息;
        /// </summary>
        protected abstract TerrainChunkInfo<TPoint, TChunk> Create(TPoint chunkPos);

        /// <summary>
        /// 创建块,若已经存在则返回实例,不存在则创建到;
        /// </summary>
        public TerrainChunkInfo<TPoint, TChunk> CreateAsync(TPoint chunkPos)
        {
            lock (asyncLock)
            {
                var info = GetOrCreate(chunkPos);
                info.CreateAsync();
                return info;
            }
        }

        /// <summary>
        /// 更新块,若不存在则返回null;
        /// </summary>
        public TerrainChunkInfo<TPoint, TChunk> UpdateAsync(TPoint chunkPos)
        {
            lock (asyncLock)
            {
                TerrainChunkInfo<TPoint, TChunk> info;
                if (chunks.TryGetValue(chunkPos, out info))
                {
                    info.UpdateAsync();
                    return info;
                }
                return null;
            }
        }

        /// <summary>
        /// 销毁块,若不存在则返回NULL;
        /// </summary>
        public TerrainChunkInfo<TPoint, TChunk> DestroyAsync(TPoint chunkPos)
        {
            lock (asyncLock)
            {
                TerrainChunkInfo<TPoint, TChunk> info;
                if (chunks.TryGetValue(chunkPos, out info))
                {
                    if (info.State == ChunkState.None)
                    {
                        chunks.Remove(chunkPos);
                        return null;
                    }
                    else
                    {
                        info.DestroyAsync();
                        return info;
                    }
                }
                return null;
            }
        }
    }
}
