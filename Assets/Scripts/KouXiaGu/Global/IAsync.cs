using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace KouXiaGu
{

    public interface IAsync<TResult> : IAsync
    {
        /// <summary>
        /// 返回的结果;
        /// </summary>
        TResult Result { get; }
    }

    public interface IAsync
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

    public abstract class AsyncOperation<TResult> : IAsync<TResult>, IEnumerator
    {
        public bool IsCompleted { get; protected set; }
        public bool IsFaulted { get; protected set; }
        public TResult Result { get; protected set; }
        public Exception Ex { get; protected set; }

        public object Current
        {
            get { return Result; }
        }

        public AsyncOperation()
        {
            IsCompleted = false;
            IsFaulted = false;
            Result = default(TResult);
            Ex = null;
        }

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

        protected abstract TResult Operate();

        public virtual bool MoveNext()
        {
            return !IsCompleted;
        }

        public void Reset()
        {
            return;
        }

    }

    public class MonoAsyncOperation<TResult> : MonoBehaviour, IAsync<TResult>
    {
        public bool IsCompleted { get; protected set; }
        public bool IsFaulted { get; protected set; }
        public TResult Result { get; protected set; }
        public Exception Ex { get; protected set; }

        protected virtual void Awake()
        {
            IsCompleted = false;
            IsFaulted = false;
            Result = default(TResult);
            Ex = null;
        }

    }

}
