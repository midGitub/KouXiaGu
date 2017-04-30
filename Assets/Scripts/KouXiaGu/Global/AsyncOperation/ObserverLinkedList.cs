using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public interface IObserverCollection<T>
    {
        int Count { get; }

        /// <summary>
        /// 不允许对合集进行更改的迭代结构;
        /// </summary>
        IEnumerable<T> Observers { get; }

        /// <summary>
        /// 订阅到,不检查是否存在重复项目;
        /// 若在遍历观察者时加入的,将不会出现在迭代内;
        /// </summary>
        IDisposable Subscribe(T observer);

        /// <summary>
        /// 迭代获取到观察者,并且在迭代过程中允许对删除合集元素,但是不允许嵌套;
        /// </summary>
        IEnumerable<T> EnumerateObserver();
    }

    public class ObserverLinkedList<T> : IObserverCollection<T>
    {
        public ObserverLinkedList()
        {
            observersList = new LinkedList<T>();
            currentNode = null;
        }

        readonly LinkedList<T> observersList;
        LinkedListNode<T> currentNode;

        public IEnumerable<T> Observers
        {
            get { return observersList; }
        }

        public int Count
        {
            get { return observersList.Count; }
        }

        /// <summary>
        /// 订阅到,不检查是否存在重复项目;
        /// 若在遍历观察者时加入的,将不会出现在迭代内;
        /// </summary>
        public IDisposable Subscribe(T observer)
        {
            LinkedListNode<T> node;
            if (currentNode == null)
            {
                node = observersList.AddLast(observer);
            }
            else
            {
                node = observersList.AddBefore(currentNode, observer);
            }
            return new Unsubscriber(this, node);
        }

        /// <summary>
        /// 迭代获取到观察者,并且在迭代过程中允许对删除合集元素,但是不允许嵌套;
        /// </summary>
        public IEnumerable<T> EnumerateObserver()
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
            public Unsubscriber(ObserverLinkedList<T> parent, LinkedListNode<T> node)
            {
                isUnsubscribed = false;
                this.parent = parent;
                this.node = node;
            }

            bool isUnsubscribed;
            ObserverLinkedList<T> parent;
            LinkedListNode<T> node;

            LinkedList<T> observers
            {
                get { return parent.observersList; }
            }

            LinkedListNode<T> currentNode
            {
                get { return parent.currentNode; }
                set { parent.currentNode = value; }
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

    public class ObserverCollection<T> : IObserverCollection<T>
    {
        public ObserverCollection()
        {
            this.observersCollection = new List<T>();
        }

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

        public IEnumerable<T> EnumerateObserver()
        {
            T[] observerArray = observersCollection.ToArray();
            return observerArray;
        }

        class Unsubscriber : IDisposable
        {
            public Unsubscriber(ICollection<T> observersCollection, T item)
            {
                isUnsubscribe = false;
                this.observersCollection = observersCollection;
                this.item = item;
            }

            bool isUnsubscribe;
            readonly ICollection<T> observersCollection;
            readonly T item;

            void IDisposable.Dispose()
            {
                if (!isUnsubscribe)
                {
                    isUnsubscribe = true;
                    observersCollection.Remove(item);
                }
            }
        }
    }

}
