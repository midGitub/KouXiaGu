using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{


    class AsyncOperationObserver : IObserver<UnityThreadDispatcher>, IDisposable
    {
        public AsyncOperationObserver()
        {

        }

        void IObserver<UnityThreadDispatcher>.OnCompleted()
        {
            throw new NotImplementedException();
        }

        void IObserver<UnityThreadDispatcher>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        void IObserver<UnityThreadDispatcher>.OnNext(UnityThreadDispatcher item)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 异步操作拓展,部分需要在 unity 线程内调用;
    /// </summary>
    public static partial class AsyncOperationExtensions
    {

        /// <summary>
        /// 当操作失败时,在unity线程内回调;
        /// </summary>
        /// <returns>返回传入参数 operation</returns>
        public static T SubscribeFaulted<T>(this T operation, Action<T> onFaulted)
            where T : IAsyncOperation
        {
            var item = new FaultedSubscriber<T>(operation, onFaulted);
            item.SubscribeUpdate();
            return operation;
        }

        /// <summary>
        /// 失败时调用;
        /// </summary>
        class FaultedSubscriber<T> : UnityThreadEvent
            where T : IAsyncOperation
        {
            public FaultedSubscriber(T operation, Action<T> onFaulted)
            {
                if (operation == null || onFaulted == null)
                    throw new ArgumentNullException();

                this.operation = operation;
                this.onFaulted = onFaulted;
            }

            T operation;
            Action<T> onFaulted;

            public override void OnNext()
            {
                if (operation.IsCompleted)
                {
                    if (operation.IsFaulted)
                    {
                        onFaulted(operation);
                    }
                    Dispose();
                }
            }

        }


        /// <summary>
        /// 完成时调用,若失败则不调用;在unity线程内回调;
        /// </summary>
        /// <returns>返回传入参数 operation</returns>
        public static T SubscribeCompleted<T>(this T operation, Action<T> onCompleted)
            where T : IAsyncOperation
        {
            var item = new CompletedSubscriber<T>(operation, onCompleted);
            item.SubscribeUpdate();
            return operation;
        }

        /// <summary>
        /// 监视完成时调用,若失败则不调用;
        /// </summary>
        class CompletedSubscriber<T> : UnityThreadEvent
            where T : IAsyncOperation
        {
            public CompletedSubscriber(T operation, Action<T> onCompleted)
            {
                if (operation == null || onCompleted == null)
                    throw new ArgumentNullException();

                this.operation = operation;
                this.onCompleted = onCompleted;
            }

            T operation;
            Action<T> onCompleted;

            public override void OnNext()
            {
                if (operation.IsCompleted)
                {
                    if (!operation.IsFaulted)
                    {
                        onCompleted(operation);
                    }
                    Dispose();
                }
            }
        }

        /// <summary>
        /// 当操作完成时,在unity线程内回调;
        /// </summary>
        /// <returns>返回传入参数 operation</returns>
        public static IAsyncOperation<TReturn> Subscribe<TReturn>(
            this IAsyncOperation<TReturn> operation,
            Action<IAsyncOperation<TReturn>, TReturn> onCompleted,
            Action<IAsyncOperation<TReturn>> onFaulted)
        {
            var item = new ReturnSubscriber<TReturn>(operation, onCompleted, onFaulted);
            item.SubscribeUpdate();
            return operation;
        }

        class ReturnSubscriber<TReturn> : UnityThreadEvent
        {
            public ReturnSubscriber(
                IAsyncOperation<TReturn> operation,
                Action<IAsyncOperation<TReturn>, TReturn> onCompleted,
                Action<IAsyncOperation<TReturn>> onFaulted)
            {
                if (operation == null || onCompleted == null || onFaulted == null)
                    throw new ArgumentNullException();

                this.operation = operation;
                this.onCompleted = onCompleted;
                this.onFaulted = onFaulted;
            }

            IAsyncOperation<TReturn> operation;
            Action<IAsyncOperation<TReturn>, TReturn> onCompleted;
            Action<IAsyncOperation<TReturn>> onFaulted;

            public override void OnNext()
            {
                if (operation.IsCompleted)
                {
                    if (operation.IsFaulted)
                    {
                        onFaulted(operation);
                    }
                    else
                    {
                        onCompleted(operation, operation.Result);
                    }
                    Dispose();
                }
            }
        }


        /// <summary>
        /// 当操作完成时,在unity线程内回调;
        /// </summary>
        /// <returns>返回传入参数 operation</returns>
        public static T Subscribe<T>(this T operation, Action<T> onCompleted, Action<T> onFaulted)
            where T : IAsyncOperation
        {
            var item = new Subscriber<T>(operation, onCompleted, onFaulted);
            item.SubscribeUpdate();
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
            disposer = item.SubscribeUpdate();
            return operation;
        }

        /// <summary>
        /// 监视完成和失败;
        /// </summary>
        class Subscriber<T> : UnityThreadEvent
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

            public override void OnNext()
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
                    Dispose();
                }
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
