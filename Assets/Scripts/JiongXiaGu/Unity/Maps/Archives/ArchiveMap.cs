using JiongXiaGu.Collections;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.Maps
{

    /// <summary>
    /// 存档地图;
    /// </summary>
    public class ArchiveMap<TKey> : SerializableDictionary<TKey, ArchiveMapNode>
    {
        public ArchiveMap(IReadOnlyDictionary<TKey, MapNode> map, IEnumerable<KeyValuePair<TKey, MapNodeChangeType>> change)
        {
            foreach (var item in change)
            {
                switch (item.Value)
                {
                    case MapNodeChangeType.Add:
                    case MapNodeChangeType.Update:
                        var value = map[item.Key];
                        Dictionary.Add(item.Key, new ArchiveMapNode(value));
                        break;

                    case MapNodeChangeType.Remove:
                        Dictionary.Add(item.Key, new ArchiveMapNode());
                        break;
                }
            }
        }

        /// <summary>
        /// 更新地图内容;
        /// </summary>
        public void Update(IDictionary<TKey, MapNode> map)
        {
            foreach (var item in Dictionary)
            {
                if (item.Value.IsRemove)
                {
                    map.Remove(item.Key);
                }
                else
                {
                    map.AddOrUpdate(item.Key, item.Value.Node.Value);
                }
            }
        }
    }
}
