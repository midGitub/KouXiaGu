using System;
using System.Collections.Generic;
using KouXiaGu.World;
using KouXiaGu.Grids;
using UnityEngine;
using UniRx;

namespace KouXiaGu.Terrain3D
{

    public class SceneLandformCollection : Dictionary<RectCoord, LandformBuilder.ChunkCreateRequest>
    {
        public SceneLandformCollection()
        {
            ReadOnlySceneChunks = this.AsReadOnlyDictionary(item => item as IAsyncOperation<Chunk>);
        }

        public IReadOnlyDictionary<RectCoord, IAsyncOperation<Chunk>> ReadOnlySceneChunks { get; private set; }

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            RectCoord chunkCoord = ChunkInfo.ChunkGrid.GetCoord(position);
            IAsyncOperation<Chunk> chunk;
            if (ReadOnlySceneChunks.TryGetValue(chunkCoord, out chunk))
            {
                if (chunk.IsCompleted && !chunk.IsFaulted)
                {
                    Vector2 uv = ChunkInfo.ChunkGrid.GetUV(chunkCoord, position);
                    return chunk.Result.Renderer.GetHeight(uv);
                }
            }
            return 0;
        }
    }

    /// <summary>
    /// 场景地形块管理;
    /// </summary>
    public class LandformBuilder
    {
        public LandformBuilder(IWorld world)
        {
            if (world == null)
                throw new ArgumentNullException("world");

            World = world;
            SceneChunks = world.Components.Landform.LandformChunks;
            chunkPool = new ChunkPool();
            completedChunkSender = new Sender<RectCoord>();
        }

        public IWorld World { get; private set; }
        public SceneLandformCollection SceneChunks { get; private set; }
        readonly ChunkPool chunkPool;
        readonly Sender<RectCoord> completedChunkSender;

        LandformBaker Baker
        {
            get { return LandformBaker.Instance; }
        }

        public RectGrid ChunkGrid
        {
            get { return ChunkInfo.ChunkGrid; }
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
            if (!SceneChunks.TryGetValue(chunkCoord, out request))
            {
                request = new ChunkCreateRequest(this, World, chunkCoord, targets);
                AddBakeQueue(request);
                SceneChunks.Add(chunkCoord, request);
            }
            return request;
        }

        /// <summary>
        /// 仅更新对应地形块,若不存在对应地形块,则返回Null;
        /// </summary>
        public IAsyncOperation<Chunk> Update(RectCoord chunkCoord, BakeTargets targets = BakeTargets.All)
        {
            ChunkCreateRequest request;
            if (SceneChunks.TryGetValue(chunkCoord, out request))
            {
                if (request.IsBaking)
                {
                    ChunkCreateRequest newRequest = new ChunkCreateRequest(request, targets);
                    AddBakeQueue(request);
                    SceneChunks[chunkCoord] = newRequest;
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
            Baker.AddRequest(request);
        }

        /// <summary>
        /// 销毁指定块;
        /// </summary>
        public void Destroy(RectCoord chunkCoord)
        {
            ChunkCreateRequest request;
            if (SceneChunks.TryGetValue(chunkCoord, out request))
            {
                SceneChunks.Remove(chunkCoord);
                request.Destroy();
            }
        }

        /// <summary>
        /// 销毁所有块;
        /// </summary>
        public void DestroyAll()
        {
            foreach (var sceneChunk in SceneChunks.Values)
            {
                sceneChunk.Destroy();
            }
            SceneChunks.Clear();
        }

        public class ChunkCreateRequest : AsyncOperation<Chunk>, IAsyncOperation<Chunk>, IBakeRequest
        {
            /// <summary>
            /// 创建到一个新的请求,并且将旧请求设置为不可用;
            /// </summary>
            public ChunkCreateRequest(ChunkCreateRequest clone, BakeTargets targets)
            {
                clone.IsCanceled = true;
                Parent = clone.Parent;
                World = clone.World;
                ChunkCoord = clone.ChunkCoord;
                Chunk = clone.Chunk;
                Targets |= targets;
                IsInQueue = false;
                IsBaking = false;
                IsCanceled = false;
            }

            public ChunkCreateRequest(LandformBuilder parent, IWorld world, RectCoord chunkCoord, BakeTargets targets)
            {
                Parent = parent;
                World = world;
                ChunkCoord = chunkCoord;
                Chunk = chunkPool.Get();
                Chunk.Position = parent.ChunkGrid.GetCenter(chunkCoord);
                Targets = targets;
                IsInQueue = false;
                IsBaking = false;
                IsCanceled = false;
            }

            public LandformBuilder Parent { get; private set; }
            public IWorld World { get; private set; }
            public RectCoord ChunkCoord { get; private set; }
            public BakeTargets Targets { get; set; }
            public bool IsInQueue { get; private set; }
            public bool IsBaking { get; private set; }
            public bool IsCanceled { get; private set; }

            ChunkPool chunkPool
            {
                get { return Parent.chunkPool; }
            }

            public Chunk Chunk
            {
                get { return Result; }
                private set { Result = value; }
            }

            /// <summary>
            /// 销毁这个请求,并且销毁块资源;
            /// </summary>
            public void Destroy()
            {
                IsCanceled = true;
                chunkPool.Release(Chunk);
                Chunk = null;
            }

            /// <summary>
            /// 重置状态信息;
            /// </summary>
            internal void Reset()
            {
                IsInQueue = false;
                IsBaking = false;
                IsCanceled = false;
                base.ResetState();
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
