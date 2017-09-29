using System;
using System.Collections;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace JiongXiaGu
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
    }


    /// <summary>
    /// 可订阅的字典结构(线程安全); 
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
            dictionaryOperationCache = new DictionaryOperationCache();
        }

        /// <summary>
        /// 保存合集引用;
        /// </summary>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IObserverCollection<IDictionaryObserver<TKey, TValue>> observers)
        {
            this.dictionary = dictionary;
            this.observers = observers;
            dictionaryOperationCache = new DictionaryOperationCache();
        }

        readonly IDictionary<TKey, TValue> dictionary;
        readonly IObserverCollection<IDictionaryObserver<TKey, TValue>> observers;
        readonly DictionaryOperationCache dictionaryOperationCache;

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
                    OnUpdated(key, original, value);
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
            OnAdded(key, value);
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
                OnRemoved(key, original);
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


        void OnAdded(TKey key, TValue newValue)
        {
            dictionaryOperationCache.Add(key, newValue);
        }

        void OnRemoved(TKey key, TValue original)
        {
            dictionaryOperationCache.Remove(key, original);
        }

        void OnUpdated(TKey key, TValue original, TValue newValue)
        {
            dictionaryOperationCache.Update(key, original, newValue);
        }

        /// <summary>
        /// 提供外部调用,通知所有观察者合集的变化;
        /// </summary>
        public void TrackAll()
        {
            foreach (var operation in dictionaryOperationCache.DictionaryOperations)
            {
                Action<IDictionaryObserver<TKey, TValue>> trackAction;
                switch (operation.OperationType)
                {
                    case OperationTypes.Add:
                        trackAction = observer => observer.OnAdded(operation.Key, operation.NewValue);
                        break;

                    case OperationTypes.Remove:
                        trackAction = observer => observer.OnRemoved(operation.Key, operation.OriginalValue);
                        break;

                    case OperationTypes.Update:
                        trackAction = observer => observer.OnUpdated(operation.Key, operation.OriginalValue, operation.NewValue);
                        break;

                    default:
                        throw new ArgumentException(operation.OperationType.ToString());
                }

                foreach (var observer in observers.EnumerateObserver())
                {
                    trackAction(observer);
                }
            }
        }

        /// <summary>
        /// 字典操作缓存;
        /// </summary>
        class DictionaryOperationCache
        {
            public List<DictionaryOperation> DictionaryOperations { get; private set; }

            public DictionaryOperationCache()
            {
                DictionaryOperations = new List<DictionaryOperation>();
            }

            /// <summary>
            /// 添加一个加入操作;
            /// </summary>
            public void Add(TKey key, TValue newValue)
            {
                int index = FindIndex(key);
                if (index < 0)
                {
                    var operation = new DictionaryOperation()
                    {
                        OperationType = OperationTypes.Add,
                        Key = key,
                        NewValue = newValue,
                    };
                    DictionaryOperations.Add(operation);
                }
                else
                {
                    var operation = DictionaryOperations[index];
                    switch (operation.OperationType)
                    {
                        case OperationTypes.Remove:
                            operation.OperationType = OperationTypes.Update;
                            operation.NewValue = newValue;
                            DictionaryOperations[index] = operation;
                            break;

                        default:
                            throw new InvalidOperationException();
                    }
                }
            }

            /// <summary>
            /// 添加一个移除操作;
            /// </summary>
            public void Remove(TKey key, TValue original)
            {
                int index = FindIndex(key);
                if (index < 0)
                {
                    var operation = new DictionaryOperation()
                    {
                        OperationType = OperationTypes.Remove,
                        Key = key,
                        OriginalValue = original,
                    };
                    DictionaryOperations.Add(operation);
                }
                else
                {
                    var operation = DictionaryOperations[index];
                    switch (operation.OperationType)
                    {
                        case OperationTypes.Add:
                            DictionaryOperations.RemoveAt(index);
                            break;

                        case OperationTypes.Update:
                            operation.OperationType = OperationTypes.Remove;
                            operation.NewValue = default(TValue);
                            DictionaryOperations[index] = operation;
                            break;

                        default:
                            throw new InvalidOperationException();
                    }
                }
            }

            /// <summary>
            /// 添加一个升级操作;
            /// </summary>
            public void Update(TKey key, TValue original, TValue newValue)
            {
                int index = FindIndex(key);
                if (index < 0)
                {
                    var operation = new DictionaryOperation()
                    {
                        OperationType = OperationTypes.Update,
                        Key = key,
                        OriginalValue = original,
                        NewValue = newValue,
                    };
                    DictionaryOperations.Add(operation);
                }
                else
                {
                    var operation = DictionaryOperations[index];
                    switch (operation.OperationType)
                    {
                        case OperationTypes.Update:
                            operation.NewValue = newValue;
                            DictionaryOperations[index] = operation;
                            break;

                        case OperationTypes.Add:
                            operation.NewValue = newValue;
                            DictionaryOperations[index] = operation;
                            break;

                        default:
                            throw new InvalidOperationException();
                    }
                }
            }

            int FindIndex(TKey key)
            {
                return DictionaryOperations.FindIndex(item => item.Key.Equals(key));
            }
        }

        struct DictionaryOperation
        {
            public OperationTypes OperationType { get; set; }
            public TKey Key { get; set; }
            public TValue OriginalValue { get; set; }
            public TValue NewValue { get; set; }
        }

        enum OperationTypes
        {
            None,
            Add,
            Remove,
            Update,
        }
    }
}
