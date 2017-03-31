using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.Rx
{

    /// <summary>
    /// 订阅器基类;
    /// </summary>
    public abstract class Tracker<T> : IObservable<T>
    {
        public Tracker()
        {
        }

        /// <summary>
        /// 观察者;
        /// </summary>
        protected abstract ICollection<IObserver<T>> observers { get; }

        /// <summary>
        /// 观察者数量;
        /// </summary>
        public int Count
        {
            get { return observers.Count; }
        }

        /// <summary>
        /// 所有观察者;
        /// </summary>
        public IEnumerable<IObserver<T>> Observers
        {
            get { return observers; }
        }

        /// <summary>
        /// 订阅到事件;
        /// </summary>
        public abstract IDisposable Subscribe(IObserver<T> observer);

        /// <summary>
        /// 推送消息到所有订阅者 OnNext() 方法;
        /// </summary>
        public virtual void Track(T item)
        {
            IObserver<T>[] observerArray = observers.ToArray();
            foreach (var observer in observerArray)
            {
                observer.OnNext(item);
            }
        }

        /// <summary>
        /// 推送消息到所有订阅者 OnError() 方法;
        /// </summary>
        public virtual void Track(Exception ex)
        {
            IObserver<T>[] observerArray = observers.ToArray();
            foreach (var observer in observerArray)
            {
                observer.OnError(ex);
            }
        }

        /// <summary>
        /// 调用订阅者的 OnCompleted() 方法;
        /// </summary>
        public virtual void EndTrack()
        {
            IObserver<T>[] observerArray = observers.ToArray();
            foreach (var observer in observerArray)
            {
                observer.OnCompleted();
            }
        }
    }

    /// <summary>
    /// 取消订阅器,不进行 Add() 操作;;
    /// </summary>
    class CollectionUnsubscriber<T> : IDisposable
    {
        /// <summary>
        /// 不进行 Add() 操作;
        /// </summary>
        public CollectionUnsubscriber(ICollection<T> collection, T observer)
        {
            if (collection == null || observer == null)
                throw new ArgumentNullException();

            Collection = collection;
            Observer = observer;
        }

        public ICollection<T> Collection { get; private set; }
        public T Observer { get; private set; }

        public void Dispose()
        {
            if (Collection != null)
            {
                Collection.Remove(Observer);
                Collection = null;
            }
        }

    }

    /// <summary>
    /// 使用 HashSet 存储观察者的订阅器;
    /// 加入 O(1), 移除 O(1);
    /// </summary>
    public class HashSetTracker<T> : Tracker<T>
    {
        public HashSetTracker()
        {
            observersSet = new HashSet<IObserver<T>>();
        }

        HashSet<IObserver<T>> observersSet;

        protected override ICollection<IObserver<T>> observers
        {
            get { return observersSet; }
        }

        public override IDisposable Subscribe(IObserver<T> observer)
        {
            if (!observersSet.Add(observer))
                throw new ArgumentException();

            return new CollectionUnsubscriber<IObserver<T>>(observersSet, observer);
        }

    }

    /// <summary>
    /// 使用 List 存储观察者的订阅器;
    /// 加入 O(n), 移除 O(n);
    /// </summary>
    public class ListTracker<T> : Tracker<T>
    {
        public ListTracker()
        {
            observersList = new List<IObserver<T>>();
        }

        List<IObserver<T>> observersList;

        protected override ICollection<IObserver<T>> observers
        {
            get { return observersList; }
        }

        public override IDisposable Subscribe(IObserver<T> observer)
        {
            if(observersList.Contains(observer))
                throw new ArgumentException();

            observersList.Add(observer);
            return new CollectionUnsubscriber<IObserver<T>>(observersList, observer);
        }

    }

    /// <summary>
    /// 取消订阅器,不进行 Add() 操作;;
    /// </summary>
    public class LinkedListUnsubscriber<T> : IDisposable
    {
        public LinkedListUnsubscriber(LinkedList<T> observers, LinkedListNode<T> observer)
        {
            if (observers == null || observer == null)
                throw new ArgumentNullException();

            Observers = observers;
            Observer = observer;
        }

        public LinkedList<T> Observers { get; private set; }
        LinkedListNode<T> Observer;

        public void Dispose()
        {
            if (Observers != null)
            {
                Observers.Remove(Observer);
                Observers = null;
            }
        }
    }

    /// <summary>
    /// 使用 LinkedList 存储观察者的订阅器;
    /// 加入 O(n), 移除 O(1);
    /// </summary>
    public class LinkedListTracker<T> : Tracker<T>
    {
        public LinkedListTracker()
        {
            observersList = new LinkedList<IObserver<T>>();
        }

        LinkedList<IObserver<T>> observersList;

        protected override ICollection<IObserver<T>> observers
        {
            get { return observersList; }
        }

        /// <summary>
        /// 订阅到,若已经存在此观察者,则返回异常;
        /// </summary>
        public override IDisposable Subscribe(IObserver<T> observer)
        {
            if (observersList.Contains(observer))
                throw new ArgumentException();

            var node = observersList.AddLast(observer);
            return new LinkedListUnsubscriber<IObserver<T>>(observersList, node);
        }
       
    }

}
