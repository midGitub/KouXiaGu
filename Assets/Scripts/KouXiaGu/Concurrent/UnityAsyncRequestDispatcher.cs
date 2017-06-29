﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Concurrent
{
    /// <summary>
    /// 用于处理在Unity线程进行操作的异步请求;
    /// </summary>
    public sealed class UnityAsyncRequestDispatcher : UnitySington<UnityAsyncRequestDispatcher>
    {
        object asyncLock;
        [SerializeField]
        Stopwatch runtimeStopwatch = new Stopwatch(0.2f);
        AsyncRequestQueue requestQueue;

        void Awake()
        {
            asyncLock = new object();
            requestQueue = new AsyncRequestQueue(runtimeStopwatch);
            SetInstance(this);
        }

        void Update()
        {
            lock (asyncLock)
            {
                requestQueue.MoveNext();
            }
        }

        public void AddQueue(IAsyncRequest request)
        {
            lock (asyncLock)
            {
                requestQueue.Add(request);
            }
        }
    }
}