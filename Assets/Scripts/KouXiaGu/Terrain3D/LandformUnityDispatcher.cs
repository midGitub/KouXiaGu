using KouXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 处理地形在Unity线程执行的操作;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class LandformUnityDispatcher : SceneSington<LandformUnityDispatcher>, IRequestDispatcher
    {
        LandformUnityDispatcher()
        {
        }

        [SerializeField]
        Stopwatch runtimeStopwatch = new Stopwatch(0.2f);
        AsyncRequestQueue requestQueue;

        public int RequestCount
        {
            get { return requestQueue == null ? 0 : requestQueue.RequestCount; }
        }

        void Awake()
        {
            SetInstance(this);
            requestQueue = new AsyncRequestQueue(runtimeStopwatch);
        }

        void Update()
        {
            requestQueue.MoveNext();
        }

        public void Add(IAsyncRequest request)
        {
            requestQueue.Add(request);
        }
    }
}
