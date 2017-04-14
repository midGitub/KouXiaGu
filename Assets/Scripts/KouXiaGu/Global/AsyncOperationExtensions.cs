using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Rx;
using UnityEngine;

namespace KouXiaGu
{

    internal interface IOperation
    {
        /// <summary>
        /// 继续返回true,需要结束返回fasle;
        /// </summary>
        bool MoveNext();
    }

    public static class OperationExtensions
    {
        static OperationObserver operationObserver
        {
            get { return OperationObserver.Instance; }
        }

        internal static IDisposable AddOperationObserver(this IOperation operation)
        {
            return operationObserver.Subscribe(operation);
        }
    }

    class OperationObserver : MonoBehaviour
    {
        static OperationObserver operationObserver;
        static readonly object asyncLock = new object();

        public static OperationObserver Instance
        {
            get { return operationObserver != null ? operationObserver : Create(); }
        }

        static OperationObserver Create()
        {
            lock (asyncLock)
            {
                if (operationObserver == null)
                {
                    var gameObject = new GameObject("OperationObserver", typeof(OperationObserver));
                    operationObserver = gameObject.GetComponent<OperationObserver>();
                }
                return operationObserver;
            }
        }

        OperationObserver()
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
    /// 异步操作拓展,部分需要在 unity 线程内调用;
    /// </summary>
    public static class AsyncOperationExtensions
    {
        static OperationObserver operationObserver
        {
            get { return OperationObserver.Instance; }
        }

        internal static IDisposable AddOperationObserver(this IOperation operation)
        {
            return operationObserver.Subscribe(operation);
        }


        /// <summary>
        /// 当操作完成时,在unity线程内回调;
        /// </summary>
        /// <returns>返回传入参数 operation</returns>
        public static T Subscribe<T>(this T operation, Action<T> onCompleted, Action<T> onFaulted)
            where T : IAsyncOperation
        {
            var item = new Subscriber<T>(operation, onCompleted, onFaulted);
            AddOperationObserver(item);
            return operation;
        }

        /// <summary>
        /// 当操作完成时,在unity线程内回调;
        /// </summary>
        /// <returns>返回传入参数 operation</returns>
        public static T Subscribe<T>(this T operation, Action<T> onCompleted, Action<T> onFaulted, out IDisposable disposer)
            where T : IAsyncOperation
        {
            var item = new Subscriber<T>(operation, onCompleted, onFaulted);
            disposer = AddOperationObserver(item);
            return operation;
        }

        class Subscriber<T> : IOperation
             where T : IAsyncOperation
        {
            public Subscriber(T operation, Action<T> onCompleted, Action<T> onFaulted)
            {
                if (operation == null || onCompleted == null || onFaulted == null)
                    throw new ArgumentNullException();

                this.operation = operation;
                this.onCompleted = onCompleted;
                this.onFaulted = onFaulted;
            }

            T operation;
            Action<T> onCompleted;
            Action<T> onFaulted;

            bool IOperation.MoveNext()
            {
                if (operation.IsCompleted)
                {
                    if (operation.IsFaulted)
                    {
                        onFaulted(operation);
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
        /// 当所有完成时调用 onCompleted(完成的操作), 除非出现异常,则调用 onFaulted(出现异常的操作);
        /// </summary>
        public static IDisposable Subscribe<T>(this IEnumerable<T> operations, Action<IList<T>> onCompleted, Action<IList<T>> onFaulted)
            where T : IAsyncOperation
        {
            var item = new EnumerateSubscriber<T>(operations, onCompleted, onFaulted);
            var disposer = AddOperationObserver(item);
            return disposer;
        }

        class EnumerateSubscriber<T> : IOperation
            where T : IAsyncOperation
        {
            public EnumerateSubscriber(IEnumerable<T> operations, Action<IList<T>> onCompleted, Action<IList<T>> onFaulted)
            {
                if (operations == null || onCompleted == null || onFaulted == null)
                    throw new ArgumentNullException();

                this.operationArray = operations.ToArray();
                this.onCompleted = onCompleted;
                this.onFaulted = onFaulted;
                errors = new List<T>();
            }

            int index = 0;
            T[] operationArray;
            List<T> errors;
            Action<IList<T>> onCompleted;
            Action<IList<T>> onFaulted;

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
                            errors.Add(operation);
                        }
                    }
                    return true;
                }

                if (errors.Count > 0)
                    onFaulted(errors);
                else
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


        /// <summary>
        /// 将错误操作转换为异常;
        /// </summary>
        public static AggregateException ToAggregateException(this IEnumerable<IAsyncOperation> operations)
        {
            List<Exception> faults = new List<Exception>();

            foreach (var operation in operations)
            {
                if (!operation.IsCompleted)
                    throw new ArgumentException();

                if (operation.IsFaulted)
                    faults.Add(operation.Exception);
            }

            return new AggregateException(faults);
        }

        /// <summary>
        /// 将错误操作转换为异常;
        /// </summary>
        public static AggregateException ToAggregateException(this IEnumerable<IAsyncOperation> operations, AggregateException exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            List<Exception> faults = new List<Exception>(exception.InnerExceptions);

            foreach (var operation in operations)
            {
                if (!operation.IsCompleted)
                    throw new ArgumentException("存在未完成;");

                if (operation.IsFaulted)
                    faults.Add(operation.Exception);
            }

            return new AggregateException(faults);
        }

    }

}
