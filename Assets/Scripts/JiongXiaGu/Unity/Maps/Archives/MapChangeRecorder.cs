using JiongXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Maps
{

    public enum ChangeType
    {
        None,
        Add,
        Update,
        Remove,
    }

    /// <summary>
    /// 地图变化节点坐标记录;
    /// </summary>
    public class MapChangeRecorder<TKey> : IObserver<MapEvent<TKey>>
    {
        public Dictionary<TKey, ChangeType> ChangedDictionary { get; private set; }

        public MapChangeRecorder()
        {
            ChangedDictionary = new Dictionary<TKey, ChangeType>();
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
            ChangeType original;
            if (ChangedDictionary.TryGetValue(key, out original))
            {
                switch (original)
                {
                    case ChangeType.Remove:
                        ChangedDictionary[key] = ChangeType.Update;
                        break;

                    default:
                        throw new ArgumentException("错误类型:" + original);
                }
            }
            else
            {
                ChangedDictionary.Add(key, ChangeType.Add);
            }
        }

        private void OnRemove(TKey key)
        {
            ChangeType original;
            if (ChangedDictionary.TryGetValue(key, out original))
            {
                switch (original)
                {
                    case ChangeType.Add:
                        ChangedDictionary.Remove(key);
                        break;

                    case ChangeType.Update:
                        ChangedDictionary[key] = ChangeType.Remove;
                        break;

                    default:
                        throw new ArgumentException("错误类型:" + original);
                }
            }
            else
            {
                ChangedDictionary.Add(key, ChangeType.Remove);
            }
        }

        private void OnUpdate(TKey key)
        {
            ChangeType original;
            if (ChangedDictionary.TryGetValue(key, out original))
            {
                switch (original)
                {
                    case ChangeType.Add:
                        break;

                    case ChangeType.Update:
                        break;

                    default:
                        throw new ArgumentException("错误类型:" + original);
                }
            }
            else
            {
                ChangedDictionary.Add(key, ChangeType.Update);
            }
        }
    }
}
