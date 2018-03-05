using JiongXiaGu.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JiongXiaGu.Unity.Maps
{

    /// <summary>
    /// 地图;
    /// </summary>
    public sealed class Map<TKey> : IMap<TKey>, IDictionary<TKey, MapNode>, IReadOnlyDictionary<TKey, MapNode>, IObservable<MapEvent<TKey>>
    {
        private readonly IDictionary<TKey, MapNode> dictionary;
        private readonly ObserverCollection<MapEvent<TKey>> observers;

        public int Count => dictionary.Count;
        public ICollection<TKey> Keys => dictionary.Keys;
        public ICollection<MapNode> Values => dictionary.Values;
        IEnumerable<TKey> IReadOnlyDictionary<TKey, MapNode>.Keys => dictionary.Keys;
        IEnumerable<MapNode> IReadOnlyDictionary<TKey, MapNode>.Values => dictionary.Values;
        bool ICollection<KeyValuePair<TKey, MapNode>>.IsReadOnly => false;

        public MapNode this[TKey key]
        {
            get
            {
                return dictionary[key];
            }
            set
            {
                MapNode original;
                if (dictionary.TryGetValue(key, out original))
                {
                    NotifyUpdate(key, original, value);
                    dictionary[key] = value;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        public Map() : this(new Dictionary<TKey,MapNode>())
        {
        }

        public Map(IDictionary<TKey, MapNode> dictionary) : this(dictionary, new ObserverLinkedList<MapEvent<TKey>>())
        {
        }

        public Map(ObserverCollection<MapEvent<TKey>> observers) : this(new Dictionary<TKey, MapNode>(), observers)
        {
        }

        public Map(IDictionary<TKey, MapNode> dictionary, ObserverCollection<MapEvent<TKey>> observers)
        {
            this.dictionary = dictionary;
            this.observers = observers;
        }

        public void Add(TKey key, MapNode value)
        {
            if (dictionary.ContainsKey(key))
            {
                NotifyAdd(key, value);
                dictionary.Add(key, value);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void Add(KeyValuePair<TKey, MapNode> item)
        {
            Add(item.Key, item.Value);
        }

        public AddOrUpdateStatus AddOrUpdate(TKey key, MapNode value)
        {
            MapNode original;
            if (dictionary.TryGetValue(key, out original))
            {
                NotifyUpdate(key, original, value);
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

        public bool Remove(TKey key)
        {
            MapNode original;
            if (dictionary.TryGetValue(key, out original))
            {
                NotifyRemove(key, original);
                dictionary.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Remove(KeyValuePair<TKey, MapNode> item)
        {
            return Remove(item.Key);
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool Contains(KeyValuePair<TKey, MapNode> item)
        {
            return ContainsKey(item.Key);
        }

        public bool TryGetValue(TKey key, out MapNode value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public void Clear()
        {
            NotifyClear();
            dictionary.Clear();
        }

        void ICollection<KeyValuePair<TKey, MapNode>>.CopyTo(KeyValuePair<TKey, MapNode>[] array, int arrayIndex)
        {
            (dictionary as ICollection<KeyValuePair<TKey, MapNode>>).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, MapNode>> GetEnumerator()
        {
            return dictionary.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IDisposable Subscribe(IObserver<MapEvent<TKey>> observer)
        {
            return observers.Add(observer);
        }

        public bool Unsubscribe(IObserver<MapEvent<TKey>> observer)
        {
            return observers.Remove(observer);
        }

        /// <summary>
        /// 通知观察者合集即将进行加入操作;
        /// </summary>
        private void NotifyAdd(TKey key, MapNode newValue)
        {
            MapEvent<TKey> mapEvent = new MapEvent<TKey>()
            {
                Map = this,
                EventType = DictionaryEventType.Add,
                Key = key,
                NewValue = newValue,
            };

            observers.NotifyNext(mapEvent);
        }

        /// <summary>
        /// 通知观察者合集即将进行加入操作;
        /// </summary>
        private void NotifyRemove(TKey key, MapNode originalValue)
        {
            MapEvent<TKey> mapEvent = new MapEvent<TKey>()
            {
                Map = this,
                EventType = DictionaryEventType.Remove,
                Key = key,
                OriginalValue = originalValue,
            };

            observers.NotifyNext(mapEvent);
        }

        /// <summary>
        /// 通知观察者合集即将进行更新操作;
        /// </summary>
        private void NotifyUpdate(TKey key, MapNode originalValue, MapNode newValue)
        {
            var changeContents = MapNode.GetNodeChangeElements(originalValue, newValue);
            if (changeContents > 0)
            {
                MapEvent<TKey> mapEvent = new MapEvent<TKey>()
                {
                    Map = this,
                    EventType = DictionaryEventType.Update,
                    Key = key,
                    OriginalValue = originalValue,
                    NewValue = newValue,
                    ChangeContents = changeContents,
                };

                observers.NotifyNext(mapEvent);
            }
        }

        /// <summary>
        /// 通知观察者合集即将进行清空;
        /// </summary>
        private void NotifyClear()
        {
            MapEvent<TKey> mapEvent = new MapEvent<TKey>()
            {
                Map = this,
                EventType = DictionaryEventType.Clear,
            };

            observers.NotifyNext(mapEvent);
        }
    }
}
