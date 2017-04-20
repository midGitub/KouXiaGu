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
    public class TerrainChunkManager
    {
        static RectGrid chunkGrid
        {
            get { return TerrainChunkInfo.ChunkGrid; }
        }

        public TerrainChunkManager()
        {
            chunkPool = new TerrainChunkPool();
            activatedChunks = new Dictionary<RectCoord, TerrainChunk>();
        }

        TerrainChunkPool chunkPool;
        Dictionary<RectCoord, TerrainChunk> activatedChunks;

        public TerrainChunkPool ChunkPool
        {
            get { return chunkPool; }
        }

        public int ActivatedChunkCount
        {
            get { return activatedChunks.Count; }
        }

        /// <summary>
        /// 创建到,若已经存在则返回异常;
        /// </summary>
        public TerrainChunk Create(RectCoord rectCoord, TerrainChunkTexture textures)
        {
            if (activatedChunks.ContainsKey(rectCoord))
                throw new ArgumentException();

            Vector3 position = chunkGrid.GetCenter(rectCoord);
            TerrainChunk chunk = chunkPool.Get();
            Set(chunk, position, textures);
            activatedChunks.Add(rectCoord, chunk);
            return chunk;
        }

        void Set(TerrainChunk chunk, Vector3 position, TerrainChunkTexture textures)
        {
            chunk.transform.position = position;
            chunk.Texture.SetTextures(textures);
        }

        /// <summary>
        /// 更新已有内容,若坐标地图块已经不存在,返回null;
        /// </summary>
        public TerrainChunk Update(RectCoord rectCoord, TerrainChunkTexture textures)
        {
            TerrainChunk chunk;
            if (activatedChunks.TryGetValue(rectCoord, out chunk))
            {
                Vector3 position = chunkGrid.GetCenter(rectCoord);
                Set(chunk, position, textures);
            }
            return chunk;
        }

        /// <summary>
        /// 确认是否存在这个地形块;
        /// </summary>
        public bool Contains(RectCoord rectCoord)
        {
            return activatedChunks.ContainsKey(rectCoord);
        }

        /// <summary>
        /// 清空所有;
        /// </summary>
        public void Clear()
        {
            chunkPool.Clear();
            Clear(activatedChunks);
        }

        void Clear(Dictionary<RectCoord, TerrainChunk> activatedChunks)
        {
            foreach (var chunk in activatedChunks.Values)
            {
                chunk.Destroy();
            }
            activatedChunks.Clear();
        }
    }


    public class TerrainChunkPool : ObjectPool<TerrainChunk>
    {
        public override TerrainChunk Instantiate()
        {
            TerrainChunk chunk = TerrainChunk.Create();
            return chunk;
        }

        public override void Reset(TerrainChunk chunk)
        {
            chunk.Clear();
        }

        public override void Destroy(TerrainChunk chunk)
        {
            chunk.Destroy();
        }
    }

}
