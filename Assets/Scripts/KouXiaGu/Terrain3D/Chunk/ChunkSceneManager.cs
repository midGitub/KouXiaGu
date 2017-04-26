//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using KouXiaGu.Grids;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 场景地形块管理;
//    /// </summary>
//    public class ChunkSceneManager
//    {
//        public RectGrid ChunkGrid
//        {
//            get { return ChunkInfo.ChunkGrid; }
//        }

//        public ChunkSceneManager()
//        {
//            chunkPool = new ChunkPool();
//            inSceneChunks = new Dictionary<RectCoord, Chunk>();
//            readOnlyInSceneChunks = inSceneChunks.AsReadOnlyDictionary();
//        }

//        readonly ChunkPool chunkPool;
//        readonly Dictionary<RectCoord, Chunk> inSceneChunks;
//        readonly IReadOnlyDictionary<RectCoord, Chunk> readOnlyInSceneChunks;

//        public IReadOnlyDictionary<RectCoord, Chunk> InSceneChunks
//        {
//            get { return readOnlyInSceneChunks; }
//        }

//        /// <summary>
//        /// 更新或创建到;
//        /// </summary>
//        public Chunk UpdateOrCreate(RectCoord rectCoord, ChunkTexture textures)
//        {
//            Chunk chunk = Update(rectCoord, textures);

//            if (chunk == null)
//                chunk = Create(rectCoord, textures);

//            return chunk;
//        }

//        /// <summary>
//        /// 创建到,若已经存在则返回异常;
//        /// </summary>
//        public Chunk Create(RectCoord chunkCoord, ChunkTexture textures)
//        {
//            if (inSceneChunks.ContainsKey(chunkCoord))
//                throw new ArgumentException();

//            Chunk chunk = chunkPool.Get();
//            chunk.Position = ChunkGrid.GetCenter(chunkCoord);
//            chunk.InitializeOrUpdate(textures);
//            inSceneChunks.Add(chunkCoord, chunk);
//            return chunk;
//        }

//        /// <summary>
//        /// 更新已有内容,若坐标地图块已经不存在,返回null;
//        /// </summary>
//        public Chunk Update(RectCoord rectCoord, ChunkTexture textures)
//        {
//            Chunk chunk;
//            if (inSceneChunks.TryGetValue(rectCoord, out chunk))
//            {
//                chunk.InitializeOrUpdate(textures);
//            }
//            return chunk;
//        }

//        public void Clear()
//        {
//            chunkPool.DestroyAll();
//            Destroy(inSceneChunks);
//        }

//        void Destroy(IDictionary<RectCoord, Chunk> activatedChunks)
//        {
//            foreach (var chunk in activatedChunks.Values)
//            {
//                chunk.Destroy();
//            }
//            activatedChunks.Clear();
//        }

//    }

//}
