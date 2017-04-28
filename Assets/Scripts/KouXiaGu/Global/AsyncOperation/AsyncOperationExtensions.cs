using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 异步操作拓展,部分需要在 unity 线程内调用;
    /// </summary>
    public static partial class AsyncOperationExtensions
    {

        abstract class UnityThreadBehaviour : IUnityThreadBehaviour<Action>, IDisposable
        {
            public UnityThreadBehaviour(object sender)
            {
                Sender = sender;
            }

            IDisposable disposer;
            public object Sender { get; private set; }

            public Action Action
            {
                get { return OnNext; }
            }

            protected abstract void OnNext();

            protected void SubscribeToUpdate()
            {
                disposer = UnityThreadDispatcher.Instance.SubscribeUpdate(this);
            }

            public void Dispose()
            {
                if (disposer != null)
                {
                    disposer.Dispose();
                    disposer = null;
                }
            }

            public override string ToString()
            {
                return Sender.ToString();
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
