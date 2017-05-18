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
    public class ArchivedMapData : IDictionary<CubicHexCoord, MapNode>, IReadOnlyDictionary<CubicHexCoord, MapNode>
    {
        public ArchivedMapData(MapData data)
        {
            this.data = data;
            changedPositions = new HashSet<CubicHexCoord>();
        }

        readonly MapData data;
        readonly HashSet<CubicHexCoord> changedPositions;

        Dictionary<CubicHexCoord, MapNode> map
        {
            get { return data.Map; }
        }

        public MapData Data
        {
            get { return data; }
        }

        /// <summary>
        /// 发生变化的节点;
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
            set { map[key] = value; }
        }

        public void Add(CubicHexCoord key, MapNode value)
        {
            map.Add(key, value);
        }

        public void Add(KeyValuePair<CubicHexCoord, MapNode> item)
        {
            map.Add(item.Key, item.Value);
        }

        public bool Remove(CubicHexCoord key)
        {
            return map.Remove(key);
        }

        public bool Remove(KeyValuePair<CubicHexCoord, MapNode> item)
        {
            return map.Remove(item.Key);
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

        public void CopyTo(KeyValuePair<CubicHexCoord, MapNode>[] array, int arrayIndex)
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
    }

}
