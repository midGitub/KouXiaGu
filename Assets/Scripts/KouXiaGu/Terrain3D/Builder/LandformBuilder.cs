using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using KouXiaGu.Grids;
using UnityEngine;

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
            sceneChunks = new Dictionary<RectCoord, IAsyncOperation<Chunk>>();
            readOnlySceneChunks = sceneChunks.AsReadOnlyDictionary();
        }

        readonly LandformBaker baker;
        readonly ChunkPool chunkPool;
        readonly Dictionary<RectCoord, IAsyncOperation<Chunk>> sceneChunks;
        readonly IReadOnlyDictionary<RectCoord, IAsyncOperation<Chunk>> readOnlySceneChunks;

        public RectGrid ChunkGrid
        {
            get { return ChunkInfo.ChunkGrid; }
        }

        public IReadOnlyDictionary<RectCoord, IAsyncOperation<Chunk>> SceneChunks
        {
            get { return readOnlySceneChunks; }
        }

        /// <summary>
        /// 尝试构建地图块,若已经存在地图块,则返回 false 和 构建地图块的操作结果;
        /// 若不存在地图块,则返回 true 和 构建地图块的操作结果;
        /// </summary>
        public bool TryCreate(RectCoord chunkCoord, out IAsyncOperation<Chunk> request)
        {
            if (sceneChunks.TryGetValue(chunkCoord, out request))
                return false;

            BuildRequest buildRequest = CreateChunk(chunkCoord);
            AddRequest(buildRequest);
            request = buildRequest;
            return true;
        }

        /// <summary>
        /// 更新这个地图块,地图块已经在构建队列中,则直接返回其;
        /// 若地图块不在队列中,或者已经构建完成,则重新加入到构建队列;
        /// </summary>
        public IAsyncOperation<Chunk> Update(RectCoord chunkCoord)
        {
            IAsyncOperation<Chunk> request;
            BuildRequest buildRequest;
            if (sceneChunks.TryGetValue(chunkCoord, out request))
            {
                buildRequest = request as BuildRequest;
                buildRequest.Reset();
                AddRequest(buildRequest);
            }
            else
            {
                buildRequest = CreateChunk(chunkCoord);
                AddRequest(buildRequest);
            }
            return buildRequest;
        }

        BuildRequest CreateChunk(RectCoord chunkCoord)
        {
            Chunk chunk = chunkPool.Get();
            chunk.Position = ChunkGrid.GetCenter(chunkCoord);
            BuildRequest buildRequest = new BuildRequest(chunkCoord, chunk);
            return buildRequest;
        }

        void AddRequest(BuildRequest request)
        {
            baker.AddRequest(request);
        }

        public void Destroy(RectCoord chunkCoord)
        {
            IAsyncOperation<Chunk> request;
            if (sceneChunks.TryGetValue(chunkCoord, out request))
            {
                BuildRequest buildRequest = request as BuildRequest;
                ReleaseChunk(buildRequest.Chunk);
                buildRequest.Cancel();
                sceneChunks.Remove(chunkCoord);
            }
        }

        void ReleaseChunk(Chunk chunk)
        {
            chunkPool.Release(chunk);
        }

        class BuildRequest : AsyncOperation<Chunk>, IBakeRequest
        {
            public BuildRequest(RectCoord chunkCoord, Chunk chunk)
            {
                ChunkCoord = chunkCoord;
                Result = chunk;
            }

            public RectCoord ChunkCoord { get; private set; }

            public Chunk Chunk
            {
                get { return Result; }
            }

            public void Reset()
            {
                ResetState();
            }

            public void Cancel()
            {
                OnCanceled();
            }

            ChunkTexture IBakeRequest.Textures
            {
                get { return Chunk.Renderer; }
            }

            void IBakeRequest.OnCompleted()
            {
                OnCompleted(Chunk);
            }

            void IBakeRequest.OnFaulted(Exception ex)
            {
                OnFaulted(ex);
            }
        }

    }

}
