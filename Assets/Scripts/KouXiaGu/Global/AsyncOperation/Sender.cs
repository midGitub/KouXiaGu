using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace KouXiaGu
{

    /// <summary>
    /// 订阅者消息传送器;
    /// </summary>
    class Sender<T> : IObservable<T>
    {
        public Sender()
        {
            observerCollection = new ObserverLinkedList<IObserver<T>>();
        }

        public Sender(IObserverCollection<IObserver<T>> observerCollection)
        {
            this.observerCollection = observerCollection;
        }

        readonly IObserverCollection<IObserver<T>> observerCollection;

        public virtual IDisposable Subscribe(IObserver<T> observer)
        {
            return observerCollection.Subscribe(observer);
        }

        public void Send(T item)
        {
            foreach (var observer in observerCollection.EnumerateObserver())
            {
                observer.OnNext(item);
            }
        }

        public void SendError(Exception ex)
        {
            foreach (var observer in observerCollection.EnumerateObserver())
            {
                observer.OnError(ex);
            }
        }

        /// <summary>
        /// 传送完成信息,并且清空订阅合集;
        /// </summary>
        public void SendCompleted()
        {
            foreach (var observer in observerCollection.EnumerateObserver())
            {
                observer.OnCompleted();
            }
            Clear();
        }

        public void Clear()
        {
            observerCollection.Clear();
        }
    }

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
