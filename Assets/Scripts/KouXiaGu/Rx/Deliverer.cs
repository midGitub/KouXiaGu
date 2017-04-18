using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Rx
{

    /// <summary>
    /// 对多个对象进行操作,可在操作过程中移除;
    /// </summary>
    public abstract class DelivererBase<T>
    {
        public DelivererBase()
        {
            currentNode = null;
            observers = new LinkedList<T>();
        }

        LinkedList<T> observers;
        LinkedListNode<T> currentNode;

        public int observerCount
        {
            get { return observers.Count; }
        }

        public IDisposable Subscribe(T observer)
        {
            var node = observers.AddLast(observer);
            return new Unsubscriber(this, node);
        }

        /// <summary>
        /// 迭代获取到观察者;
        /// </summary>
        protected IEnumerable<T> EnumerateObserver()
        {
            currentNode = observers.First;

            while (currentNode != null)
            {
                var observer = currentNode.Value;
                currentNode = currentNode.Next;
                yield return observer;
            }
        }

        class Unsubscriber : IDisposable
        {
            public Unsubscriber(DelivererBase<T> tracker, LinkedListNode<T> node)
            {
                isUnsubscribed = false;
                this.tracker = tracker;
                this.node = node;
            }

            bool isUnsubscribed;
            DelivererBase<T> tracker;
            LinkedListNode<T> node;

            LinkedList<T> observers
            {
                get { return tracker.observers; }
            }

            LinkedListNode<T> currentNode
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

    public abstract class Deliverer<T> : DelivererBase<T>
    {
        public void Track()
        {
            foreach (var observer in EnumerateObserver())
            {
                Operate(observer);
            }
        }

        protected abstract void Operate(T observer);
    }

    public abstract class Deliverer<T, TParameter> : DelivererBase<T>
    {
        public void Track(TParameter parameter)
        {
            foreach (var observer in EnumerateObserver())
            {
                Operate(observer, parameter);
            }
        }

        protected abstract void Operate(T observer, TParameter parameter);
    }

}
