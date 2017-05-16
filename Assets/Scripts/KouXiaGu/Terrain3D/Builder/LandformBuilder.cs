using System;
using System.Collections.Generic;
using KouXiaGu.World;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块创建管理;
    /// </summary>
    public class LandformBuilder
    {
        public LandformBuilder(IWorldData worldData)
        {
            baker = LandformBaker.Initialize(worldData);
            chunkPool = new ChunkPool();
            sceneChunks = new Dictionary<RectCoord, ChunkBakeRequest>();
            readOnlySceneChunks = sceneChunks.AsReadOnlyDictionary();
        }

        readonly LandformBaker baker;
        readonly ChunkPool chunkPool;
        readonly Dictionary<RectCoord, ChunkBakeRequest> sceneChunks;
        readonly IReadOnlyDictionary<RectCoord, ChunkBakeRequest> readOnlySceneChunks;

        public LandformBaker Baker
        {
            get { return baker; }
        }

        public RectGrid ChunkGrid
        {
            get { return ChunkInfo.ChunkGrid; }
        }

        internal IReadOnlyDictionary<RectCoord, ChunkBakeRequest> SceneDisplayedChunks
        {
            get { return readOnlySceneChunks; }
        }

        /// <summary>
        /// 仅创建对应地形块,若已经存在则返回存在的元素;
        /// </summary>
        public Chunk Create(RectCoord chunkCoord, BakeTargets targets = BakeTargets.All)
        {
            ChunkBakeRequest request;
            if (!sceneChunks.TryGetValue(chunkCoord, out request))
            {
                Chunk chunk = chunkPool.Get();
                chunk.Position = ChunkGrid.GetCenter(chunkCoord);
                request = new ChunkBakeRequest(chunkCoord, chunk, targets);
                AddBakeQueue(request);
                sceneChunks.Add(chunkCoord, request);
            }
            return request.Chunk;
        }

        /// <summary>
        /// 仅更新对应地形块,若不存在对应地形块,则返回Null;
        /// </summary>
        public Chunk Update(RectCoord chunkCoord, BakeTargets targets = BakeTargets.All)
        {
            ChunkBakeRequest request;
            if (sceneChunks.TryGetValue(chunkCoord, out request))
            {
                if (request.IsInBakeQueue & !request.IsBaking)
                {
                    request.ResetState();
                    request.Targets |= targets;
                }
                else
                {
                    request.ResetState();
                    request.Targets = targets;
                    AddBakeQueue(request);
                }
                return request.Chunk;
            }
            return null;
        }

        void AddBakeQueue(ChunkBakeRequest request)
        {
            baker.AddRequest(request);
        }

        public void Destroy(RectCoord chunkCoord)
        {
            ChunkBakeRequest request;
            if (sceneChunks.TryGetValue(chunkCoord, out request))
            {
                chunkPool.Release(request.Chunk);
                request.Cancel();
                sceneChunks.Remove(chunkCoord);
            }
        }
    }

}
