using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu
{


    public static partial class AsyncOperationExtensions
    {

        /// <summary>
        /// 多个操作基类;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        abstract class EnumerateSubscriberBase<T> : IUnityThreadBehaviour<Action>, IDisposable
            where T : IAsyncOperation
        {
            public EnumerateSubscriberBase(object sender, IEnumerable<T> operations)
            {
                Sender = sender;
                operationArray = operations.ToArray();
                faultedOperations = new List<T>();
            }

            int index = 0;
            T[] operationArray;
            protected List<T> faultedOperations { get; private set; }
            IDisposable disposer;

            public object Sender { get; private set; }

            public Action Action
            {
                get { return OnNext; }
            }

            protected abstract void OnCompleted(IList<T> completedOperations);
            protected abstract void OnFaulted(IList<T> faultedOperations);

            protected void SubscribeToUpdate()
            {
                disposer = UnityThreadDispatcher.Instance.SubscribeUpdate(this);
            }

            public void OnNext()
            {
                if (index < operationArray.Length)
                {
                    var operation = operationArray[index];
                    if (operation.IsCompleted)
                    {
                        index++;
                        if (operation.IsFaulted)
                        {
                            faultedOperations.Add(operation);
                        }
                    }
                    return;
                }

                if (faultedOperations.Count > 0)
                    OnFaulted(faultedOperations);
                else
                    OnCompleted(operationArray);

                Dispose();
            }

            public void Dispose()
            {
                if (disposer != null)
                {
                    disposer.Dispose();
                    disposer = null;
                }
            }
        }


        /// <summary>
        /// 当所有完成时调用 onCompleted(完成的操作), 除非出现异常,则调用 onFaulted(出现异常的操作);
        /// </summary>
        public static IDisposable Subscribe<T>(
            this IEnumerable<T> operations,
            object sender,
            Action<IList<T>> onCompleted,
            Action<IList<T>> onFaulted)
            where T : IAsyncOperation
        {
            var item = new EnumerateSubscriber<T>(sender, operations, onCompleted, onFaulted);
            return item;
        }


        class EnumerateSubscriber<T> : EnumerateSubscriberBase<T>
            where T : IAsyncOperation
        {
            public EnumerateSubscriber(object sender, IEnumerable<T> operations, 
                Action<IList<T>> onCompleted, Action<IList<T>> onFaulted)
                : base(sender, operations)
            {
                if (operations == null || onCompleted == null || onFaulted == null)
                    throw new ArgumentNullException();

                this.onCompleted = onCompleted;
                this.onFaulted = onFaulted;
                SubscribeToUpdate();
            }

            Action<IList<T>> onCompleted;
            Action<IList<T>> onFaulted;

            protected override void OnCompleted(IList<T> completedOperations)
            {
                onCompleted(completedOperations);
            }

            protected override void OnFaulted(IList<T> faultedOperations)
            {
                onFaulted(faultedOperations);
            }
        }

    }

}
