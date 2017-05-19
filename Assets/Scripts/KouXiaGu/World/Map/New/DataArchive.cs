using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 记录地图变化;
    /// </summary>
    public class DataArchive : IDictionary<CubicHexCoord, MapNode>, IReadOnlyDictionary<CubicHexCoord, MapNode>
    {
        public DataArchive(MapData data)
        {
            this.data = data;
            changedPositions = new HashSet<CubicHexCoord>();
        }

        readonly MapData data;
        readonly HashSet<CubicHexCoord> changedPositions;
        readonly IObserverCollection<IDictionaryObserver<CubicHexCoord, MapNode>> observers;

        IDictionary<CubicHexCoord, MapNode> map
        {
            get { return data.Map; }
        }

        public MapData Data
        {
            get { return data; }
        }

        /// <summary>
        /// 发生变化的节点,不包括被移除的节点;
        /// </summary>
        public ICollection<CubicHexCoord> ChangedPositions
        {
            get { return changedPositions; }
        }

        public ICollection<CubicHexCoord> Keys
        {
            get { return map.Keys; }
        }

        public ICollection<MapNode> Values
        {
            get { return map.Values; }
        }

        IEnumerable<CubicHexCoord> IReadOnlyDictionary<CubicHexCoord, MapNode>.Keys
        {
            get { return map.Keys; }
        }

        IEnumerable<MapNode> IReadOnlyDictionary<CubicHexCoord, MapNode>.Values
        {
            get { return map.Values; }
        }

        public int Count
        {
            get { return map.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public MapNode this[CubicHexCoord key]
        {
            get { return map[key]; }
            set
            {
                map[key] = value;
                changedPositions.Add(key);
            }
        }

        public void Add(KeyValuePair<CubicHexCoord, MapNode> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(CubicHexCoord key, MapNode value)
        {
            map.Add(key, value);
            changedPositions.Add(key);
        }

        public bool Remove(KeyValuePair<CubicHexCoord, MapNode> item)
        {
            return Remove(item.Key);
        }

        public bool Remove(CubicHexCoord key)
        {
            bool isRemoved = map.Remove(key);
            changedPositions.Remove(key);
            return isRemoved;
        }

        public bool ContainsKey(CubicHexCoord key)
        {
            return map.ContainsKey(key);
        }

        public bool Contains(KeyValuePair<CubicHexCoord, MapNode> item)
        {
            return map.ContainsKey(item.Key);
        }

        public bool TryGetValue(CubicHexCoord key, out MapNode value)
        {
            return map.TryGetValue(key, out value);
        }

        public void Clear()
        {
            map.Clear();
        }

        void ICollection<KeyValuePair<CubicHexCoord, MapNode>>.CopyTo(KeyValuePair<CubicHexCoord, MapNode>[] array, int arrayIndex)
        {
            (map as ICollection<KeyValuePair<CubicHexCoord, MapNode>>).CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<CubicHexCoord, MapNode>> GetEnumerator()
        {
            return map.GetEnumerator();
        }

        /// <summary>
        /// 获取变化的到用于归档的数据;
        /// </summary>
        public MapData GetArchivedData()
        {
            MapData archivedData = new MapData()
            {
                Map = GetChangedData(),
            };
            return archivedData;
        }

        /// <summary>
        /// 获取到发生变化的节点结构;
        /// </summary>
        public Dictionary<CubicHexCoord, MapNode> GetChangedData()
        {
            Dictionary<CubicHexCoord, MapNode> changedData = new Dictionary<CubicHexCoord, MapNode>();
            foreach (var position in changedPositions)
            {
                MapNode node = map[position];
                changedData.Add(position, node);
            }
            return changedData;
        }
    }
}
