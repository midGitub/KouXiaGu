using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Concurrent
{

    /// <summary>
    /// 表示异步请求;
    /// </summary>
    public interface IAsyncRequest
    {
        void AddQueue();
        void Operate();
    }

    public class AsyncRequest : IAsyncRequest
    {
        public AsyncRequest(Action action)
        {
            Action = action;
        }

        public bool IsInQueue { get; private set; }
        public Action Action { get; private set; }

        public void AddQueue()
        {
            IsInQueue = true;
        }

        public void Operate()
        {
            try
            {
                Action();
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
            }
            finally
            {
                IsInQueue = false;
            }
        }
    }

}
