using System;
using System.Collections.Generic;
using KouXiaGu.World;
using KouXiaGu.Grids;
using UnityEngine;
using UniRx;

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
            completedChunkSender = new Sender<RectCoord>();
        }

        readonly LandformBaker baker;
        readonly ChunkPool chunkPool;
        readonly Dictionary<RectCoord, ChunkCreateRequest> sceneChunks;
        readonly Sender<RectCoord> completedChunkSender;

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
        /// 当地形创建完成时传送消息;
        /// </summary>
        public IObservable<RectCoord> CompletedChunkSender
        {
            get { return completedChunkSender; }
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
                request = new ChunkCreateRequest(this, chunkCoord, chunk, targets);
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
                    ChunkCreateRequest newRequest = new ChunkCreateRequest(this, chunkCoord, request.Chunk, targets);
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

            public ChunkCreateRequest(LandformBuilder parent, RectCoord chunkCoord, Chunk chunk, BakeTargets targets) : this()
            {
                Parent = parent;
                ChunkCoord = chunkCoord;
                Chunk = chunk;
                Targets = targets;
            }

            public LandformBuilder Parent { get; private set; }
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
                        Parent.completedChunkSender.Send(ChunkCoord);
                    }
                }
                catch (Exception ex)
                {
                    Parent.completedChunkSender.SendError(ex);
                }
                finally
                {
                    IsInQueue = false;
                    IsBaking = false;
                }
            }
        }
    }
}
