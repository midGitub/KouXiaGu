using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Rx;
using UnityEngine;

namespace KouXiaGu
{



    /// <summary>
    /// 异步操作拓展,部分需要在 unity 线程内调用;
    /// </summary>
    public static class AsyncOperationExtensions
    {

        interface IOperation
        {
            /// <summary>
            /// 继续返回true,需要结束返回fasle;
            /// </summary>
            bool MoveNext();
        }

        static IDisposable AddOperation(IOperation operation)
        {
            return operationObserver.Subscribe(operation);
        }

        static AsyncOperationObserver operationObserver
        {
            get { return AsyncOperationObserver.Instance; }
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
                    var operation = operationList[i];

                    if (operation.MoveNext())
                    {
                        i++;
                    }
                    else
                    {
                        operationList.RemoveAt(i);
                    }
                }
            }

            public IDisposable Subscribe(IOperation operation)
            {
                operationList.Add(operation);
                return new CollectionUnsubscriber<IOperation>(operationList, operation);
            }

            public bool Contains(IOperation operation)
            {
                return operationList.Contains(operation);
            }

        }


        /// <summary>
        /// 当操作完成时,在unity线程内回调;
        /// </summary>
        /// <returns>返回传入参数 operation</returns>
        public static T Subscribe<T>(this T operation, Action<T> onCompleted, Action<T> onError)
            where T : IAsyncOperation
        {
            if (onCompleted == null || onError == null)
                throw new ArgumentNullException();

            var item = new Subscriber<T>(operation, onCompleted, onError);
            AddOperation(item);
            return operation;
        }

        /// <summary>
        /// 当操作完成时,在unity线程内回调;
        /// </summary>
        /// <returns>返回传入参数 operation</returns>
        public static T Subscribe<T>(this T operation, Action<T> onCompleted, Action<T> onError, out IDisposable disposer)
            where T : IAsyncOperation
        {
            if (onCompleted == null || onError == null)
                throw new ArgumentNullException();

            var item = new Subscriber<T>(operation, onCompleted, onError);
            disposer = AddOperation(item);
            return operation;
        }

        class Subscriber<T> : IOperation
             where T : IAsyncOperation
        {
            public Subscriber(T operation, Action<T> onCompleted, Action<T> onError)
            {
                this.operation = operation;
                this.onCompleted = onCompleted;
                this.onError = onError;
            }

            T operation;
            Action<T> onCompleted;
            Action<T> onError;

            bool IOperation.MoveNext()
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
            }
        }



        /// <summary>
        /// 当所有完成时调用 onCompleted(), 对出现异常的操作,分别调用 onError();
        /// </summary>
        public static IDisposable Subscribe<T>(this IEnumerable<T> operations, Action<IEnumerable<T>> onCompleted, Action<T> onError)
            where T : IAsyncOperation
        {
            if (onCompleted == null || onError == null)
                throw new ArgumentNullException();

            var item = new EnumerateSubscriber<T>(operations, onCompleted, onError);
            var disposer = AddOperation(item);
            return disposer;
        }

        class EnumerateSubscriber<T> : IOperation
            where T : IAsyncOperation
        {
            public EnumerateSubscriber(IEnumerable<T> operations, Action<IEnumerable<T>> onCompleted, Action<T> onError)
            {
                this.operationArray = operations.ToArray();
                this.onCompleted = onCompleted;
                this.onError = onError;
            }

            int index = 0;
            T[] operationArray;
            Action<IEnumerable<T>> onCompleted;
            Action<T> onError;

            bool IOperation.MoveNext()
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
                        }
                    }
                    return true;
                }
                onCompleted(operationArray);
                return false;
            }
        }


        /// <summary>
        /// 获取到枚举结构,当操作完成时结束枚举;
        /// </summary>
        public static IEnumerator GetEnumerator(this IAsyncOperation operation)
        {
            return new Enumerate(operation);
        }

        class Enumerate : IEnumerator
        {
            public Enumerate(IAsyncOperation operation)
            {
                this.operation = operation;
            }

            IAsyncOperation operation;

            object IEnumerator.Current
            {
                get { return operation; }
            }

            bool IEnumerator.MoveNext()
            {
                return !operation.IsCompleted;
            }

            void IEnumerator.Reset()
            {
                return;
            }
        }

    }

}
