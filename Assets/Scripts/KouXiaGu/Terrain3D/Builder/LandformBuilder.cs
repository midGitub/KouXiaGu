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
                request = CreateChunk(chunkCoord);
                AddBakeQueue(request);
                sceneChunks.Add(chunkCoord, request);
            }
            return request.Chunk;
        }

        /// <summary>
        /// 更新或创建地形块,若地形块已经在构建队列中,则变更到合适的烘培项目;
        /// 若已经烘焙完成,或者正在烘焙,则重新加入到构建队列;
        /// 若未找到对应的地形块,则创建到;
        /// </summary>
        public Chunk CreateOrUpdate(RectCoord chunkCoord, BakeTargets targets)
        {
            ChunkBakeRequest request;
            if (sceneChunks.TryGetValue(chunkCoord, out request))
            {
                if (request.IsInBakeQueue)
                {
                    if (request.IsBaking)
                    {
                        request.ResetState();
                        request.Targets = targets;
                        AddBakeQueue(request);
                    }
                    else
                    {
                        request.ResetState();
                        request.Targets |= targets;
                    }
                }
                else
                {
                    request.ResetState();
                    request.Targets = targets;
                    AddBakeQueue(request);
                }
            }
            else
            {
                request = CreateChunk(chunkCoord);
                AddBakeQueue(request);
                sceneChunks.Add(chunkCoord, request);
            }
            return request.Chunk;
        }

        void AddBakeQueue(ChunkBakeRequest request)
        {
            baker.AddRequest(request);
        }

        ChunkBakeRequest CreateChunk(RectCoord chunkCoord, BakeTargets targets = BakeTargets.All)
        {
            Chunk chunk = chunkPool.Get();
            chunk.Position = ChunkGrid.GetCenter(chunkCoord);
            ChunkBakeRequest buildRequest = new ChunkBakeRequest(chunkCoord, chunk, targets);
            return buildRequest;
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
