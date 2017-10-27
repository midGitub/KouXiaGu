using System;
using System.Collections;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using JiongXiaGu.Collections;

namespace JiongXiaGu
{

    public enum DictionaryEventType
    {
        Add,
        Remove,
        Update,
        Clear,
    }

    public struct DictionaryEvent<TKey, TValue>
    {
        public IReadOnlyDictionary<TKey, TValue> Dictionary { get; set; }
        public DictionaryEventType EventType { get; set; }
        public TKey Key { get; set; }
        public TValue OriginalValue { get; set; }
        public TValue NewValue { get; set; }
    }

    /// <summary>
    /// 可订阅的字典结构(线程安全); 
    /// </summary>
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IObservable<DictionaryEvent<TKey, TValue>>
    {
        private readonly object asyncLock = new object();
        private readonly IDictionary<TKey, TValue> dictionary;
        private readonly ObserverCollection<DictionaryEvent<TKey, TValue>> observers;

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) : this(dictionary, new ObserverLinkedList<DictionaryEvent<TKey, TValue>>())
        {
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, ObserverCollection<DictionaryEvent<TKey, TValue>> observers)
        {
            this.dictionary = dictionary;
            this.observers = observers;
        }

        public int Count
        {
            get { return dictionary.Count; }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get { return dictionary.Keys; }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get { return dictionary.Values; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
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
                    NotifyUpdate(key, original, value);
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        public IDisposable Subscribe(IObserver<DictionaryEvent<TKey, TValue>> observer)
        {
            return observers.Add(observer);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// 加入到合集内,若成功加入合集则通知所有订阅者;
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            lock (asyncLock)
            {
                NotifyAdd(key, value);
                dictionary.Add(key, value);
            }
        }

        /// <summary>
        /// 加入到合集,若已经存在则更新;
        /// </summary>
        public AddOrUpdateStatus AddOrUpdate(TKey key, TValue value)
        {
            lock (asyncLock)
            {
                TValue originalValue;
                if (dictionary.TryGetValue(key, out originalValue))
                {
                    NotifyUpdate(key, originalValue, value);
                    dictionary[key] = value;
                    return AddOrUpdateStatus.Updated;
                }
                else
                {
                    NotifyAdd(key, value);
                    dictionary.Add(key, value);
                    return AddOrUpdateStatus.Added;
                }
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool Remove(TKey key)
        {
            TValue original;
            if (dictionary.TryGetValue(key, out original))
            {
                dictionary.Remove(key);
                NotifyRemove(key, original);
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
            throw new InvalidOperationException("不支持清空操作!");
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

        /// <summary>
        /// 通知观察者合集即将进行加入操作;
        /// </summary>
        private void NotifyAdd(TKey key, TValue newValue)
        {
            foreach (var observer in observers)
            {
                DictionaryEvent<TKey, TValue> dictionaryEvent = new DictionaryEvent<TKey, TValue>()
                {
                    Dictionary = this,
                    EventType = DictionaryEventType.Add,
                    Key = key,
                    NewValue = newValue,
                };
                observer.OnNext(dictionaryEvent);
            }
        }

        /// <summary>
        /// 通知观察者合集即将进行加入操作;
        /// </summary>
        private void NotifyRemove(TKey key, TValue originalValue)
        {
            foreach (var observer in observers)
            {
                DictionaryEvent<TKey, TValue> dictionaryEvent = new DictionaryEvent<TKey, TValue>()
                {
                    Dictionary = this,
                    EventType = DictionaryEventType.Remove,
                    Key = key,
                    OriginalValue = originalValue,
                };
                observer.OnNext(dictionaryEvent);
            }
        }

        /// <summary>
        /// 通知观察者合集即将进行更新操作;
        /// </summary>
        private void NotifyUpdate(TKey key, TValue originalValue, TValue newValue)
        {
            foreach (var observer in observers)
            {
                DictionaryEvent<TKey, TValue> dictionaryEvent = new DictionaryEvent<TKey, TValue>()
                {
                    Dictionary = this,
                    EventType = DictionaryEventType.Update,
                    Key = key,
                    OriginalValue = originalValue,
                    NewValue = newValue,
                };
                observer.OnNext(dictionaryEvent);
            }
        }

        /// <summary>
        /// 通知观察者合集即将进行清空;
        /// </summary>
        private void NotifyClear()
        {
            foreach (var observer in observers)
            {
                DictionaryEvent<TKey, TValue> dictionaryEvent = new DictionaryEvent<TKey, TValue>()
                {
                    Dictionary = this,
                    EventType = DictionaryEventType.Clear,
                };
            }
        }
    }
}
