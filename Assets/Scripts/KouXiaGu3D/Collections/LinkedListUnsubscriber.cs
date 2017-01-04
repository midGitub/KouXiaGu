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
            this.observers = observers;
            this.observer = observer;
        }

        LinkedList<T> observers;
        LinkedListNode<T> observer;

        public void Dispose()
        {
            observers.Remove(observer);
        }

    }

}
