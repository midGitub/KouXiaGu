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
        public RectGrid ChunkGrid
        {
            get { return ChunkInfo.ChunkGrid; }
        }

        public ChunkManager()
        {
            chunkPool = new ChunkPool();
            activatedChunks = new Dictionary<RectCoord, Chunk>();
            readOnlyActivatedChunks = activatedChunks.AsReadOnlyDictionary();
        }

        readonly ChunkPool chunkPool;
        readonly Dictionary<RectCoord, Chunk> activatedChunks;
        readonly IReadOnlyDictionary<RectCoord, Chunk> readOnlyActivatedChunks;

        public IReadOnlyDictionary<RectCoord, Chunk> ActivatedChunks
        {
            get { return readOnlyActivatedChunks; }
        }

        /// <summary>
        /// 更新或创建到;
        /// </summary>
        public Chunk UpdateOrCreate(RectCoord rectCoord, ChunkTexture textures)
        {
            Chunk chunk = Update(rectCoord, textures);

            if (chunk == null)
                chunk = Create(rectCoord, textures);

            return chunk;
        }

        /// <summary>
        /// 创建到,若已经存在则返回异常;
        /// </summary>
        public Chunk Create(RectCoord rectCoord, ChunkTexture textures)
        {
            if (activatedChunks.ContainsKey(rectCoord))
                throw new ArgumentException();

            Chunk chunk = chunkPool.Get();
            chunk.Position = ChunkGrid.GetCenter(rectCoord);
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

        public void Clear()
        {
            chunkPool.DestroyAll();
            Destroy(activatedChunks);
        }

        void Destroy(IDictionary<RectCoord, Chunk> activatedChunks)
        {
            foreach (var chunk in activatedChunks.Values)
            {
                chunk.Destroy();
            }
            activatedChunks.Clear();
        }

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            RectCoord rectCoord = ChunkGrid.GetCoord(position);
            Chunk chunk;
            if (activatedChunks.TryGetValue(rectCoord, out chunk))
            {
                Vector2 uv = ChunkGrid.GetUV(rectCoord, position);
                return chunk.Texture.GetHeight(uv);
            }
            return 0;
        }

    }

}
