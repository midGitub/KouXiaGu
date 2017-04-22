using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UniRx;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘焙;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformBaker : UnitySington<LandformBaker>
    {
        LandformBaker()
        {
        }

        [SerializeField]
        Stopwatch runtimeStopwatch;
        LinkedList<BakingRequest> requestQueue;

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
            SetInstance(this);
            requestQueue = new LinkedList<BakingRequest>();
        }

        void Update()
        {
            if (requestQueue.Count != 0)
            {
                runtimeStopwatch.Restart();
                while (!runtimeStopwatch.Await())
                {
                    var current = requestQueue.First.Value;
                    current.MoveNext();
                    if (current.IsCompleted)
                    {
                        requestQueue.RemoveFirst();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 添加烘焙请求;
        /// </summary>
        public IBakingRequest AddRequest(RectCoord chunkCoord)
        {
            var request = new BakingRequest(chunkCoord);
            requestQueue.AddLast(request);
            return request;
        }


        class BakingRequest : AsyncOperation<ChunkTexture>, IBakingRequest
        {
            public BakingRequest(RectCoord chunkCoord)
            {
                this.chunkCoord = chunkCoord;
                bakeCoroutine = BakeCoroutine();
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

            IEnumerator BakeCoroutine()
            {
                throw new NotImplementedException();
            }
        }

    }

}
