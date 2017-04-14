using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using KouXiaGu.Rx;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 异步操作;
    /// </summary>
    public interface IAsyncOperation
    {
        /// <summary>
        /// 是否完成?
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// 是否由于未经处理异常的原因而完成;
        /// </summary>
        bool IsFaulted { get; }

        /// <summary>
        /// 导致提前结束的异常;
        /// </summary>
        Exception Ex { get; }
    }

    /// <summary>
    /// 带返回值异步操作;
    /// </summary>
    public interface IAsyncOperation<TResult> : IAsyncOperation
    {
        /// <summary>
        /// 返回的结果;
        /// </summary>
        TResult Result { get; }
    }

    /// <summary>
    /// 表示异步的操作;
    /// </summary>
    public abstract class AsyncOperation<TResult> : IAsyncOperation<TResult>
    {
        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public TResult Result { get; private set; }
        public Exception Ex { get; private set; }

        protected void OnCompleted(TResult result)
        {
            Result = result;
            IsCompleted = true;
        }

        protected void OnError(Exception ex)
        {
            Ex = ex;
            IsFaulted = true;
            IsCompleted = true;
        }

        public override string ToString()
        {
            return base.ToString() +
                "[IsCompleted:" + IsCompleted +
                ",IsFaulted:" + IsFaulted +
                ",Exception:" + Ex + "]";
        }
    }

    /// <summary>
    /// 表示在多线程内进行的操作;
    /// </summary>
    public abstract class ThreadOperation<TResult> : IAsyncOperation<TResult>
    {
        public ThreadOperation()
        {
            IsCompleted = false;
            IsFaulted = false;
            Result = default(TResult);
            Ex = null;
        }

        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public TResult Result { get; private set; }
        public Exception Ex { get; private set; }

        /// <summary>
        /// 开始在多线程内操作,手动开始;
        /// </summary>
        public void Start()
        {
            ThreadPool.QueueUserWorkItem(OperateAsync);
        }

        void OperateAsync(object state)
        {
            try
            {
                Result = Operate();
            }
            catch (Exception ex)
            {
                Ex = ex;
                IsFaulted = true;
            }
            finally
            {
                IsCompleted = true;
            }
        }

        /// <summary>
        /// 需要在线程内进行的操作;
        /// </summary>
        protected abstract TResult Operate();

    }

    /// <summary>
    /// 表示在Unity线程内进行的异步操作;
    /// </summary>
    public abstract class CoroutineOperation<TResult> : MonoBehaviour, IAsyncOperation<TResult>
    {
        protected CoroutineOperation()
        {
        }

        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public TResult Result { get; private set; }
        public Exception Ex { get; private set; }

        protected virtual void Awake()
        {
            IsCompleted = false;
            IsFaulted = false;
            Result = default(TResult);
            Ex = null;
        }

        protected void OnCompleted(TResult result)
        {
            Result = result;
            IsCompleted = true;
        }

        protected void OnError(Exception ex)
        {
            Ex = ex;
            IsFaulted = true;
            IsCompleted = true;
        }

    }

}
