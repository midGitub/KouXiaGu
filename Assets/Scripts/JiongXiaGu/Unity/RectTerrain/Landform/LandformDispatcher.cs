using JiongXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 处理地形在Unity线程执行的操作;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class LandformDispatcher : MonoBehaviour, IRequestDispatcher
    {
        LandformDispatcher()
        {
        }

        [SerializeField]
        Stopwatch runtimeStopwatch = new Stopwatch(0.2f);
        RequestQueue requestQueue;

        public int Count
        {
            get { return requestQueue == null ? 0 : requestQueue.Count; }
        }

        void Awake()
        {
            requestQueue = new RequestQueue(runtimeStopwatch);
        }

        [SerializeField]
        int wait;
        int temp;

        void Update()
        {
            if (wait < temp++)
            {
                temp = 0;
                requestQueue.MoveNext();
            }
        }

        public IRequest Add(IRequest request)
        {
            return requestQueue.Add(request);
        }
    }
}
