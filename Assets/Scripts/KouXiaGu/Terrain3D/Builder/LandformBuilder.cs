using System;
using System.Collections.Generic;
using KouXiaGu.World;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 场景地形块管理;
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
                sceneChunks.Remove(chunkCoord);
                request.Cancel();
                chunkPool.Release(request.Chunk);
            }
        }

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            RectCoord chunkCoord = ChunkInfo.ChunkGrid.GetCoord(position);
            ChunkBakeRequest chunk;
            if (sceneChunks.TryGetValue(chunkCoord, out chunk))
            {
                Vector2 uv = ChunkInfo.ChunkGrid.GetUV(chunkCoord, position);
                return chunk.Chunk.Renderer.GetHeight(uv);
            }
            return 0;
        }
    }


    /// <summary>
    /// 地形块烘培请求;
    /// </summary>
    class ChunkBakeRequest : IBakeRequest
    {
        public ChunkBakeRequest(RectCoord chunkCoord, Chunk chunk, BakeTargets targets)
        {
            ChunkCoord = chunkCoord;
            Chunk = chunk;
            Targets = targets;
            inBakeQueueTime = 0;
            IsBaking = false;
            IsCanceled = false;
        }

        public RectCoord ChunkCoord { get; private set; }
        public BakeTargets Targets { get; internal set; }
        public Chunk Chunk { get; private set; }
        int inBakeQueueTime;
        public bool IsBaking { get; private set; }
        public bool IsCanceled { get; private set; }

        public bool IsInBakeQueue
        {
            get { return inBakeQueueTime > 0; }
        }

        ChunkTexture IBakeRequest.Textures
        {
            get { return Chunk.Renderer; }
        }

        void IBakeRequest.AddBakeQueue()
        {
            inBakeQueueTime++;
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
                IsBaking = false;
                inBakeQueueTime--;
            }
        }

        /// <summary>
        /// 重置状态;
        /// </summary>
        internal void ResetState()
        {
            IsCanceled = false;
        }

        internal void Cancel()
        {
            IsCanceled = true;
        }
    }

}
