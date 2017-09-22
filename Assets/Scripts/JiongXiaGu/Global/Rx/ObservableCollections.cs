using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using System.Collections.ObjectModel;

namespace JiongXiaGu
{

    public interface IObservableCollection<T>
    {
        IDisposable Subscribe(ICollectionObserver<T> observer);
    }

    public interface ICollectionObserver<T>
    {
        void OnAdded(T item);
        void OnRemoved(T item);
        void OnCleared();
    }

    /// <summary>
    /// 可观察加入移除操作合集;
    /// </summary>
    public class ObservableCollection<T> : ICollection<T>, IObservableCollection<T>
    {
        /// <summary>
        /// 保存合集引用;
        /// </summary>
        public ObservableCollection(ICollection<T> collection)
        {
            this.collection = collection;
            observers = new ObserverLinkedList<ICollectionObserver<T>>();
        }

        /// <summary>
        /// 保存合集引用;
        /// </summary>
        public ObservableCollection(ICollection<T> collection, IObserverCollection<ICollectionObserver<T>> observerCollection)
        {
            this.collection = collection;
            observers = observerCollection;
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
