using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace KouXiaGu
{

    public interface IObservableDictionary<TKey, TValue>
    {
        IDisposable Subscribe(IDictionaryObserver<TKey, TValue> observer);
    }

    public interface IDictionaryObserver<TKey, TValue>
    {
        void OnAdded(TKey key, TValue newValue);
        void OnRemoved(TKey key, TValue originalValue);
        void OnUpdated(TKey key, TValue originalValue, TValue newValue);
    }




    public interface IObservableCollection<T>
    {
        IDisposable Subscribe(ICollectionObserver<T> observer);
    }

    public interface ICollectionObserver<T>
    {
        void OnAdded(T item);
        void OnRemoved(T item);
        void OnCompleted();
    }

    public class ObservableCollection<T> : ICollection<T>, IObservableCollection<T>
    {
        public ObservableCollection(ICollection<T> collection, IObserverCollection<ICollectionObserver<T>> observerCollection)
        {
            this.collection = collection;
            this.observers = observerCollection;
        }

        readonly ICollection<T> collection;
        readonly IObserverCollection<ICollectionObserver<T>> observers;

        public ICollection<T> Collection
        {
            get { return collection; }
        }

        public IEnumerable<ICollectionObserver<T>> Observers
        {
            get { return observers.Observers; }
        }

        public int Count
        {
            get { return collection.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IDisposable Subscribe(ICollectionObserver<T> observer)
        {
            return observers.Subscribe(observer);
        }

        public void Add(T item)
        {
            collection.Add(item);
            TrackAdd(item);
        }

        void TrackAdd(T item)
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                observer.OnAdded(item);
            }
        }

        public bool Remove(T item)
        {
            if (collection.Remove(item))
            {
                TrackRemove(item);
                return true;
            }
            return false;
        }

        void TrackRemove(T item)
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                observer.OnRemoved(item);
            }
        }

        public bool Contains(T item)
        {
            return collection.Contains(item);
        }

        public void Clear()
        {
            foreach (var item in collection)
            {
                TrackRemove(item);
            }
            collection.Clear();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            collection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }
    }

}
