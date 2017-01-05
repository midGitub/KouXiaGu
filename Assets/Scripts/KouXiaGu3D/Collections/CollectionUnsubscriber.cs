using System;
using System.Collections.Generic;

namespace KouXiaGu.Collections
{

    public class CollectionUnsubscriber<T> : IDisposable
    {
        public CollectionUnsubscriber(ICollection<T> observers, T observer)
        {
            if (observers == null)
                throw new ArgumentNullException();

            this.observers = observers;
            this.observer = observer;
        }

        ICollection<T> observers;
        T observer;

        public void Dispose()
        {
            if (observers != null)
            {
                observers.Remove(observer);
                observers = null;
                observer = default(T);
            }
        }
    }

}
