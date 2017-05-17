using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    public interface IRequest
    {
        bool IsCanceled { get; }
        void Operate();
        void AddQueue();
        void OutQueue();
    }

    /// <summary>
    /// 异步请求处理;
    /// </summary>
    public sealed class RequestDispatcher : MonoBehaviour
    {
        RequestDispatcher()
        {
        }

        [SerializeField]
        Stopwatch runtimeStopwatch;
        Queue<IRequest> requestQueue;
        Coroutine createrCoroutine;

        public IEnumerable<IRequest> Requests
        {
            get { return requestQueue; }
        }

        public int RequestCount
        {
            get { return requestQueue.Count; }
        }

        void Awake()
        {
            requestQueue = new Queue<IRequest>();
            createrCoroutine = new Coroutine(Coroutine());
        }

        void Update()
        {
            createrCoroutine.MoveNext();
        }

        public void AddQueue(IRequest request)
        {
            request.AddQueue();
            requestQueue.Enqueue(request);
        }

        IEnumerator Coroutine()
        {
            while (true)
            {
                while (requestQueue.Count == 0)
                {
                    yield return null;
                }

                try
                {
                    IRequest request = requestQueue.Dequeue();

                    if (!request.IsCanceled)
                    {
                        request.Operate();
                    }

                    request.OutQueue();
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }

                if (runtimeStopwatch.Await())
                {
                    yield return null;
                    runtimeStopwatch.Restart();
                }
            }
        }
    }
}
