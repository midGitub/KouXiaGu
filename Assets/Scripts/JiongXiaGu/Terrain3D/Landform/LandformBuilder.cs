using System;
using System.Collections.Generic;
using JiongXiaGu.World;
using JiongXiaGu.Grids;
using UnityEngine;
using JiongXiaGu.Concurrent;
using System.Collections;
using System.Linq;

namespace JiongXiaGu.Terrain3D
{

    public class SceneLandformCollection
    {
        public SceneLandformCollection()
        {
            SceneChunks = new Dictionary<RectCoord, OLandformBuilder.CreateRequest>();
            ReadOnlySceneChunks = SceneChunks.AsReadOnlyDictionary(item => item as IAsyncOperation<LandformChunk>);
        }

        internal Dictionary<RectCoord, OLandformBuilder.CreateRequest> SceneChunks { get; private set; }
        public IReadOnlyDictionary<RectCoord, IAsyncOperation<LandformChunk>> ReadOnlySceneChunks { get; private set; }

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            RectCoord chunkCoord = LandformChunkInfo.ChunkGrid.GetCoord(position);
            OLandformBuilder.CreateRequest chunk;
            if (SceneChunks.TryGetValue(chunkCoord, out chunk))
            {
                if (chunk.Result != null)
                {
                    Vector2 uv = LandformChunkInfo.ChunkGrid.GetUV(chunkCoord, position);
                    return chunk.Result.Renderer.GetHeight(uv);
                }
            }
            return 0;
        }
    }

    /// <summary>
    /// 场景地形块管理;
    /// </summary>
    [Obsolete]
    class OLandformBuilder
    {
        public OLandformBuilder(IWorld world, IAsyncRequestDispatcher requestDispatcher)
        {
            if (world == null)
                throw new ArgumentNullException("world");

            World = world;
            this.requestDispatcher = requestDispatcher;
            //SceneChunks = world.Components.Landform.LandformChunks.SceneChunks;
            chunkPool = new LandformChunkPool();
            completedChunkSender = new Tracker<RectCoord>();
        }

        public IWorld World { get; private set; }
        public Dictionary<RectCoord, CreateRequest> SceneChunks { get; private set; }
        IAsyncRequestDispatcher requestDispatcher;
        readonly object unityThreadLock = new object();
        readonly LandformChunkPool chunkPool;
        readonly Tracker<RectCoord> completedChunkSender;

        public RectGrid ChunkGrid
        {
            get { return LandformChunkInfo.ChunkGrid; }
        }

        /// <summary>
        /// 当地形创建完成时传送消息;
        /// </summary>
        public IObservable<RectCoord> CompletedChunkSender
        {
            get { return completedChunkSender; }
        }

        static BakeCamera bakeCamera
        {
            get { return LandformSettings.Instance.bakeCamera; }
        }

        static BakeLandform bakeLandform
        {
            get { return LandformSettings.Instance.bakeLandform; }
        }

        static BakeRoad bakeRoad
        {
            get { return LandformSettings.Instance.bakeRoad; }
        }

        /// <summary>
        /// 异步创建对应地形块,若已经存在则返回存在的元素;
        /// </summary>
        public IAsyncOperation<LandformChunk> CreateAsync(RectCoord position, BakeTargets targets = BakeTargets.All)
        {
            CreateRequest request;
            if (!SceneChunks.TryGetValue(position, out request))
            {
                request = new CreateRequest(this, position, targets);
                requestDispatcher.Add(request);
                SceneChunks.Add(position, request);
            }
            return request;
        }

        /// <summary>
        /// 仅更新对应地形块,若不存在对应地形块,则返回Null;
        /// </summary>
        public IAsyncOperation<LandformChunk> UpdateAsync(RectCoord chunkCoord, BakeTargets targets = BakeTargets.All)
        {
            lock (unityThreadLock)
            {
                CreateRequest request;
                if (SceneChunks.TryGetValue(chunkCoord, out request))
                {
                    if (request.IsCompleted)
                    {
                        request.Reset();
                        request.Targets = targets;
                        requestDispatcher.Add(request);
                    }
                    else
                    {
                        request.Targets |= targets;
                    }
                    return request;
                }
                return null;
            }
        }

        /// <summary>
        /// 销毁指定块;
        /// </summary>
        public void DestroyAsync(RectCoord chunkCoord)
        {
            CreateRequest request;
            if (SceneChunks.TryGetValue(chunkCoord, out request))
            {
                SceneChunks.Remove(chunkCoord);
                Destory(request);
            }
        }

        void Destory(CreateRequest request)
        {
            lock (unityThreadLock)
            {
                if (request.IsCompleted)
                {
                    DestroyRequest destroyRequest = new DestroyRequest(this, request.ChunkCoord, request.Result);
                    requestDispatcher.Add(destroyRequest);
                }
                else
                {
                    request.Cancele();
                }
            }
        }

        /// <summary>
        /// 销毁所有块;
        /// </summary>
        internal void DestroyAll()
        {
            foreach (var sceneChunk in SceneChunks.Values)
            {
                Destory(sceneChunk);
            }
            SceneChunks.Clear();
        }


        internal class CreateRequest : AsyncOperation<LandformChunk>, IAsyncRequest, IState
        {
            public CreateRequest(OLandformBuilder parent, RectCoord chunkCoord, LandformChunk chunk)
            {
                Parent = parent;
                ChunkCoord = chunkCoord;
                OnCompleted(chunk);
            }

            public CreateRequest(OLandformBuilder parent, RectCoord chunkCoord, BakeTargets targets)
            {
                Parent = parent;
                ChunkCoord = chunkCoord;
                Targets = targets;
            }

            public OLandformBuilder Parent { get; private set; }
            public RectCoord ChunkCoord { get; private set; }
            public BakeTargets Targets { get; internal set; }
            public bool IsCanceled { get; private set; }

            LandformChunk chunk
            {
                get { return result; }
                set { result = value; }
            }

            LandformChunkPool chunkPool
            {
                get { return Parent.chunkPool; }
            }

            IWorld world
            {
                get { return Parent.World; }
            }

            public void Cancele()
            {
                IsCanceled = true;
            }

            public void Reset()
            {
                ResetState();
                IsCanceled = false;
            }

            bool IAsyncRequest.Prepare()
            {
                return true;
            }

            bool IAsyncRequest.Operate()
            {
                lock (Parent.unityThreadLock)
                {
                    if (IsCanceled || IsCompleted)
                    {
                        return false;
                    }
                    if (chunk == null)
                    {
                        chunk = chunkPool.Get();
                        chunk.Position = Parent.ChunkGrid.GetCenter(ChunkCoord);
                    }
                    CubicHexCoord chunkCenter = ChunkCoord.GetChunkHexCenter();

                    if ((Targets & BakeTargets.Landform) > 0)
                    {
                        bakeLandform.BakeCoroutine(bakeCamera, world.WorldData, chunkCenter, chunk.Renderer);
                    }
                    if ((Targets & BakeTargets.Road) > 0)
                    {
                        bakeRoad.BakeCoroutine(bakeCamera, world.WorldData, chunkCenter, chunk.Renderer);
                    }
                    chunk.Renderer.Apply();
                    OnCompleted();
                    Parent.completedChunkSender.Send(ChunkCoord);
                    return false;
                }
            }

            void IAsyncRequest.OnAddQueue()
            {
                if (IsCompleted)
                {
                    Debug.LogError("将完成的请求加入到处理队列!");
                }
            }

            void IAsyncRequest.OnQuitQueue()
            {
            }
        }

        public class DestroyRequest : IAsyncRequest
        {
            public DestroyRequest(OLandformBuilder parent, RectCoord chunkCoord, LandformChunk chunk)
            {
                this.parent = parent;
                this.chunkCoord = chunkCoord;
                this.chunk = chunk;
            }

            OLandformBuilder parent;
            LandformChunk chunk;
            RectCoord chunkCoord;

            LandformChunkPool chunkPool
            {
                get { return parent.chunkPool; }
            }

            bool IAsyncRequest.Operate()
            {
                chunkPool.Release(chunk);
                chunk = null;
                parent.completedChunkSender.Send(chunkCoord);
                return false;
            }

            void IAsyncRequest.OnAddQueue()
            {
            }

            bool IAsyncRequest.Prepare()
            {
                return true;
            }

            void IAsyncRequest.OnQuitQueue()
            {
            }
        }
    }
}
