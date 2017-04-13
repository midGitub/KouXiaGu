using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// 表示在多线程内进行的操作;
    /// </summary>
    public abstract class AsyncOperation<TResult> : IAsyncOperation<TResult>
    {
        public AsyncOperation()
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
    /// 表示在协程内进行的操作;
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

    public interface IOperation
    {
        bool OnNext();
    }


    /// <summary>
    /// 异步操作拓展,都在 unity 线程内调用;
    /// </summary>
    public static class AsyncOperationExtensions
    {

        /// <summary>
        /// 当操作完成时,在unity线程内回调;
        /// </summary>
        public static void Subscribe<T>(this T operation, Action<T> onCompleted, Action<T> onError)
            where T : IAsyncOperation
        {
            if (onCompleted == null || onError == null)
                throw new ArgumentNullException();

            AddOperation(delegate ()
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
        }


        static AsyncOperationObserver operationObserver
        {
            get { return AsyncOperationObserver.Instance; }
        }

        static void AddOperation(Func<bool> moveNext)
        {
            operationObserver.Add(new Operation(moveNext));
        }

        class Operation : IOperation
        {
            /// <summary>
            /// 构造;
            /// </summary>
            /// <param name="func">不允许出现异常;</param>
            public Operation(Func<bool> func)
            {
                if (func == null)
                    throw new ArgumentNullException();

                this.func = func;
            }

            Func<bool> func;

            public bool OnNext()
            {
                return func();
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

            List<IOperation> operationList;

            public int Count
            {
                get { return operationList.Count; }
            }

            void Awake()
            {
                DontDestroyOnLoad(gameObject);
                operationList = new List<IOperation>();
            }

            void Update()
            {
                for (int i = 0; i < operationList.Count;)
                {
                    IOperation operation = operationList[i];

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

            public void Add(IOperation operation)
            {
                operationList.Add(operation);
            }

            public bool Remove(IOperation operation)
            {
                return operationList.Remove(operation);
            }

            public bool Contains(IOperation operation)
            {
                return operationList.Contains(operation);
            }

        }

    }

}
