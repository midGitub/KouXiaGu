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
        /// <summary>
        /// 当加入队列调用;
        /// </summary>
        void OnAddQueue();

        /// <summary>
        /// 在调用 Operate() 之前调用一次,若返回false则直接退出队列,返回true则允许调用 Operate();
        /// </summary>
        bool Prepare();

        /// <summary>
        /// 返回true则继续调用,返回false则停止调用;
        /// </summary>
        bool Operate();

        /// <summary>
        /// 当退出队列调用;
        /// </summary>
        void OnQuitQueue();
    }

    /// <summary>
    /// 提供继承;
    /// </summary>
    public abstract class AsyncRequest : IAsyncRequest
    {
        /// <summary>
        /// 是否正在处置队列中?
        /// </summary>
        public bool InQueue { get; private set; }

        /// <summary>
        /// 是否正在进行任何操作中?
        /// </summary>
        public bool IsBusy { get; private set; }


        void IAsyncRequest.OnAddQueue()
        {
            InQueue = true;
            IsBusy = false;
            OnAddQueue();
        }

        bool IAsyncRequest.Prepare()
        {
            return Prepare();
        }

        bool IAsyncRequest.Operate()
        {
            IsBusy = true;
            return Operate();
        }

        void IAsyncRequest.OnQuitQueue()
        {
            InQueue = false;
            IsBusy = false;
            OnQuitQueue();
        }


        protected virtual void OnAddQueue()
        {
        }

        protected virtual bool Prepare()
        {
            return true;
        }

        protected virtual bool Operate()
        {
            return false;
        }

        protected virtual void OnQuitQueue()
        {
        }
    }
}
