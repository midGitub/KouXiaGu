﻿using System;
using System.Collections.Generic;
using KouXiaGu.World;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{


    class ChunkRequest : IBakeRequest
    {
        public ChunkRequest(RectCoord chunkCoord, Chunk chunk, BakeTargets targets)
        {
            ChunkCoord = chunkCoord;
            Chunk = chunk;
            Targets = targets;
            IsBaking = false;
            IsInBakeQueue = false;
            IsBakeCompleted = false;
            IsCanceled = false;
        }

        public RectCoord ChunkCoord { get; private set; }
        public BakeTargets Targets { get; internal set; }
        public Chunk Chunk { get; private set; }
        public bool IsInBakeQueue { get; private set; }
        public bool IsBaking { get; private set; }
        public bool IsBakeCompleted { get; private set; }
        public bool IsCanceled { get; private set; }

        ChunkTexture IBakeRequest.Textures
        {
            get { return Chunk.Renderer; }
        }

        void IBakeRequest.AddBakeQueue()
        {
            if (IsInBakeQueue)
                UnityEngine.Debug.LogError("重复加入烘培队列?");

            IsInBakeQueue = true;
        }

        void IBakeRequest.StartBake()
        {
            if (IsBaking)
                UnityEngine.Debug.LogError("重复烘焙?");

            IsBaking = true;
        }

        void IBakeRequest.BakeCompleted()
        {
            try
            {
                Chunk.Renderer.Apply();
            }
            finally
            {
                IsBakeCompleted = true;
                IsBaking = false;
                IsInBakeQueue = false;
            }
        }

        /// <summary>
        /// 重置状态;
        /// </summary>
        internal void ResetState()
        {
            IsBakeCompleted = false;
            IsCanceled = false;
        }

        internal void Cancel()
        {
            IsCanceled = true;
        }
    }


    /// <summary>
    /// 地形块创建管理;
    /// </summary>
    public class LandformBuilder
    {
        public LandformBuilder(IWorldData worldData)
        {
            baker = LandformBaker.Initialize(worldData);
            chunkPool = new ChunkPool();
            sceneChunks = new Dictionary<RectCoord, ChunkRequest>();
            readOnlySceneChunks = sceneChunks.AsReadOnlyDictionary();
        }

        readonly LandformBaker baker;
        readonly ChunkPool chunkPool;
        readonly Dictionary<RectCoord, ChunkRequest> sceneChunks;
        readonly IReadOnlyDictionary<RectCoord, ChunkRequest> readOnlySceneChunks;

        public LandformBaker Baker
        {
            get { return baker; }
        }

        public RectGrid ChunkGrid
        {
            get { return ChunkInfo.ChunkGrid; }
        }

        internal IReadOnlyDictionary<RectCoord, ChunkRequest> SceneDisplayedChunks
        {
            get { return readOnlySceneChunks; }
        }

        /// <summary>
        /// 更新或创建地形块,若地形块已经在构建队列中,则变更到合适的烘培项目;
        /// 若已经烘焙完成,或者正在烘焙,则重新加入到构建队列;
        /// 若未找到对应的地形块,则创建到;
        /// </summary>
        public Chunk CreateOrUpdate(RectCoord chunkCoord, BakeTargets targets)
        {
            ChunkRequest request;
            if (sceneChunks.TryGetValue(chunkCoord, out request))
            {
                if (request.IsInBakeQueue && !request.IsBaking)
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
            }
            else
            {
                request = CreateChunk(chunkCoord);
                AddBakeQueue(request);
                sceneChunks.Add(chunkCoord, request);
            }
            return request.Chunk;
        }

        void AddBakeQueue(ChunkRequest request)
        {
            baker.AddRequest(request);
        }

        ChunkRequest CreateChunk(RectCoord chunkCoord, BakeTargets targets = BakeTargets.All)
        {
            Chunk chunk = chunkPool.Get();
            chunk.Position = ChunkGrid.GetCenter(chunkCoord);
            ChunkRequest buildRequest = new ChunkRequest(chunkCoord, chunk, targets);
            return buildRequest;
        }

        public void Destroy(RectCoord chunkCoord)
        {
            ChunkRequest request;
            if (sceneChunks.TryGetValue(chunkCoord, out request))
            {
                chunkPool.Release(request.Chunk);
                request.Cancel();
                sceneChunks.Remove(chunkCoord);
            }
        }
    }

}
