using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace KouXiaGu
{

    /// <summary>
    /// 异步返回值订阅;
    /// </summary>
    class AsyncResultSender<T> : Sender<T>
    {
        public AsyncResultSender(IAsyncOperation<T> asyncOperation)
        {
            this.asyncOperation = asyncOperation;
        }

        public AsyncResultSender(IAsyncOperation<T> asyncOperation, IObserverCollection<IObserver<T>> observerCollection)
            : base(observerCollection)
        {
            this.asyncOperation = asyncOperation;
        }

        readonly IAsyncOperation<T> asyncOperation;

        public IAsyncOperation<T> AsyncOperation
        {
            get { return asyncOperation; }
        }

        /// <summary>
        /// 订阅到,若已经存在结果则返回Null,并且调用委托;
        /// </summary>
        public override IDisposable Subscribe(IObserver<T> observer)
        {
            if (asyncOperation.IsFaulted)
            {
                observer.OnError(asyncOperation.Exception);
            }
            else if (asyncOperation.IsCompleted)
            {
                observer.OnNext(asyncOperation.Result);
            }
            else
            {
                return base.Subscribe(observer);
            }
            observer.OnCompleted();
            return null;
        }
    }

}
