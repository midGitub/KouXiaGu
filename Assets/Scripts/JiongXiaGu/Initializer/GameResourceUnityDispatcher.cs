using JiongXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu
{

    /// <summary>
    /// 负责在Unity线程读取游戏资源;
    /// </summary>
    public sealed class GameResourceUnityDispatcher : UnitySington<GameResourceUnityDispatcher>
    {
        GameResourceUnityDispatcher()
        {
        }

        [SerializeField]
        Stopwatch runtimeStopwatch = new Stopwatch(1f);
        AsyncRequestQueue requestQueue;

        public int RequestCount
        {
            get { return requestQueue == null ? 0 : requestQueue.RequestCount; }
        }

        void Awake()
        {
            requestQueue = new AsyncRequestQueue(runtimeStopwatch);
            SetInstance(this);
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
