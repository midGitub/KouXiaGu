using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Collections
{


    public class LinkedListUnsubscriber<T> : IDisposable
    {
        public LinkedListUnsubscriber(LinkedList<T> observers, LinkedListNode<T> observer)
        {
            if (observers == null || observer == null)
                throw new ArgumentNullException();

            this.observers = observers;
            this.observer = observer;
        }

        LinkedList<T> observers;
        LinkedListNode<T> observer;

        public virtual void Dispose()
        {
            if (observers != null)
            {
                observers.Remove(observer);
                observers = null;
                observer = null;
            }
        }

    }

}
