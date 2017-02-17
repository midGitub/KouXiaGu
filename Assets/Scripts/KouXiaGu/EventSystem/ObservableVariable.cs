using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.EventSystem
{

    /// <summary>
    /// 可观察的变量;
    /// </summary>
    public abstract class ObservableVariable : IObservable, IEnumerable<IObserver>
    {

        protected ObservableVariable()
        {
        }


        /// <summary>
        /// 观察者链表;
        /// </summary>
        LinkedList<IObserver> observers = new LinkedList<IObserver>();

        /// <summary>
        /// 正在进行通知的观察者,若不存在则为null;
        /// </summary>
        LinkedListNode<IObserver> current;


        /// <summary>
        /// 订阅者总数;
        /// </summary>
        public int ObserverCount
        {
            get { return observers.Count; }
        }


        /// <summary>
        /// 通知所有观察者,数值发生变化;
        /// </summary>
        protected void OnValueChanged()
        {
            current = observers.First;

            while (current != null)
            {
                var tempCurrent = current;
                current.Value.OnNext(this);

                if (current == tempCurrent)
                {
                    current = current.Next;
                }
            }
        }


        public IEnumerator<IObserver> GetEnumerator()
        {
            return ((IEnumerable<IObserver>)this.observers).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IObserver>)this.observers).GetEnumerator();
        }


        /// <summary>
        /// 订阅事件,不检查是否重复订阅观察;
        /// </summary>
        public IDisposable Subscribe(IObserver observer)
        {
            LinkedListNode<IObserver> node = observers.AddLast(observer);
            return new Unsubscriber(this, node);
        }

        /// <summary>
        /// 退订方法;
        /// </summary>
        class Unsubscriber : IDisposable
        {
            public Unsubscriber(ObservableVariable provider, LinkedListNode<IObserver> node)
            {
                if (provider == null || node == null)
                    throw new ArgumentNullException();

                this.provider = provider;
                this.node = node;
            }

            ObservableVariable provider;
            LinkedListNode<IObserver> node;

            public void Dispose()
            {
                if (node != null)
                {
                    if (provider.current == node)
                    {
                        provider.current = provider.current.Next;
                    }

                    provider.observers.Remove(node);
                    provider = null;
                    node = null;
                }
            }

            public override bool Equals(object obj)
            {
                var t = obj as Unsubscriber;

                if (t == null)
                    return false;

                return 
                    t.node == node &&
                    t.provider == provider;
            }

            public override int GetHashCode()
            {
                return node.GetHashCode() ^ provider.GetHashCode();
            }

        }

    }

}
