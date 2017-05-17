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
            sceneChunks = new Dictionary<RectCoord, ChunkCreateRequest>();
        }

        readonly LandformBaker baker;
        readonly ChunkPool chunkPool;
        readonly Dictionary<RectCoord, ChunkCreateRequest> sceneChunks;

        public LandformBaker Baker
        {
            get { return baker; }
        }

        public RectGrid ChunkGrid
        {
            get { return ChunkInfo.ChunkGrid; }
        }

        public IEnumerable<RectCoord> SceneCoords
        {
            get { return sceneChunks.Keys; }
        }

        /// <summary>
        /// 仅创建对应地形块,若已经存在则返回存在的元素;
        /// </summary>
        public IAsyncOperation<Chunk> Create(RectCoord chunkCoord, BakeTargets targets = BakeTargets.All)
        {
            ChunkCreateRequest request;
            if (!sceneChunks.TryGetValue(chunkCoord, out request))
            {
                Chunk chunk = chunkPool.Get();
                chunk.Position = ChunkGrid.GetCenter(chunkCoord);
                request = new ChunkCreateRequest(chunkCoord, chunk, targets);
                AddBakeQueue(request);
                sceneChunks.Add(chunkCoord, request);
            }
            return request;
        }

        /// <summary>
        /// 仅更新对应地形块,若不存在对应地形块,则返回Null;
        /// </summary>
        public IAsyncOperation<Chunk> Update(RectCoord chunkCoord, BakeTargets targets = BakeTargets.All)
        {
            ChunkCreateRequest request;
            if (sceneChunks.TryGetValue(chunkCoord, out request))
            {
                if (request.IsBaking)
                {
                    request.IsCanceled = true;
                    ChunkCreateRequest newRequest = new ChunkCreateRequest(chunkCoord, request.Chunk, targets);
                    AddBakeQueue(request);
                    sceneChunks[chunkCoord] = newRequest;
                }
                else if (request.IsInQueue)
                {
                    request.Targets |= targets;
                }
                else
                {
                    request.Reset();
                    AddBakeQueue(request);
                }
                return request;
            }
            return null;
        }

        void AddBakeQueue(ChunkCreateRequest request)
        {
            baker.AddRequest(request);
        }

        public void Destroy(RectCoord chunkCoord)
        {
            ChunkCreateRequest request;
            if (sceneChunks.TryGetValue(chunkCoord, out request))
            {
                sceneChunks.Remove(chunkCoord);
                chunkPool.Release(request.Chunk);
                request.Chunk = null;
                request.IsCanceled = true;
            }
        }

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            RectCoord chunkCoord = ChunkInfo.ChunkGrid.GetCoord(position);
            ChunkCreateRequest chunk;
            if (sceneChunks.TryGetValue(chunkCoord, out chunk))
            {
                Vector2 uv = ChunkInfo.ChunkGrid.GetUV(chunkCoord, position);
                return chunk.Chunk.Renderer.GetHeight(uv);
            }
            return 0;
        }


        class ChunkCreateRequest : AsyncOperation<Chunk>, IBakeRequest
        {
            ChunkCreateRequest()
            {
                IsInQueue = false;
                IsBaking = false;
                IsCanceled = false;
            }

            public ChunkCreateRequest(RectCoord chunkCoord, Chunk chunk, BakeTargets targets) : this()
            {
                ChunkCoord = chunkCoord;
                Chunk = chunk;
                Targets = targets;
            }

            public RectCoord ChunkCoord { get; set; }
            public BakeTargets Targets { get; set; }
            public bool IsInQueue { get; private set; }
            public bool IsBaking { get; private set; }
            public bool IsCanceled { get; set; }

            public Chunk Chunk
            {
                get { return Result; }
                set { Result = value; }
            }

            /// <summary>
            /// 重置状态信息;
            /// </summary>
            public void Reset()
            {
                if (IsInQueue)
                {
                    throw new ArgumentException();
                }

                IsInQueue = false;
                IsBaking = false;
                IsCanceled = false;
            }

            ChunkTexture IBakeRequest.Textures
            {
                get { return Chunk.Renderer; }
            }

            void IRequest.Operate()
            {
                IsBaking = true;
            }

            void IRequest.AddQueue()
            {
                IsInQueue = true;
            }

            void IRequest.OutQueue()
            {
                try
                {
                    if (!IsCanceled)
                    {
                        Result.Renderer.Apply();
                        OnCompleted(Chunk);
                    }
                }
                finally
                {
                    IsInQueue = false;
                    IsBaking = false;
                }
            }
        }
    }


    ///// <summary>
    ///// 地形块烘培请求;
    ///// </summary>
    //class ChunkCreateRequest : IBakeRequest
    //{
    //    public ChunkCreateRequest(RectCoord chunkCoord, Chunk chunk, BakeTargets targets)
    //    {
    //        ChunkCoord = chunkCoord;
    //        Chunk = chunk;
    //        Targets = targets;
    //        inBakeQueueTime = 0;
    //        IsBaking = false;
    //        IsCanceled = false;
    //    }

    //    public RectCoord ChunkCoord { get; private set; }
    //    public BakeTargets Targets { get; internal set; }
    //    public Chunk Chunk { get; private set; }
    //    int inBakeQueueTime;
    //    public bool IsBaking { get; private set; }
    //    public bool IsCanceled { get; private set; }

    //    public bool IsInBakeQueue
    //    {
    //        get { return inBakeQueueTime > 0; }
    //    }

    //    ChunkTexture IBakeRequest.Textures
    //    {
    //        get { return Chunk.Renderer; }
    //    }

    //    void IRequest.AddQueue()
    //    {
    //        inBakeQueueTime++;
    //    }

    //    void IRequest.Operate()
    //    {
    //        if (IsBaking)
    //            UnityEngine.Debug.LogError("重复烘焙?");

    //        IsBaking = true;
    //    }

    //    void IRequest.OutQueue()
    //    {
    //        try
    //        {
    //            Chunk.Renderer.Apply();
    //        }
    //        finally
    //        {
    //            IsBaking = false;
    //            inBakeQueueTime--;
    //        }
    //    }

    //    /// <summary>
    //    /// 重置状态;
    //    /// </summary>
    //    internal void ResetState()
    //    {
    //        IsCanceled = false;
    //    }

    //    /// <summary>
    //    /// 停用这个块请求;
    //    /// </summary>
    //    internal void Destroy()
    //    {
    //        IsCanceled = true;
    //        Chunk = null;
    //    }
    //}

}
