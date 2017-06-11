using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{


    public static partial class AsyncOperationExtensions
    {

        abstract class SubscriberBase<T> : UnityThreadBehaviour
            where T : IAsyncOperation
        {
            public SubscriberBase(object sender, T operation)
                : base(sender)
            {
                this.operation = operation;
            }

            protected T operation { get; private set; }

            protected abstract void OnCompleted(T operation);
            protected abstract void OnFaulted(T operation);

            protected override void OnNext()
            {
                if (operation.IsCompleted)
                {
                    if (operation.IsFaulted)
                    {
                        OnFaulted(operation);
                    }
                    else
                    {
                        OnCompleted(operation);
                    }
                    Dispose();
                }
            }
        }



        /// <summary>
        /// 当操作失败时,在unity线程内回调;
        /// </summary>
        /// <returns>返回传入参数 operation</returns>
        public static T SubscribeFaulted<T>(this T operation, object sender, Action<T> onFaulted)
            where T : IAsyncOperation
        {
            new FaultedSubscriber<T>(sender, operation, onFaulted);
            return operation;
        }

        /// <summary>
        /// 失败时调用;
        /// </summary>
        class FaultedSubscriber<T> : SubscriberBase<T>
            where T : IAsyncOperation
        {
            public FaultedSubscriber(object sender, T operation, Action<T> onFaulted)
                : base(sender, operation)
            {
                if (operation == null || onFaulted == null)
                    throw new ArgumentNullException();

                this.onFaulted = onFaulted;
                SubscribeToUpdate();
            }

            Action<T> onFaulted;

            protected override void OnCompleted(T operation)
            {
                return;
            }

            protected override void OnFaulted(T operation)
            {
                onFaulted(operation);
            }
        }



        /// <summary>
        /// 完成时调用,若失败则不调用;在unity线程内回调;
        /// </summary>
        /// <returns>返回传入参数 operation</returns>
        public static T SubscribeCompleted<T>(this T operation, object sender, Action<T> onCompleted)
            where T : IAsyncOperation
        {
            new CompletedSubscriber<T>(sender, operation, onCompleted);
            return operation;
        }

        /// <summary>
        /// 监视完成时调用,若失败则不调用;
        /// </summary>
        class CompletedSubscriber<T> : SubscriberBase<T>
            where T : IAsyncOperation
        {
            public CompletedSubscriber(object sender, T operation, Action<T> onCompleted)
                : base(sender, operation)
            {
                if (operation == null || onCompleted == null)
                    throw new ArgumentNullException();

                this.onCompleted = onCompleted;
                SubscribeToUpdate();
            }

            Action<T> onCompleted;

            protected override void OnCompleted(T operation)
            {
                onCompleted(operation);
            }

            protected override void OnFaulted(T operation)
            {
                return;
            }
        }



        /// <summary>
        /// 当操作完成时,在unity线程内回调;
        /// </summary>
        /// <returns>返回传入参数 operation</returns>
        public static IAsyncOperation<TReturn> Subscribe<TReturn>(
            this IAsyncOperation<TReturn> operation,
            object sender,
            Action<IAsyncOperation<TReturn>, TReturn> onCompleted,
            Action<IAsyncOperation<TReturn>> onFaulted)
        {
            new ReturnSubscriber<TReturn>(sender, operation, onCompleted, onFaulted);
            return operation;
        }

        class ReturnSubscriber<TReturn> : SubscriberBase<IAsyncOperation<TReturn>>
        {
            public ReturnSubscriber(
				object sender,
                IAsyncOperation<TReturn> operation,
                Action<IAsyncOperation<TReturn>, TReturn> onCompleted,
                Action<IAsyncOperation<TReturn>> onFaulted)
				: base(sender, operation)
            {
                if (operation == null || onCompleted == null || onFaulted == null)
                    throw new ArgumentNullException();

                this.onCompleted = onCompleted;
                this.onFaulted = onFaulted;
                SubscribeToUpdate();
            }

            Action<IAsyncOperation<TReturn>, TReturn> onCompleted;
            Action<IAsyncOperation<TReturn>> onFaulted;

            protected override void OnCompleted(IAsyncOperation<TReturn> operation)
            {
                onCompleted(operation, operation.Result);
            }

            protected override void OnFaulted(IAsyncOperation<TReturn> operation)
            {
                onFaulted(operation);
            }
        }



        /// <summary>
        /// 当操作完成时,在unity线程内回调;
        /// </summary>
        /// <returns>返回传入参数 operation</returns>
        public static T Subscribe<T>(this T operation, object sender,
            Action<T> onCompleted, Action<T> onFaulted)
            where T : IAsyncOperation
        {
            new Subscriber<T>(sender, operation, onCompleted, onFaulted);
            return operation;
        }

        /// <summary>
        /// 当操作完成时,在unity线程内回调;
        /// </summary>
        /// <returns>返回传入参数 operation</returns>
        public static T Subscribe<T>(this T operation, object sender, 
			Action<T> onCompleted, Action<T> onFaulted, out IDisposable disposer)
            where T : IAsyncOperation
        {
            var item = new Subscriber<T>(sender, operation, onCompleted, onFaulted);
            disposer = item;
            return operation;
        }

        /// <summary>
        /// 监视完成和失败;
        /// </summary>
        class Subscriber<T> : SubscriberBase<T>
             where T : IAsyncOperation
        {
            public Subscriber(object sender, T operation, Action<T> onCompleted, Action<T> onFaulted)
				: base(sender, operation)
            {
                if (operation == null || onCompleted == null || onFaulted == null)
                    throw new ArgumentNullException();

                this.onCompleted = onCompleted;
                this.onFaulted = onFaulted;
                SubscribeToUpdate();
            }

            Action<T> onCompleted;
            Action<T> onFaulted;

            protected override void OnCompleted(T operation)
            {
                onCompleted(operation);
            }

            protected override void OnFaulted(T operation)
            {
                onFaulted(operation);
            }
        }

    }

}
