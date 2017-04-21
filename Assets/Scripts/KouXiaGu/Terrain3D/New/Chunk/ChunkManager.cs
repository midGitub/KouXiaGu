using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块管理;
    /// </summary>
    public class ChunkManager
    {
        static RectGrid chunkGrid
        {
            get { return ChunkInfo.ChunkGrid; }
        }

        public ChunkManager()
        {
            chunkPool = new ChunkPool();
            activatedChunks = new Dictionary<RectCoord, Chunk>();
        }

        ChunkPool chunkPool;
        Dictionary<RectCoord, Chunk> activatedChunks;

        public int ActivatedChunkCount
        {
            get { return activatedChunks.Count; }
        }

        public Chunk this[RectCoord coord]
        {
            get { return activatedChunks[coord]; }
        }

        /// <summary>
        /// 创建到,若已经存在则返回异常;
        /// </summary>
        public Chunk Create(RectCoord rectCoord, ChunkTexture textures)
        {
            if (activatedChunks.ContainsKey(rectCoord))
                throw new ArgumentException();

            Chunk chunk = chunkPool.Get();
            chunk.Position = chunkGrid.GetCenter(rectCoord);
            chunk.SetTextures(textures);
            activatedChunks.Add(rectCoord, chunk);
            return chunk;
        }

        /// <summary>
        /// 更新已有内容,若坐标地图块已经不存在,返回null;
        /// </summary>
        public Chunk Update(RectCoord rectCoord, ChunkTexture textures)
        {
            Chunk chunk;
            if (activatedChunks.TryGetValue(rectCoord, out chunk))
            {
                chunk.SetTextures(textures);
            }
            return chunk;
        }

        public bool Contains(RectCoord rectCoord)
        {
            return activatedChunks.ContainsKey(rectCoord);
        }

        public bool TryGetChunk(RectCoord rectCoord, out Chunk chunk)
        {
            return activatedChunks.TryGetValue(rectCoord, out chunk);
        }

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            RectCoord rectCoord = chunkGrid.GetCoord(position);
            Chunk chunk;
            if (TryGetChunk(rectCoord, out chunk))
            {
                Vector2 uv = chunkGrid.GetUV(rectCoord, position);
                return chunk.Texture.GetHeight(uv);
            }
            return 0;
        }

        /// <summary>
        /// 清空所有;
        /// </summary>
        public void DestroyAll()
        {
            chunkPool.DestroyAll();
            Destroy(activatedChunks);
        }

        void Destroy(Dictionary<RectCoord, Chunk> activatedChunks)
        {
            foreach (var chunk in activatedChunks.Values)
            {
                chunk.Destroy();
            }
            activatedChunks.Clear();
        }
    }

}
