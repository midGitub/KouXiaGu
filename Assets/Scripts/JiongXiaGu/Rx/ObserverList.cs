using System;
using System.Collections.Generic;
using JiongXiaGu.Collections;

namespace JiongXiaGu
{
    /// <summary>
    /// 观察者合集;加入O(n),移除O(n), 迭代O(n);
    /// </summary>
    public class ObserverList<T> : ObserverCollection<T>
    {
        private List<IObserver<T>> observers;
        public IEqualityComparer<IObserver<T>> Comparer { get; private set; }

        public ObserverList()
        {
            observers = new List<IObserver<T>>();
            Comparer = EqualityComparer<IObserver<T>>.Default;
        }

        public override IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (observers.Contains(observer))
                throw RepeatedObserverException(observer);

            observers.Add(observer);
            var unsubscriber = new Unsubscriber(this, observer);
            return unsubscriber;
        }

        protected override IEnumerable<IObserver<T>> EnumerateObserver()
        {
            return observers;
        }

        /// <summary>
        /// 提供移除观察者方法;
        /// </summary>
        private class Unsubscriber : IDisposable
        {
            private ObserverList<T> observerCollection;
            private IObserver<T> observer;
            private List<IObserver<T>> observers => observerCollection.observers;
            private IEqualityComparer<IObserver<T>> comparer => observerCollection.Comparer;

            public Unsubscriber(ObserverList<T> observerCollection, IObserver<T> observer)
            {
                this.observerCollection = observerCollection;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (observerCollection != null)
                {
                    observers.Remove(item => comparer.Equals(item, observer));

                    observerCollection = null;
                    observer = null;
                }
            }
        }
    }
}
