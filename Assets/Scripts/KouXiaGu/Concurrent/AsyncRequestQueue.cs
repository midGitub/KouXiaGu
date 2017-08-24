using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.Concurrent
{

    /// <summary>
    /// 异步请求合集;
    /// </summary>
    public class AsyncRequestQueue : IRequestDispatcher
    {
        public AsyncRequestQueue(ISegmented stopwatch)
        {
            Stopwatch = stopwatch;
            requestQueue = new Queue<IAsyncRequest>();
            coroutine = Coroutine();
        }

        Queue<IAsyncRequest> requestQueue;
        IEnumerator coroutine;
        public ISegmented Stopwatch { get; private set; }

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
        /// 添加请求到;
        /// </summary>
        public void Add(IAsyncRequest request)
        {
            request.OnAddQueue();
            requestQueue.Enqueue(request);
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
                while (requestQueue.Count == 0)
                {
                    yield return null;
                }

                var request = requestQueue.Dequeue();
                bool needContinue = request.Prepare();

                while (needContinue)
                {
                    if (Stopwatch.Await())
                    {
                        yield return null;
                        Stopwatch.Restart();
                    }
                    try
                    {
                        needContinue = request.Operate();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("[AsyncRequest]操作请求出现异常 : " + "<" + request.ToString() + ">" + ex);
                        needContinue = false;
                    }
                }
                QuieQueue(request);
            }
        }

        void QuieQueue(IAsyncRequest request)
        {
            try
            {
                request.OnQuitQueue();
            }
            catch (Exception ex)
            {
                Debug.LogError("[AsyncRequest]请求退出时出现异常 : " + "<" + request.ToString() + ">" + ex);
            }
        }
    }
}
