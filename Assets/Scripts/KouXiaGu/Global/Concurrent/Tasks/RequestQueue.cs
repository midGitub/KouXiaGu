using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.Concurrent
{

    /// <summary>
    /// 操作请求队列;
    /// </summary>
    public class RequestQueue : IRequestDispatcher
    {
        public RequestQueue(ISegmented stopwatch)
        {
            Stopwatch = stopwatch;
            requestQueue = new Queue<IRequest>();
            coroutine = Coroutine();
        }

        Queue<IRequest> requestQueue;
        public ISegmented Stopwatch { get; set; }
        IEnumerator coroutine;
        readonly object asyncLock = new object();

        public int Count
        {
            get { return requestQueue.Count; }
        }

        public IRequest Add(IRequest request)
        {
            lock (asyncLock)
            {
                if (!request.IsCompleted)
                {
                    requestQueue.Enqueue(request);
                }
                return request;
            }
        }

        public void Clear()
        {
            lock (asyncLock)
            {
                requestQueue.Clear();
            }
        }

        /// <summary>
        /// 在对应线程内调用;
        /// </summary>
        public void MoveNext()
        {
            coroutine.MoveNext();
        }

        IEnumerator Coroutine()
        {
            while (true)
            {
                IRequest request;

                if (Stopwatch.Await())
                {
                    yield return null;
                    Stopwatch.Restart();
                }

                while (requestQueue.Count == 0)
                {
                    yield return null;
                }

                lock (asyncLock)
                {
                    request = requestQueue.Dequeue();
                }

                Do(request);
            }
        }

        void Do(IRequest request)
        {
            try
            {
                if (!request.IsCompleted)
                {
                    request.Operate();
                }
            }
            catch(Exception ex)
            {
                Debug.LogError("[Request]请求出现异常 : " + "<" + request.ToString() + ">" + ex);
            }
        }
    }
}
