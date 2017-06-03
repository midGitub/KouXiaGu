//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using KouXiaGu.Grids;
//using KouXiaGu.World;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D
//{

//    public interface ICoroutineState : IState
//    {
//        /// <summary>
//        /// 重新开始计算;
//        /// </summary>
//        void Restart();

//        /// <summary>
//        /// 需要等待返回true,不需要等待返回false;
//        /// </summary>
//        bool Await();
//    }

//    public interface IBakeRequest : IRequest
//    {
//        IWorld World { get; }
//        RectCoord ChunkCoord { get; }
//        Chunk Chunk { get; }
//        BakeTargets Targets { get; }
//    }

//    /// <summary>
//    /// 地形烘培;
//    /// </summary>
//    [DisallowMultipleComponent]
//    public sealed class LandformBaker : SceneSington<LandformBaker>
//    {
//        /// <summary>
//        /// 透明的黑色颜色;
//        /// </summary>
//        public static readonly Color BlackTransparent = new Color(0, 0, 0, 0);

//        /// <summary>
//        /// 地平线颜色;
//        /// </summary>
//        public static readonly Color Horizon = new Color(0.5f, 0.5f, 0.5f, 1);

//        LandformBaker()
//        {
//        }

//        [SerializeField]
//        BakeCamera bakeCamera = null;
//        [SerializeField]
//        BakeLandform bakeLandform = null;
//        [SerializeField]
//        BakeRoad bakeRoad = null;
//        [SerializeField]
//        Stopwatch runtimeStopwatch = new Stopwatch(0.2f);
//        Coroutine bakeCoroutine;
//        Queue<IBakeRequest> requestQueue;

//        public int RequestCount
//        {
//            get { return requestQueue == null ? 0 : requestQueue.Count; }
//        }

//        public bool IsEmpty
//        {
//            get { return RequestCount == 0; }
//        }

//        void Awake()
//        {
//            SetInstance(this);
//            bakeLandform.Initialize();
//            bakeRoad.Initialise();
//            requestQueue = new Queue<IBakeRequest>();
//            bakeCoroutine = new Coroutine(BakeCoroutine());
//        }

//        [SerializeField]
//        int waitUpdate = 80;
//        int currentUpdateTime;

//        void Update()
//        {
//            if (currentUpdateTime < waitUpdate)
//            {
//                currentUpdateTime++;
//                return;
//            }
//            else
//            {
//                currentUpdateTime = 0;
//            }

//            runtimeStopwatch.Restart();
//            bakeCoroutine.MoveNext();
//        }

//        void Reset()
//        {
//            bakeCamera.qualitySettings.Updata();
//        }

//        IEnumerator BakeCoroutine()
//        {
//            while (true)
//            {
//                while (requestQueue.Count == 0)
//                {
//                    yield return null;
//                }

//                IBakeRequest bakeRequest = requestQueue.Dequeue();
//                if (bakeRequest.IsCanceled)
//                {
//                    goto _Complete_;
//                }

//                bakeRequest.Operate();
//                BakeTargets targets = bakeRequest.Targets;
//                CubicHexCoord chunkCenter = bakeRequest.ChunkCoord.GetChunkHexCenter();
//                ICoroutineState state = new CoroutineState(runtimeStopwatch, bakeRequest);

//                if ((targets & BakeTargets.Landform) > 0)
//                {
//                    yield return bakeLandform.BakeCoroutine(bakeCamera, bakeRequest.World, chunkCenter, bakeRequest.Chunk.Renderer, state);
//                }

//                if (state.IsCanceled)
//                {
//                    goto _Complete_;
//                }
//                if (runtimeStopwatch.Await())
//                {
//                    yield return null;
//                    runtimeStopwatch.Restart();
//                }

//                if ((targets & BakeTargets.Road) > 0)
//                {
//                    yield return bakeRoad.BakeCoroutine(bakeCamera, bakeRequest.World, chunkCenter, bakeRequest.Chunk.Renderer, state);
//                }

//            _Complete_:
//                bakeRequest.OutQueue();
//            }
//        }

//        public void AddRequest(IBakeRequest request)
//        {
//            request.AddQueue();
//            requestQueue.Enqueue(request);
//        }

//        [ContextMenu("Log")]
//        void Log()
//        {
//            Debug.Log(typeof(LandformBaker).Name + ";RequestCount:" + RequestCount);
//        }

//        class CoroutineState : ICoroutineState
//        {
//            public CoroutineState(ISegmented stopwatch, IRequest request)
//            {
//                this.stopwatch = stopwatch;
//                this.request = request;
//            }

//            readonly ISegmented stopwatch;
//            readonly IRequest request;

//            public bool IsCanceled
//            {
//                get { return request.IsCanceled; }
//            }

//            public bool Await()
//            {
//                return stopwatch.Await();
//            }

//            public void Restart()
//            {
//                stopwatch.Restart();
//            }
//        }
//    }

//}
