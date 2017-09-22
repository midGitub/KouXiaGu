using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu
{

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
                finally
                {
                    Observers = null;
                    Observer = null;
                }
            }
        }
    }
}
