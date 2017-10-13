using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu
{

    public interface IObserverCollection<T>
    {
        int Count { get; }

        /// <summary>
        /// 不允许对合集进行更改的迭代结构;
        /// </summary>
        IEnumerable<T> Observers { get; }

        /// <summary>
        /// 订阅到;
        /// </summary>
        IDisposable Subscribe(T observer);

        /// <summary>
        /// 取消订阅;
        /// </summary>
        bool Unsubscribe(T observer);

        /// <summary>
        /// 迭代获取到观察者,并且在迭代过程中允许对删除合集元素,但是不允许嵌套;
        /// 若在迭代时加入新元素,则不会出现在本次迭代;
        /// </summary>
        IEnumerable<T> EnumerateObserver();

        /// <summary>
        /// 确认是否存在此元素;
        /// </summary>
        bool Contains(T item);

        /// <summary>
        /// 清除所有观察者;
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// 使用双向链表存储观察者,迭代时不需要临时空间;
    /// </summary>
    public class ObserverLinkedList<T> : IObserverCollection<T>
    {
        public ObserverLinkedList()
        {
            observersCollection = new LinkedList<T>();
            currentNode = null;
        }

        readonly LinkedList<T> observersCollection;
        LinkedListNode<T> currentNode;

        public IEnumerable<T> Observers
        {
            get { return observersCollection; }
        }

        public int Count
        {
            get { return observersCollection.Count; }
        }

        /// <summary>
        /// 订阅到,不检查是否存在重复项目;
        /// 若在遍历观察者时加入的,将不会出现在迭代内;
        /// </summary>
        public IDisposable Subscribe(T observer)
        {
            LinkedListNode<T> node;
            node = observersCollection.AddFirst(observer);
            return new Unsubscriber(this, node);
        }

        /// <summary>
        /// 取消订阅;
        /// </summary>
        public bool Unsubscribe(T observer)
        {
            return observersCollection.Remove(observer);
        }

        /// <summary>
        /// 迭代获取到观察者,并且在迭代过程中允许对删除合集元素,但是不允许嵌套;
        /// </summary>
        public IEnumerable<T> EnumerateObserver()
        {
            currentNode = observersCollection.First;
            while (currentNode != null)
            {
                var observer = currentNode.Value;
                currentNode = currentNode.Next;
                yield return observer;
            }
        }

        public bool Contains(T item)
        {
            return observersCollection.Contains(item);
        }

        public void Clear()
        {
            observersCollection.Clear();
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
                get { return parent.observersCollection; }
            }

            LinkedListNode<T> currentNode
            {
                get { return parent.currentNode; }
                set { parent.currentNode = value; }
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
                    if (node == currentNode)
                    {
                        currentNode = currentNode.Next;
                    }
                    observers.Remove(node);

                    parent = null;
                    node = null;
                    isDisposed = true;
                }
            }
        }
    }

    /// <summary>
    /// 允许重复加入的订阅合集;加入O(1),移除O(n);
    /// </summary>
    public class ObserverList<T> : ObserverCollection<T>
    {
        public ObserverList() : base(new List<T>())
        {
        }
    }

    /// <summary>
    /// 不允许重复订阅的合集;加入O(1),移除O(1);
    /// </summary>
    public class ObserverHashSet<T> : ObserverCollection<T>
    {
        public ObserverHashSet() : base(new HashSet<T>())
        {
        }
    }

    /// <summary>
    /// 使用定义的 ICollection<T> 接口存储观察者,迭代时需要临时空间;
    /// </summary>
    public class ObserverCollection<T> : IObserverCollection<T>
    {
        public ObserverCollection(ICollection<T> observersCollection)
        {
            this.observersCollection = observersCollection;
        }

        readonly ICollection<T> observersCollection;

        public int Count
        {
            get { return observersCollection.Count; }
        }

        public IEnumerable<T> Observers
        {
            get { return observersCollection; }
        }

        public IDisposable Subscribe(T observer)
        {
            observersCollection.Add(observer);
            return new Unsubscriber(observersCollection, observer);
        }

        public bool Unsubscribe(T observer)
        {
            return observersCollection.Remove(observer);
        }

        public IEnumerable<T> EnumerateObserver()
        {
            T[] observerArray = observersCollection.ToArray();
            return observerArray;
        }

        public bool Contains(T item)
        {
            return observersCollection.Contains(item);
        }

        public void Clear()
        {
            observersCollection.Clear();
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
}
