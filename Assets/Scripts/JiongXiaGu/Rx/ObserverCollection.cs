using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu
{

    /// <summary>
    /// 观察者合集;
    /// </summary>
    public interface IObserverCollection<T> : IReadOnlyCollection<T>
    {
        //int Count { get; }

        ///// <summary>
        ///// 不允许对合集进行更改的迭代结构;
        ///// </summary>
        //IEnumerable<T> Observers { get; }

        /// <summary>
        /// 订阅到;
        /// </summary>
        IDisposable Subscribe(T observer);

        /// <summary>
        /// 迭代获取到观察者,并且在迭代过程中允许改变合集元素;
        /// 在迭代时对合集的更改,不会体现在本次迭代;
        /// </summary>
        IReadOnlyCollection<T> EnumerateObserver();

        /// <summary>
        /// 确认是否存在此元素;
        /// </summary>
        bool Contains(T item);
    }

    /// <summary>
    /// 使用双向链表存储观察者,添加O(1), 移除O(1), 迭代O(n);
    /// </summary>
    public class ObserverLinkedList<T> : IObserverCollection<T>
    {
        public ObserverLinkedList()
        {
            observers = new LinkedList<T>();
            tempObservers = new List<T>();
            hasChanged = false;
        }

        public ObserverLinkedList(IEnumerable<T> observers)
        {
            this.observers = new LinkedList<T>(observers);
            tempObservers = new List<T>(this.observers);
            hasChanged = false;
        }

        readonly LinkedList<T> observers;
        readonly List<T> tempObservers;
        bool hasChanged;

        public int Count
        {
            get { return observers.Count; }
        }

        public IDisposable Subscribe(T observer)
        {
            LinkedListNode<T> node;
            node = observers.AddFirst(observer);
            var unsubscriber = new Unsubscriber(this, node);
            hasChanged = true;
            return unsubscriber;
        }

        public IReadOnlyCollection<T> EnumerateObserver()
        {
            if (hasChanged)
            {
                tempObservers.Clear();
                tempObservers.AddRange(observers);
                hasChanged = false;
            }
            return tempObservers;
        }

        public bool Contains(T item)
        {
            return observers.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return observers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        class Unsubscriber : IDisposable
        {
            private bool isDisposed = false;
            private ObserverLinkedList<T> parent;
            private LinkedListNode<T> node;

            public Unsubscriber(ObserverLinkedList<T> parent, LinkedListNode<T> node)
            {
                this.parent = parent;
                this.node = node;
            }

            ~Unsubscriber()
            {
                Dispose(false);
            }

            LinkedList<T> observers
            {
                get { return parent.observers; }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            void Dispose(bool disposing)
            {
                if (!isDisposed)
                {
                    observers.Remove(node);
                    parent = null;
                    node = null;
                    isDisposed = true;
                }
            }
        }
    }

    /// <summary>
    /// 使用定义的 ICollection<T> 接口存储观察者,迭代时需要临时空间;
    /// </summary>
    public class ObserverCollection<T> : IObserverCollection<T>
    {
        public ObserverCollection(ICollection<T> observers)
        {
            this.observers = observers;
            tempObservers = new List<T>(observers);
            hasChanged = false;
        }

        readonly ICollection<T> observers;
        readonly List<T> tempObservers;
        bool hasChanged;

        public int Count
        {
            get { return observers.Count; }
        }

        public IDisposable Subscribe(T observer)
        {
            observers.Add(observer);
            var unsubscriber = new Unsubscriber(observers, observer);
            hasChanged = true;
            return unsubscriber;
        }

        public IReadOnlyCollection<T> EnumerateObserver()
        {
            if (hasChanged)
            {
                tempObservers.Clear();
                tempObservers.AddRange(observers);
                hasChanged = false;
            }
            return tempObservers;
        }

        public bool Contains(T item)
        {
            return observers.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return observers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        class Unsubscriber : IDisposable
        {
            private bool isDisposed;
            private ICollection<T> observersCollection;
            private T observer;

            public Unsubscriber(ICollection<T> observersCollection, T observer)
            {
                isDisposed = false;
                this.observersCollection = observersCollection;
                this.observer = observer;
            }

            ~Unsubscriber()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            void Dispose(bool disposing)
            {
                if (!isDisposed)
                {
                    observersCollection.Remove(observer);

                    observersCollection = null;
                    observer = default(T);
                    isDisposed = true;
                }
            }
        }
    }

    /// <summary>
    /// 允许重复加入的订阅合集;加入O(1),移除O(n), 迭代O(n);
    /// </summary>
    public class ObserverList<T> : ObserverCollection<T>
    {
        public ObserverList() : base(new List<T>())
        {
        }

        public ObserverList(IEnumerable<T> observers) : base(new List<T>(observers))
        {
        }
    }

    /// <summary>
    /// 不允许重复订阅的合集;加入O(1), 移除O(1), 迭代O(n);
    /// </summary>
    public class ObserverHashSet<T> : ObserverCollection<T>
    {
        public ObserverHashSet() : base(new HashSet<T>())
        {
        }

        public ObserverHashSet(IEnumerable<T> observers) : base(new HashSet<T>(observers))
        {
        }
    }
}
