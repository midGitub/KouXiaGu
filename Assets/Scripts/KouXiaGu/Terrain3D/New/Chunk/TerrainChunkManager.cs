﻿using System;
using System.Collections.Generic;
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

        public TerrainChunk this[RectCoord coord]
        {
            get { return activatedChunks[coord]; }
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

        public bool Contains(RectCoord rectCoord)
        {
            return activatedChunks.ContainsKey(rectCoord);
        }

        public bool TryGetChunk(RectCoord rectCoord, out TerrainChunk chunk)
        {
            return activatedChunks.TryGetValue(rectCoord, out chunk);
        }

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            RectCoord rectCoord = chunkGrid.GetCoord(position);
            TerrainChunk chunk;
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

        void Destroy(Dictionary<RectCoord, TerrainChunk> activatedChunks)
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
