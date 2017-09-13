using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace KouXiaGu
{

    /// <summary>
    /// 可订阅的字典结构; 
    /// </summary>
    public interface IObservableDictionary<TKey, TValue>
    {
        IDisposable Subscribe(IDictionaryObserver<TKey, TValue> observer);
    }


    /// <summary>
    /// 字典结构订阅者; 
    /// </summary>
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
        void OnClear(IDictionary<TKey, TValue> dictionary);
    }


    /// <summary>
    /// 可订阅的字典结构; 
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
            TrackClear(this);
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

        void TrackClear(IDictionary<TKey, TValue> dictionary)
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                observer.OnClear(dictionary);
            }
        }
    }
}
