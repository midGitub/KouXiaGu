using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.Concurrent
{

    public interface IRequestDispatcher
    {
        int RequestCount { get; }
        void Add(IAsyncRequest request);
    }

    /// <summary>
    /// 异步请求合集;
    /// </summary>
    public class AsyncRequestQueue : IRequestDispatcher
    {
        public AsyncRequestQueue(ISegmented stopwatch)
        {
            this.stopwatch = stopwatch;
            requestQueue = new Queue<IAsyncRequest>();
            coroutine = new Coroutine(Coroutine());
        }

        ISegmented stopwatch;
        Queue<IAsyncRequest> requestQueue;
        Coroutine coroutine;

        public IEnumerable<IAsyncRequest> Requests
        {
            get { return requestQueue; }
        }

        /// <summary>
        /// 请求数目;
        /// </summary>
        public int RequestCount
        {
            get { return requestQueue.Count; }
        }

        /// <summary>
        /// 需要在对应Unity线程内调用;
        /// </summary>
        public void MoveNext()
        {
            coroutine.MoveNext();
        }

        /// <summary>
        /// 添加请求到;
        /// </summary>
        public void Add(IAsyncRequest request)
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

                var request = requestQueue.Dequeue();

                try
                {
                    request.Operate();
                }
                catch (Exception ex)
                {
                    Debug.LogError("[UnityAsyncRequestDispatcher]请求出现异常 : " + ex);
                }

                if (stopwatch.Await())
                {
                    yield return null;
                    stopwatch.Restart();
                }
            }
        }
    }
}
