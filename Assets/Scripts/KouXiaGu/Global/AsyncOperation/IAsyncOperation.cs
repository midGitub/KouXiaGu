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
        Exception Exception { get; }
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
        public Exception Exception { get; private set; }

        protected virtual void OnCompleted()
        {
            IsCompleted = true;
        }

        protected virtual void OnFaulted(Exception ex)
        {
            if (IsFaulted && Exception != null)
            {
                AggregateException aggregateEx = Exception as AggregateException;
                if (aggregateEx == null)
                {
                    Exception = new AggregateException(Exception, ex);
                }
                else
                {
                    List<Exception> exceptions = new List<Exception>(aggregateEx.InnerExceptions);
                    exceptions.Add(ex);
                    Exception = new AggregateException(exceptions);
                }
            }
            else
            {
                Exception = ex;
                IsFaulted = true;
                IsCompleted = true;
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
        public Exception Exception { get; private set; }

        protected virtual void OnCompleted(TResult result)
        {
            Result = result;
            IsCompleted = true;
        }

        protected virtual void OnFaulted(Exception ex)
        {
            if (IsFaulted && Exception != null)
            {
                AggregateException aggregateEx = Exception as AggregateException;
                if (aggregateEx == null)
                {
                    Exception = new AggregateException(Exception, ex);
                }
                else
                {
                    List<Exception> exceptions = new List<Exception>(aggregateEx.InnerExceptions);
                    exceptions.Add(ex);
                    Exception = new AggregateException(exceptions);
                }
            }
            else
            {
                Exception = ex;
                IsFaulted = true;
                IsCompleted = true;
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


    /// <summary>
    /// 表示同步操作,但是通过 IAsyncOperation 返回异常;
    /// </summary>
    public class Operation : AsyncOperation
    {
        public Operation(Action operation)
        {
            try
            {
                operation();
                OnCompleted();
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }
    }

    /// <summary>
    /// 表示同步操作,但是通过 IAsyncOperation 返回异常;
    /// </summary>
    public class Operation<TResult> : AsyncOperation<TResult>
    {
        public Operation(Func<TResult> operation)
        {
            try
            {
                TResult item = operation();
                OnCompleted(item);
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }
    }


    /// <summary>
    /// 表示在多线程内进行的操作;
    /// </summary>
    public abstract class ThreadOperation : AsyncOperation, IAsyncOperation
    {
        /// <summary>
        /// 需要在线程内进行的操作;
        /// </summary>
        protected abstract void Operate();

        /// <summary>
        /// 开始在多线程内操作,手动开始;
        /// </summary>
        public void Start()
        {
            ThreadPool.QueueUserWorkItem(OperateAsync);
        }

        protected virtual void OperateAsync(object state)
        {
            try
            {
                Operate();
                OnCompleted();
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }
    }

    /// <summary>
    /// 表示在多线程内进行的操作;
    /// </summary>
    public abstract class ThreadOperation<TResult> : AsyncOperation<TResult>, IAsyncOperation<TResult>
    {
        /// <summary>
        /// 需要在线程内进行的操作;
        /// </summary>
        protected abstract TResult Operate();

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
                var result = Operate();
                OnCompleted(result);
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }
    }

    /// <summary>
    /// 表示在多线程内进行的操作;
    /// </summary>
    public class ThreadDelegateOperation<TResult> : ThreadOperation<TResult>
    {
        public ThreadDelegateOperation(Func<TResult> reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            this.reader = reader;
            Start();
        }

        Func<TResult> reader;

        protected override TResult Operate()
        {
            return reader();
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
        public Exception Exception { get; private set; }

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
            if (IsFaulted && Exception != null)
            {
                AggregateException aggregateEx = Exception as AggregateException;
                if (aggregateEx == null)
                {
                    Exception = new AggregateException(Exception, ex);
                }
                else
                {
                    List<Exception> exceptions = new List<Exception>(aggregateEx.InnerExceptions);
                    exceptions.Add(ex);
                    Exception = new AggregateException(exceptions);
                }
            }
            else
            {
                Exception = ex;
                IsFaulted = true;
                IsCompleted = true;
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
        public Exception Exception { get; private set; }

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
            if (IsFaulted && Exception != null)
            {
                AggregateException aggregateEx = Exception as AggregateException;
                if (aggregateEx == null)
                {
                    Exception = new AggregateException(Exception, ex);
                }
                else
                {
                    List<Exception> exceptions = new List<Exception>(aggregateEx.InnerExceptions);
                    exceptions.Add(ex);
                    Exception = new AggregateException(exceptions);
                }
            }
            else
            {
                Exception = ex;
                IsFaulted = true;
                IsCompleted = true;
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


    /// <summary>
    /// 表示在协程内操作;
    /// </summary>
    public abstract class CoroutineOperation<TResult> : AsyncOperation<TResult>, IEnumerator
    {
        public CoroutineOperation(ISegmented segmented) : base()
        {
            this.coroutine = Operate();
            Segmented = segmented;
        }

        IEnumerator coroutine;
        public ISegmented Segmented { get; private set; }

        object IEnumerator.Current
        {
            get { return null; }
        }

        /// <summary>
        /// 仅支持返回 Null;
        /// </summary>
        protected abstract IEnumerator Operate();
        void IEnumerator.Reset() { }

        bool IEnumerator.MoveNext()
        {
            try
            {
                Segmented.Restart();
                while (!Segmented.KeepWait())
                {
                    if(!coroutine.MoveNext())
                    {
                        CompletedCheck();
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
                return false;
            }
        }

        void CompletedCheck()
        {
            if (!IsCompleted)
            {
                Debug.LogWarning("在协程完毕时未调用 OnCompleted(TResult result);");
            }
        }

    }

}
