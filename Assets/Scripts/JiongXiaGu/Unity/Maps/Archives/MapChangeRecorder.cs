using JiongXiaGu.Collections;
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

        /// <summary>
        /// 根据存档内容更新地图;
        /// </summary>
        public MapChangeRecorder(IDictionary<TKey, MapNode> map, IReadOnlyDictionary<TKey, ArchiveMapNode> archived)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));
            if (archived == null)
                throw new ArgumentNullException(nameof(archived));

            ChangedDictionary = new Dictionary<TKey, MapChangeType>();

            foreach (var item in archived)
            {
                if (item.Value.IsRemove)
                {
                    ChangedDictionary.Add(item.Key, MapChangeType.Remove);
                    map.Remove(item.Key);
                }
                else
                {
                    if (map.ContainsKey(item.Key))
                    {
                        map[item.Key] = item.Value.Node.Value;
                        ChangedDictionary.Add(item.Key, MapChangeType.Update);
                    }
                    else
                    {
                        map.Add(item.Key, item.Value.Node.Value);
                        ChangedDictionary.Add(item.Key, MapChangeType.Add);
                    }
                }
            }
        }

        /// <summary>
        /// 将存档信息加入到地图;
        /// </summary>
        public static void AddArchived(IDictionary<TKey, MapNode> map, IReadOnlyDictionary<TKey, ArchiveMapNode> archived)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));
            if (archived == null)
                throw new ArgumentNullException(nameof(archived));

            foreach (var item in archived)
            {
                if (item.Value.IsRemove)
                {
                    map.Remove(item.Key);
                }
                else
                {
                    if (map.ContainsKey(item.Key))
                    {
                        map[item.Key] = item.Value.Node.Value;
                    }
                    else
                    {
                        map.Add(item.Key, item.Value.Node.Value);
                    }
                }
            }
        }

        /// <summary>
        /// 获取到用于存档的合集,若无变化则返回null;
        /// </summary>
        public Dictionary<TKey, ArchiveMapNode> GetArchiveMap(IReadOnlyDictionary<TKey, MapNode> map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            Dictionary<TKey, ArchiveMapNode> dictionary = new Dictionary<TKey, ArchiveMapNode>();

            foreach (var item in ChangedDictionary)
            {
                switch (item.Value)
                {
                    case MapChangeType.Add:
                    case MapChangeType.Update:
                        var value = map[item.Key];
                        dictionary.Add(item.Key, new ArchiveMapNode(value));
                        break;

                    case MapChangeType.Remove:
                        dictionary.Add(item.Key, new ArchiveMapNode());
                        break;
                }
            }

            if (dictionary.Count > 0)
            {
                return dictionary;
            }
            else
            {
                return null;
            }
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
