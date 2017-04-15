using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        AggregateException Exception { get; }
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
    public abstract class AsyncOperation : IAsyncOperation
    {
        public AsyncOperation()
        {
            IsCompleted = false;
            IsFaulted = false;
            Exception = null;
        }

        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public AggregateException Exception { get; private set; }

        protected void OnCompleted()
        {
            IsCompleted = true;
        }

        protected void OnFaulted(Exception ex)
        {
            Exception = ex as AggregateException ?? new AggregateException(ex);
            IsFaulted = true;
            IsCompleted = true;
        }

        public override string ToString()
        {
            return base.ToString() +
                "[IsCompleted:" + IsCompleted +
                ",IsFaulted:" + IsFaulted +
                ",Exception:" + Exception + "]";
        }
    }

    /// <summary>
    /// 表示异步的操作;
    /// </summary>
    public abstract class AsyncOperation<TResult> : IAsyncOperation<TResult>
    {
        public AsyncOperation()
        {
            IsCompleted = false;
            IsFaulted = false;
            Result = default(TResult);
            Exception = null;
        }

        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public TResult Result { get; private set; }
        public AggregateException Exception { get; private set; }

        protected void OnCompleted(TResult result)
        {
            Result = result;
            IsCompleted = true;
        }

        protected void OnFaulted(Exception ex)
        {
            Exception = ex as AggregateException ?? new AggregateException(ex);
            IsFaulted = true;
            IsCompleted = true;
        }

        public override string ToString()
        {
            return base.ToString() +
                "[IsCompleted:" + IsCompleted +
                ",IsFaulted:" + IsFaulted +
                ",Exception:" + Exception + "]";
        }
    }


    /// <summary>
    /// 表示在多线程内进行的操作;
    /// </summary>
    public abstract class ThreadOperation : IAsyncOperation
    {
        public ThreadOperation()
        {
            IsCompleted = false;
            IsFaulted = false;
            Exception = null;
        }

        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public AggregateException Exception { get; private set; }

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
                Operate();
            }
            catch (Exception ex)
            {
                Exception = ex as AggregateException ?? new AggregateException(ex);
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
        protected abstract void Operate();

        public override string ToString()
        {
            return base.ToString() +
                "[IsCompleted:" + IsCompleted +
                ",IsFaulted:" + IsFaulted +
                ",Exception:" + Exception + "]";
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
            Exception = null;
        }

        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public TResult Result { get; private set; }
        public AggregateException Exception { get; private set; }

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
                Exception = ex as AggregateException ?? new AggregateException(ex);
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

        public override string ToString()
        {
            return base.ToString() +
                "[IsCompleted:" + IsCompleted +
                ",IsFaulted:" + IsFaulted +
                ",Exception:" + Exception + "]";
        }
    }


    /// <summary>
    /// 继承 MonoBehaviour 的操作类;
    /// </summary>
    public abstract class OperationMonoBehaviour : MonoBehaviour, IAsyncOperation
    {
        protected OperationMonoBehaviour()
        {
        }

        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public AggregateException Exception { get; private set; }

        protected virtual void Awake()
        {
            IsCompleted = false;
            IsFaulted = false;
            Exception = null;
        }

        protected void OnCompleted()
        {
            IsCompleted = true;
        }

        protected void OnFaulted(Exception ex)
        {
            Exception = ex as AggregateException ?? new AggregateException(ex);
            IsFaulted = true;
            IsCompleted = true;
        }

        public override string ToString()
        {
            return base.ToString() +
                "[IsCompleted:" + IsCompleted +
                ",IsFaulted:" + IsFaulted +
                ",Exception:" + Exception + "]";
        }
    }

    /// <summary>
    /// 继承 MonoBehaviour 的操作类;
    /// </summary>
    public abstract class OperationMonoBehaviour<TResult> : MonoBehaviour, IAsyncOperation<TResult>
    {
        protected OperationMonoBehaviour()
        {
        }

        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public TResult Result { get; private set; }
        public AggregateException Exception { get; private set; }

        protected virtual void Awake()
        {
            IsCompleted = false;
            IsFaulted = false;
            Result = default(TResult);
            Exception = null;
        }

        protected void OnCompleted(TResult result)
        {
            Result = result;
            IsCompleted = true;
        }

        protected void OnFaulted(Exception ex)
        {
            Exception = ex as AggregateException ?? new AggregateException(ex);
            IsFaulted = true;
            IsCompleted = true;
        }

        public override string ToString()
        {
            return base.ToString() +
                "[IsCompleted:" + IsCompleted +
                ",IsFaulted:" + IsFaulted +
                ",Exception:" + Exception + "]";
        }
    }


    /// <summary>
    /// 表示在协程内操作;
    /// </summary>
    public abstract class CoroutineOperation<TResult> : IAsyncOperation<TResult>, IEnumerator<TResult>
    {
        public CoroutineOperation()
        {
            IsCompleted = false;
            IsFaulted = false;
            Result = default(TResult);
            Exception = null;
            this.coroutine = Operate();
        }

        IEnumerator<TResult> coroutine;
        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public TResult Result { get; private set; }
        public AggregateException Exception { get; private set; }

        public object Current
        {
            get { return coroutine.Current; }
        }

        TResult IEnumerator<TResult>.Current
        {
            get { return coroutine.Current; }
        }

        protected abstract IEnumerator Operate();
        public abstract void Dispose();
        void IEnumerator.Reset() { }

        protected void OnCompleted(TResult result)
        {
            Result = result;
            IsCompleted = true;
        }

        void OnFaulted(Exception ex)
        {
            Exception = ex as AggregateException ?? new AggregateException(ex);
            IsFaulted = true;
            IsCompleted = true;
        }

        bool IEnumerator.MoveNext()
        {
            try
            {
                return coroutine.MoveNext();
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
                return false;
            }
        }

        public override string ToString()
        {
            return base.ToString() +
                "[IsCompleted:" + IsCompleted +
                ",IsFaulted:" + IsFaulted +
                ",Exception:" + Exception + "]";
        }
    }

}
