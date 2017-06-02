using System;
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
    public class UnityAsyncRequestDispatcher : UnitySington<UnityAsyncRequestDispatcher>
    {
        protected UnityAsyncRequestDispatcher()
        {
        }

        [SerializeField]
        Stopwatch runtimeStopwatch = new Stopwatch(0.2f);
        AsyncRequestQueue requestQueue;

        protected virtual void Awake()
        {
            requestQueue = new AsyncRequestQueue(runtimeStopwatch);
        }

        protected virtual void Update()
        {
            requestQueue.MoveNext();
        }

        public void AddQueue(IAsyncRequest request)
        {
            requestQueue.Add(request);
        }
    }
}
