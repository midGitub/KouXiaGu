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
        IEnumerator Operate();
        void OutQueue();
    }


    public abstract class AsyncRequest : IAsyncRequest
    {
        public bool IsInQueue { get; private set; }

        public abstract IEnumerator Operate();

        public virtual void AddQueue()
        {
            if (IsInQueue)
                throw new ArgumentException("重复加入!");

            IsInQueue = true;
        }

        public virtual void OutQueue()
        {
            if (IsInQueue)
                throw new ArgumentException("时序错误;");

            IsInQueue = false;
        }
    }
}
