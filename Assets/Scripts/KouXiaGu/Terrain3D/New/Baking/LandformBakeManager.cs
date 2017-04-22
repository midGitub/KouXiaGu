using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.Grids;
using UniRx;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘焙管理;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformBakeManager : MonoBehaviour
    {
        LandformBakeManager()
        {
        }

        [SerializeField]
        Stopwatch runtimeStopwatch;
        [SerializeField]
        LandformBaker baker;
        LinkedList<BakingRequest> requestQueue;
        IReadOnlyCollection<IBakingRequest> readOnlyRequestQueue;

        public Stopwatch RuntimeStopwatch
        {
            get { return runtimeStopwatch; }
            set { runtimeStopwatch = value; }
        }

        public IBakingRequest Current
        {
            get { return requestQueue.First.Value; }
        }

        void Awake()
        {
            requestQueue = new LinkedList<BakingRequest>();
            readOnlyRequestQueue = requestQueue.AsReadOnlyCollection(item => item as IBakingRequest);
        }

        void Update()
        {
            if (requestQueue.Count != 0)
            {
                runtimeStopwatch.Restart();
                while (!runtimeStopwatch.Await())
                {
                    var current = requestQueue.First.Value;
                    if (current.IsCompleted)
                    {
                        requestQueue.RemoveFirst();
                        break;
                    }
                    current.MoveNext();
                }
            }
        }

        /// <summary>
        /// 添加烘焙请求;
        /// </summary>
        public IBakingRequest AddRequest(RectCoord chunkCoord)
        {
            var request = new BakingRequest(chunkCoord, baker);
            requestQueue.AddLast(request);
            return request;
        }

        /// <summary>
        /// 取消所有请求;
        /// </summary>
        public void CanceleAll()
        {
            foreach (var request in requestQueue)
            {
                request.Dispose();
            }
        }

        class BakingRequest : AsyncOperation<ChunkTexture>, IBakingRequest
        {
            public BakingRequest(RectCoord chunkCoord, LandformBaker baker)
            {
                this.chunkCoord = chunkCoord;
                bakeCoroutine = baker.GetBakeCoroutine(chunkCoord);
                tracker = new ListTracker<ChunkTexture>(1);
            }

            readonly RectCoord chunkCoord;
            readonly IEnumerator bakeCoroutine;
            readonly ListTracker<ChunkTexture> tracker;

            public RectCoord ChunkCoord
            {
                get { return chunkCoord; }
            }

            protected override void OnCompleted(ChunkTexture result)
            {
                base.OnCompleted(result);
                tracker.Track(result);
            }

            protected override void OnCanceled()
            {
                base.OnCanceled();
                var error = new OperationCanceledException();
                tracker.TrackError(error);
            }

            protected override void OnFaulted(Exception ex)
            {
                base.OnFaulted(ex);
                tracker.TrackError(ex);
            }

            public IDisposable Subscribe(IObserver<ChunkTexture> observer)
            {
                return tracker.Subscribe(observer);
            }

            public bool MoveNext()
            {
                return bakeCoroutine.MoveNext();
            }

            /// <summary>
            /// 取消,结束烘培请求;
            /// </summary>
            public void Dispose()
            {
                OnCanceled();
            }

        }

    }

}
