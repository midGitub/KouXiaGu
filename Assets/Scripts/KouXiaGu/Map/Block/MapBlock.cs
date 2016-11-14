using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 地图块;区分预制地图块和后期添加地图块的结构;
    /// </summary>
    public class MapBlock<T> : IMap<ShortVector2, T>
    {

        public MapBlock(Dictionary<ShortVector2, T> prefabMap, Dictionary<ShortVector2, T> archiveMap)
        {
            this.mapCollection = BlendMap(prefabMap, archiveMap);
        }

        internal Dictionary<ShortVector2, T> mapCollection;

        /// <summary>
        /// 当前地图;
        /// </summary>
        public Dictionary<ShortVector2, T> MapCollection
        {
            get { return mapCollection; }
        }

        public T this[ShortVector2 key]
        {
            get { return this.mapCollection[key]; }
            set { this.mapCollection[key] = value; }
        }

        public int Count
        {
            get { return this.mapCollection.Count; }
        }

        public bool IsEmpty
        {
            get { return mapCollection.Count == 0; }
        }

        public void Add(ShortVector2 position, T item)
        {
            this.mapCollection.Add(position, item);
        }

        public bool Remove(ShortVector2 position)
        {
            return this.mapCollection.Remove(position);
        }

        public bool ContainsPosition(ShortVector2 position)
        {
            return this.mapCollection.ContainsKey(position);
        }

        public bool TryGetValue(ShortVector2 position, out T item)
        {
            return this.mapCollection.TryGetValue(position, out item);
        }

        public void Clear()
        {
            mapCollection.Clear();
        }

        /// <summary>
        /// 混合两个词典,并且返回一个新的词典;
        /// </summary>
        private Dictionary<ShortVector2, T> BlendMap(
            Dictionary<ShortVector2, T> prefabMap, Dictionary<ShortVector2, T> archiveMap)
        {
            Dictionary<ShortVector2, T> mapCollection = new Dictionary<ShortVector2, T>(prefabMap);
            mapCollection.AddOrReplace(archiveMap);
            return mapCollection;
        }

        public override string ToString()
        {
            return base.ToString() +
                "\n元素个数:" + mapCollection.Count;
        }

    }

}
