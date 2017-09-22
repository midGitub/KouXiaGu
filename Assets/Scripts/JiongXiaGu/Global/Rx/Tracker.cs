using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu
{

    /// <summary>
    /// 订阅者消息传送器;
    /// </summary>
    class Tracker<T> : IObservable<T>
    {
        public Tracker()
        {
            observerCollection = new ObserverLinkedList<IObserver<T>>();
        }

        public Tracker(IObserverCollection<IObserver<T>> observerCollection)
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
}
