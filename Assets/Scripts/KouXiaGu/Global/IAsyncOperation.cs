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

    [Obsolete]
    public interface IAsync<TResult> : IAsync
    {
        /// <summary>
        /// 返回的结果;
        /// </summary>
        TResult Result { get; }
    }

    [Obsolete]
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


    /// <summary>
    /// 异步操作;
    /// </summary>
    public interface IAsyncOperation : IEnumerator
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

        object IEnumerator.Current
        {
            get { return Result; }
        }

        bool IEnumerator.MoveNext()
        {
            return !IsCompleted;
        }

        void IEnumerator.Reset()
        {
            return;
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

        object IEnumerator.Current
        {
            get { return Result; }
        }

        bool IEnumerator.MoveNext()
        {
            return !IsCompleted;
        }

        void IEnumerator.Reset()
        {
            return;
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

        object IEnumerator.Current
        {
            get { return Result; }
        }

        bool IEnumerator.MoveNext()
        {
            return !IsCompleted;
        }

        void IEnumerator.Reset()
        {
            return;
        }

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

    /// <summary>
    /// 异步操作拓展,都需要在 unity 线程内调用;
    /// </summary>
    public static class AsyncOperationExtensions
    {

        /// <summary>
        /// 当操作完成时,在unity线程内回调;
        /// </summary>
        public static IDisposable Subscribe<T>(this T operation, Action<T> onCompleted, Action<T> onError)
            where T : IAsyncOperation
        {
            if (onCompleted == null || onError == null)
                throw new ArgumentNullException();

            IDisposable disposer = new Operation(delegate ()
            {
                if (operation.IsCompleted)
                {
                    if (operation.IsFaulted)
                    {
                        onError(operation);
                    }
                    else
                    {
                        onCompleted(operation);
                    }
                    return false;
                }
                return true;
            });

            return disposer;
        }

        /// <summary>
        /// 当所有完成时调用 onCompleted(), 若其中一个出现错误,则调用 onError();
        /// </summary>
        public static IDisposable OnCompleted<T>(this IEnumerable<T> operations, Action onCompleted, Action<T> onError)
            where T : IAsyncOperation
        {
            if (onCompleted == null || onError == null)
                throw new ArgumentNullException();

            var operationArray = operations.ToArray();
            int index = 0;

            IDisposable disposer = new Operation(delegate ()
            {
                if (index < operationArray.Length)
                {
                    var operation = operationArray[index];
                    if (operation.IsCompleted)
                    {
                        index++;
                        if (operation.IsFaulted)
                        {
                            onError(operation);
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            });

            return disposer;
        }


        static AsyncOperationObserver operationObserver
        {
            get { return AsyncOperationObserver.Instance; }
        }

        class Operation : IDisposable
        {
            public Operation(Func<bool> moveNext)
            {
                if (moveNext == null)
                    throw new ArgumentNullException();

                this.func = moveNext;
                disposer = operationObserver.Subscribe(this);
            }

            IDisposable disposer;
            Func<bool> func;

            public bool OnNext()
            {
                return func();
            }

            public void OnCompleted()
            {
                disposer = null;
            }

            public void Dispose()
            {
                this.disposer.Dispose();
            }
        }

        class AsyncOperationObserver : MonoBehaviour
        {
            static AsyncOperationObserver operationObserver;
            static readonly object asyncLock = new object();

            public static AsyncOperationObserver Instance
            {
                get { return operationObserver != null ? operationObserver : Create(); }
            }

            static AsyncOperationObserver Create()
            {
                lock (asyncLock)
                {
                    if (operationObserver == null)
                    {
                        var gameObject = new GameObject("AsyncOperationObserver", typeof(AsyncOperationObserver));
                        operationObserver = gameObject.GetComponent<AsyncOperationObserver>();
                    }
                    return operationObserver;
                }
            }

            AsyncOperationObserver()
            {
            }

            List<Operation> operationList;

            public int Count
            {
                get { return operationList.Count; }
            }

            void Awake()
            {
                DontDestroyOnLoad(gameObject);
                operationList = new List<Operation>();
            }

            void Update()
            {
                for (int i = 0; i < operationList.Count;)
                {
                    Operation operation = operationList[i];

                    if (operation.OnNext())
                    {
                        i++;
                    }
                    else
                    {
                        operationList.RemoveAt(i);
                    }
                }
            }

            public IDisposable Subscribe(Operation operation)
            {
                operationList.Add(operation);
                return new CollectionUnsubscriber<Operation>(operationList, operation);
            }

            public bool Contains(Operation operation)
            {
                return operationList.Contains(operation);
            }

        }

    }

}
