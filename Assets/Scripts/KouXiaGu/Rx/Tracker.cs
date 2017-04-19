using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.Rx
{

    /// <summary>
    /// 订阅器基类;
    /// </summary>
    public abstract class TrackerBase<T> : IXiaGuObservable<T>
    {
        public TrackerBase()
        {
        }

        /// <summary>
        /// 观察者;
        /// </summary>
        protected abstract ICollection<IXiaGuObserver<T>> observers { get; }

        /// <summary>
        /// 观察者数量;
        /// </summary>
        public int ObserverCount
        {
            get { return observers.Count; }
        }

        /// <summary>
        /// 所有观察者;
        /// </summary>
        public IEnumerable<IXiaGuObserver<T>> Observers
        {
            get { return observers; }
        }

        /// <summary>
        /// 订阅到事件,不检查是否存在相同的订阅者;
        /// </summary>
        public virtual IDisposable Subscribe(IXiaGuObserver<T> observer)
        {
            observers.Add(observer);
            return new CollectionUnsubscriber<IXiaGuObserver<T>>(observers, observer);
        }

        /// <summary>
        /// 订阅到事件,若存在相同的订阅者则返回异常;
        /// </summary>
        public virtual IDisposable SubscribeOnly(IXiaGuObserver<T> observer)
        {
            if (observers.Contains(observer))
                throw new ArgumentException();

            observers.Add(observer);
            return new CollectionUnsubscriber<IXiaGuObserver<T>>(observers, observer);
        }

        /// <summary>
        /// 迭代获取到观察者;
        /// </summary>
        protected virtual IEnumerable<IXiaGuObserver<T>> EnumerateObserver()
        {
            IXiaGuObserver<T>[] observerArray = observers.ToArray();
            return observerArray;
        }

        /// <summary>
        /// 推送消息到所有订阅者 OnNext() 方法;
        /// </summary>
        public virtual void Track(T item)
        {
            foreach (var observer in EnumerateObserver())
            {
                try
                {
                    observer.OnNext(item);
                }
                catch (Exception observerException)
                {
                    OnError(observer, observerException);
                }
            }
        }

        /// <summary>
        /// 推送消息到所有订阅者 OnError() 方法;
        /// </summary>
        public virtual void TrackError(Exception ex)
        {
            foreach (var observer in EnumerateObserver())
            {
                try
                {
                    observer.OnError(ex);
                }
                catch (Exception observerException)
                {
                    OnError(observer, observerException);
                }
            }
        }

        /// <summary>
        /// 调用订阅者的 OnCompleted() 方法,并移除所有订阅者;
        /// </summary>
        public virtual void TrackCompleted()
        {
            foreach (var observer in EnumerateObserver())
            {
                try
                {
                    observer.OnCompleted();
                }
                catch (Exception observerException)
                {
                    OnError(observer, observerException);
                }
            }
        }

        /// <summary>
        /// 当观察者发生异常时调用;
        /// </summary>
        protected virtual void OnError(IXiaGuObserver<T> observer, Exception ex)
        {
            UnityEngine.Debug.LogError(ex);
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
    /// 加入 O(1), 移除 O(1), 推送 O(2n);
    /// </summary>
    public class HashSetTracker<T> : TrackerBase<T>
    {
        public HashSetTracker()
        {
            observersSet = new HashSet<IXiaGuObserver<T>>();
        }

        HashSet<IXiaGuObserver<T>> observersSet;

        protected override ICollection<IXiaGuObserver<T>> observers
        {
            get { return observersSet; }
        }

        public override IDisposable Subscribe(IXiaGuObserver<T> observer)
        {
            if (!observersSet.Add(observer))
                throw new ArgumentException();

            return new CollectionUnsubscriber<IXiaGuObserver<T>>(observersSet, observer);
        }

    }

    /// <summary>
    /// 使用 List 存储观察者的订阅器;
    /// 加入 O(n), 移除 O(n), 推送 O(2n);
    /// </summary>
    public class ListTracker<T> : TrackerBase<T>
    {
        public ListTracker()
        {
            observersList = new List<IXiaGuObserver<T>>();
        }

        List<IXiaGuObserver<T>> observersList;

        protected override ICollection<IXiaGuObserver<T>> observers
        {
            get { return observersList; }
        }

        public override IDisposable Subscribe(IXiaGuObserver<T> observer)
        {
            if(observersList.Contains(observer))
                throw new ArgumentException();

            observersList.Add(observer);
            return new CollectionUnsubscriber<IXiaGuObserver<T>>(observersList, observer);
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
                try
                {
                    Observers.Remove(Observer);
                }
                catch (InvalidOperationException)
                {
                }
                finally
                {
                    Observers = null;
                }
            }
        }
    }


    /// <summary>
    /// 使用 LinkedList 存储观察者的订阅器;
    /// 加入 O(1), 移除 O(1), 推送 O(n);
    /// </summary>
    public class LinkedListTracker<T> : TrackerBase<T>
    {
        public LinkedListTracker()
        {
            observersList = new LinkedList<IXiaGuObserver<T>>();
            currentNode = null;
        }

        LinkedList<IXiaGuObserver<T>> observersList;
        LinkedListNode<IXiaGuObserver<T>> currentNode;

        protected override ICollection<IXiaGuObserver<T>> observers
        {
            get { return observersList; }
        }

        /// <summary>
        /// 订阅到;
        /// </summary>
        public override IDisposable Subscribe(IXiaGuObserver<T> observer)
        {
            var node = observersList.AddLast(observer);
            return new Unsubscriber(this, node);
        }

        /// <summary>
        /// 订阅到,若已经存在此观察者,则返回异常;
        /// </summary>
        public override IDisposable SubscribeOnly(IXiaGuObserver<T> observer)
        {
            if (observersList.Contains(observer))
                throw new ArgumentException();

            var node = observersList.AddLast(observer);
            return new Unsubscriber(this, node);
        }

        /// <summary>
        /// 迭代获取到观察者;
        /// </summary>
        protected override IEnumerable<IXiaGuObserver<T>> EnumerateObserver()
        {
            currentNode = observersList.First;

            while (currentNode != null)
            {
                var observer = currentNode.Value;
                currentNode = currentNode.Next;
                yield return observer;
            }
        }

        class Unsubscriber : IDisposable
        {
            public Unsubscriber(LinkedListTracker<T> tracker, LinkedListNode<IXiaGuObserver<T>> node)
            {
                isUnsubscribed = false;
                this.tracker = tracker;
                this.node = node;
            }

            bool isUnsubscribed;
            LinkedListTracker<T> tracker;
            LinkedListNode<IXiaGuObserver<T>> node;

            LinkedList<IXiaGuObserver<T>> observers
            {
                get { return tracker.observersList; }
            }

            LinkedListNode<IXiaGuObserver<T>> currentNode
            {
                get { return tracker.currentNode; }
                set { tracker.currentNode = value; }
            }

            public void Dispose()
            {
                if (!isUnsubscribed)
                {
                    if (node == currentNode)
                    {
                        currentNode = currentNode.Next;
                    }

                    observers.Remove(node);
                    isUnsubscribed = true;
                }
            }
        }
    }

}
