using KouXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent]
    public sealed class LandformUnityDispatcher : SceneSington<LandformUnityDispatcher>
    {
        LandformUnityDispatcher()
        {
        }

        [SerializeField]
        Stopwatch runtimeStopwatch = new Stopwatch(0.2f);
        AsyncRequestQueue requestQueue;

        void Awake()
        {
            SetInstance(this);
            requestQueue = new AsyncRequestQueue(runtimeStopwatch);
        }

        void Update()
        {
            requestQueue.MoveNext();
        }

        public void AddQueue(IAsyncRequest request)
        {
            requestQueue.Add(request);
        }
    }
}
