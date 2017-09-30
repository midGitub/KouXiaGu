using System;
using System.Collections.Generic;

namespace JiongXiaGu
{

    /// <summary>
    /// 对观察者手动传送消息的字典结构;
    /// </summary>
    public class ObservableAsyncDictionary<TKey, TValue> : ObservableDictionary<TKey, TValue>
    {
        readonly DictionaryOperationCache dictionaryOperationCache;

        public ObservableAsyncDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
            dictionaryOperationCache = new DictionaryOperationCache();
        }

        public ObservableAsyncDictionary(IDictionary<TKey, TValue> dictionary, IObserverCollection<IDictionaryObserver<TKey, TValue>> observers) : base(dictionary, observers)
        {
            dictionaryOperationCache = new DictionaryOperationCache();
        }

        protected override void OnAdded(TKey key, TValue newValue)
        {
            dictionaryOperationCache.Add(key, newValue);
        }

        protected override void OnRemoved(TKey key, TValue original)
        {
            dictionaryOperationCache.Remove(key, original);
        }

        protected override void OnUpdated(TKey key, TValue original, TValue newValue)
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
