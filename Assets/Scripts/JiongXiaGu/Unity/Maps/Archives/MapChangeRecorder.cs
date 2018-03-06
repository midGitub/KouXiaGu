using System;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.Maps
{

    /// <summary>
    /// 地图变化节点坐标记录;
    /// </summary>
    public class MapChangeRecorder<TKey> : IObserver<MapEvent<TKey>>
    {
        public Dictionary<TKey, MapChangeType> ChangedDictionary { get; private set; }

        public MapChangeRecorder()
        {
            ChangedDictionary = new Dictionary<TKey, MapChangeType>();
        }


        void IObserver<MapEvent<TKey>>.OnNext(MapEvent<TKey> value)
        {
            switch (value.EventType)
            {
                case DictionaryEventType.Add:
                    OnAdd(value.Key);
                    break;

                case DictionaryEventType.Remove:
                    OnRemove(value.Key);
                    break;

                case DictionaryEventType.Update:
                    OnUpdate(value.Key);
                    break;
            }
        }

        void IObserver<MapEvent<TKey>>.OnCompleted()
        {
        }

        void IObserver<MapEvent<TKey>>.OnError(Exception error)
        {
        }

        public void OnAdd(TKey key)
        {
            MapChangeType original;
            if (ChangedDictionary.TryGetValue(key, out original))
            {
                switch (original)
                {
                    case MapChangeType.Remove:
                        ChangedDictionary[key] = MapChangeType.Update;
                        break;

                    default:
                        throw new ArgumentException("错误类型:" + original);
                }
            }
            else
            {
                ChangedDictionary.Add(key, MapChangeType.Add);
            }
        }

        private void OnRemove(TKey key)
        {
            MapChangeType original;
            if (ChangedDictionary.TryGetValue(key, out original))
            {
                switch (original)
                {
                    case MapChangeType.Add:
                        ChangedDictionary.Remove(key);
                        break;

                    case MapChangeType.Update:
                        ChangedDictionary[key] = MapChangeType.Remove;
                        break;

                    default:
                        throw new ArgumentException("错误类型:" + original);
                }
            }
            else
            {
                ChangedDictionary.Add(key, MapChangeType.Remove);
            }
        }

        private void OnUpdate(TKey key)
        {
            MapChangeType original;
            if (ChangedDictionary.TryGetValue(key, out original))
            {
                switch (original)
                {
                    case MapChangeType.Add:
                        break;

                    case MapChangeType.Update:
                        break;

                    default:
                        throw new ArgumentException("错误类型:" + original);
                }
            }
            else
            {
                ChangedDictionary.Add(key, MapChangeType.Update);
            }
        }
    }
}
