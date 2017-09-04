using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using System.Collections.ObjectModel;

namespace KouXiaGu
{


    public interface IObservableDictionary<TKey, TValue>
    {
        IDisposable Subscribe(IDictionaryObserver<TKey, TValue> observer);
    }

    public interface IDictionaryObserver<TKey, TValue>
    {
        /// <summary>
        /// 在加入之后调用;
        /// </summary>
        void OnAdded(TKey key, TValue newValue);

        /// <summary>
        /// 在移除之后调用;
        /// </summary>
        void OnRemoved(TKey key, TValue originalValue);

        /// <summary>
        /// 在更新之后调用;
        /// </summary>
        void OnUpdated(TKey key, TValue originalValue, TValue newValue);

        /// <summary>
        /// 在清除之前调用;
        /// </summary>
        void OnClear();
    }

    /// <summary>
    /// 可观察的字典接口;
    /// </summary>
    public class ObservableDictionary<TKey, TValue> : IObservableDictionary<TKey, TValue>, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        /// <summary>
        /// 保存合集引用;
        /// </summary>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = dictionary;
            observers = new ObserverLinkedList<IDictionaryObserver<TKey, TValue>>();
        }

        /// <summary>
        /// 保存合集引用;
        /// </summary>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IObserverCollection<IDictionaryObserver<TKey, TValue>> observers)
        {
            this.dictionary = dictionary;
            this.observers = observers;
        }

        readonly IDictionary<TKey, TValue> dictionary;
        readonly IObserverCollection<IDictionaryObserver<TKey, TValue>> observers;

        public int Count
        {
            get { return dictionary.Count; }
        }

        public ICollection<TKey> Keys
        {
            get { return dictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return dictionary.Values; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        public IObserverCollection<IDictionaryObserver<TKey, TValue>> Observers
        {
            get { return observers; }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get { return dictionary.Keys; }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get { return dictionary.Values; }
        }

        public TValue this[TKey key]
        {
            get { return dictionary[key]; }
            set
            {
                TValue original;
                if (dictionary.TryGetValue(key, out original))
                {
                    dictionary[key] = value;
                    TrackUpdated(key, original, value);
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        public IDisposable Subscribe(IDictionaryObserver<TKey, TValue> observer)
        {
            return observers.Subscribe(observer);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// 加入到合集内,若成功加入合集则通知所有订阅者;
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
            TrackAdded(key, value);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool Remove(TKey key)
        {
            TValue original;
            if (dictionary.TryGetValue(key, out original))
            {
                dictionary.Remove(key);
                TrackRemoved(key, original);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return dictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// 移除所有内容,并且逐一通知到观察者;
        /// </summary>
        public void Clear()
        {
            TrackClear();
            dictionary.Clear();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            dictionary.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }


        void TrackAdded(TKey key, TValue newValue)
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                observer.OnAdded(key, newValue);
            }
        }

        void TrackRemoved(TKey key, TValue original)
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                observer.OnRemoved(key, original);
            }
        }

        void TrackUpdated(TKey key, TValue original, TValue newValue)
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                observer.OnUpdated(key, original, newValue);
            }
        }

        void TrackClear()
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                observer.OnClear();
            }
        }
    }



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
