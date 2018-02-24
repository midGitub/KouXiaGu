using JiongXiaGu.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace JiongXiaGu.Unity.RectMaps
{


    /// <summary>
    /// 游戏地图,线程安全的;
    /// </summary>
    public class Map<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IObservable<DictionaryEvent<TKey, TValue>>
    {
        private readonly ReaderWriterLockSlim asyncLock;
        private readonly IDictionary<TKey, TValue> dictionary;
        private readonly ObserverCollection<DictionaryEvent<TKey, TValue>> observers;

        public ICollection<TKey> Keys => dictionary.Keys;
        public ICollection<TValue> Values => dictionary.Values;
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => dictionary.Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => dictionary.Values;
        public int Count => dictionary.Count;
        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get
            {
                using (asyncLock.ReadLock())
                {
                    return dictionary[key];
                }
            }
            set
            {
                using (asyncLock.UpgradeableReadLock())
                {
                    TValue original;
                    if (dictionary.TryGetValue(key, out original))
                    {
                        using (asyncLock.WriteLock())
                        {
                            NotifyUpdate(key, original, value);
                            dictionary[key] = value;
                        }
                    }
                    else
                    {
                        throw new KeyNotFoundException();
                    }
                }
            }
        }

        public Map() : this(new Dictionary<TKey,TValue>())
        {
        }

        public Map(IDictionary<TKey, TValue> dictionary) : this(dictionary, new ObserverLinkedList<DictionaryEvent<TKey, TValue>>())
        {
        }

        public Map(IDictionary<TKey, TValue> dictionary, ObserverCollection<DictionaryEvent<TKey, TValue>> observers)
        {
            this.dictionary = dictionary;
            this.observers = observers;
        }

        public void Add(TKey key, TValue value)
        {
            using (asyncLock.UpgradeableReadLock())
            {
                if (dictionary.ContainsKey(key))
                {
                    using (asyncLock.WriteLock())
                    {
                        NotifyAdd(key, value);
                        dictionary.Add(key, value);
                    }
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public AddOrUpdateStatus AddOrUpdate(TKey key, TValue value)
        {
            using (asyncLock.UpgradeableReadLock())
            {
                TValue original;
                if (dictionary.TryGetValue(key, out original))
                {
                    using (asyncLock.WriteLock())
                    {
                        NotifyUpdate(key, original, value);
                        dictionary[key] = value;
                        return AddOrUpdateStatus.Updated;
                    }
                }
                else
                {
                    using (asyncLock.WriteLock())
                    {
                        NotifyAdd(key, value);
                        dictionary.Add(key, value);
                        return AddOrUpdateStatus.Added;
                    }
                }
            }
        }

        public bool Remove(TKey key)
        {
            using (asyncLock.UpgradeableReadLock())
            {
                TValue original;
                if (dictionary.TryGetValue(key, out original))
                {
                    using (asyncLock.WriteLock())
                    {
                        NotifyRemove(key, original);
                        dictionary.Remove(key);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool ContainsKey(TKey key)
        {
            using (asyncLock.ReadLock())
            {
                return dictionary.ContainsKey(key);
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ContainsKey(item.Key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            using (asyncLock.ReadLock())
            {
                return dictionary.TryGetValue(key, out value);
            }
        }

        public void Clear()
        {
            using (asyncLock.WriteLock())
            {
                NotifyClear();
                dictionary.Clear();
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            using (asyncLock.ReadLock())
            {
                (dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            using (asyncLock.ReadLock())
            {
                return dictionary.ToList().GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IDisposable Subscribe(IObserver<DictionaryEvent<TKey, TValue>> observer)
        {
            using (asyncLock.WriteLock())
            {
                return observers.Add(observer);
            }
        }

        public bool Unsubscribe(IObserver<DictionaryEvent<TKey, TValue>> observer)
        {
            using (asyncLock.WriteLock())
            {
                return observers.Remove(observer);
            }
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
                observer.OnNext(dictionaryEvent);
            }
        }
    }
}
