using JiongXiaGu.Collections;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.Maps
{


    public class ArchiveMap<TKey> : SerializableDictionary<TKey, ArchiveMapNode>
    {
        public ArchiveMap(IReadOnlyDictionary<TKey, MapNode> map, IEnumerable<KeyValuePair<TKey, ChangeType>> change)
        {
            foreach (var item in change)
            {
                switch (item.Value)
                {
                    case ChangeType.Add:
                    case ChangeType.Update:
                        var value = map[item.Key];
                        Dictionary.Add(item.Key, new ArchiveMapNode(value));
                        break;

                    case ChangeType.Remove:
                        Dictionary.Add(item.Key, new ArchiveMapNode());
                        break;
                }
            }
        }
    }
}
