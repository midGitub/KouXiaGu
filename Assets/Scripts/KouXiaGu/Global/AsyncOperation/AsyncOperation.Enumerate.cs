using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{


    public static partial class AsyncOperationExtensions
    {

        /// <summary>
        /// 多个操作基类;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        abstract class EnumerateSubscriberBase<T> : UnityThreadEvent
            where T : IAsyncOperation
        {
            public EnumerateSubscriberBase(IEnumerable<T> operations)
            {
                operationArray = operations.ToArray();
                faultedOperations = new List<T>();
            }

            int index = 0;
            T[] operationArray;
            protected List<T> faultedOperations { get; private set; }

            protected abstract void OnCompleted(IList<T> completedOperations);
            protected abstract void OnFaulted(IList<T> faultedOperations);

            public override void OnNext()
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
        }


        /// <summary>
        /// 当所有完成时调用 onCompleted(完成的操作), 除非出现异常,则调用 onFaulted(出现异常的操作);
        /// </summary>
        public static IDisposable Subscribe<T>(
            this IEnumerable<T> operations,
            Action<IList<T>> onCompleted,
            Action<IList<T>> onFaulted)
            where T : IAsyncOperation
        {
            var item = new EnumerateSubscriber<T>(operations, onCompleted, onFaulted);
            var disposer = item.SubscribeUpdate();
            return disposer;
        }


        class EnumerateSubscriber<T> : EnumerateSubscriberBase<T>
            where T : IAsyncOperation
        {
            public EnumerateSubscriber(IEnumerable<T> operations, Action<IList<T>> onCompleted, Action<IList<T>> onFaulted)
                : base(operations)
            {
                if (operations == null || onCompleted == null || onFaulted == null)
                    throw new ArgumentNullException();

                this.onCompleted = onCompleted;
                this.onFaulted = onFaulted;
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
