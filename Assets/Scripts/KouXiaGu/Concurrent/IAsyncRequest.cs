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
        /// 返回true则继续调用,返回false则停止调用;
        /// </summary>
        bool Operate();

        /// <summary>
        /// 当退出队列调用;
        /// </summary>
        void OnQuitQueue();
    }

    /// <summary>
    /// 可中断的异步操作;
    /// </summary>
    public interface IAsyncRequestRevocable : IAsyncRequest
    {
        /// <summary>
        /// 当前是否允许取消?
        /// </summary>
        bool IsRevocable { get; }

        /// <summary>
        /// 是否已经被取消?
        /// </summary>
        bool IsCancelled { get; }

        /// <summary>
        /// 需要取消进行的操作;
        /// </summary>
        bool Cancel();
    }
}
