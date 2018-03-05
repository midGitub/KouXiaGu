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

    /// <summary>
    /// 地图变化节点坐标记录;
    /// </summary>
    public class MapChangeRecorder<TKey> : IObserver<MapEvent<TKey>>
    {
        public Dictionary<TKey, MapNodeChangeType> ChangedDictionary { get; private set; }

        public MapChangeRecorder()
        {
            ChangedDictionary = new Dictionary<TKey, MapNodeChangeType>();
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
            MapNodeChangeType original;
            if (ChangedDictionary.TryGetValue(key, out original))
            {
                switch (original)
                {
                    case MapNodeChangeType.Remove:
                        ChangedDictionary[key] = MapNodeChangeType.Update;
                        break;

                    default:
                        throw new ArgumentException("错误类型:" + original);
                }
            }
            else
            {
                ChangedDictionary.Add(key, MapNodeChangeType.Add);
            }
        }

        private void OnRemove(TKey key)
        {
            MapNodeChangeType original;
            if (ChangedDictionary.TryGetValue(key, out original))
            {
                switch (original)
                {
                    case MapNodeChangeType.Add:
                        ChangedDictionary.Remove(key);
                        break;

                    case MapNodeChangeType.Update:
                        ChangedDictionary[key] = MapNodeChangeType.Remove;
                        break;

                    default:
                        throw new ArgumentException("错误类型:" + original);
                }
            }
            else
            {
                ChangedDictionary.Add(key, MapNodeChangeType.Remove);
            }
        }

        private void OnUpdate(TKey key)
        {
            MapNodeChangeType original;
            if (ChangedDictionary.TryGetValue(key, out original))
            {
                switch (original)
                {
                    case MapNodeChangeType.Add:
                        break;

                    case MapNodeChangeType.Update:
                        break;

                    default:
                        throw new ArgumentException("错误类型:" + original);
                }
            }
            else
            {
                ChangedDictionary.Add(key, MapNodeChangeType.Update);
            }
        }
    }
}
