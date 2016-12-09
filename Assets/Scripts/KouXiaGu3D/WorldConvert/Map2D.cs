using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏地图结构;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ProtoContract]
    public class Map2D<TP, T> : IMap2D<TP, T>, IReadOnlyMap2D<TP, T>
    {

        /// <summary>
        /// 地图结构;
        /// </summary>
        [ProtoMember(1)]
        Dictionary<TP, T> mapCollection;

        public IEnumerable<TP> Points
        {
            get
            {
                return mapCollection.Keys;
            }
        }

        public IEnumerable<T> Nodes
        {
            get
            {
                return mapCollection.Values;
            }
        }

        public int Count
        {
            get
            {
                return mapCollection.Count;
            }
        }

        public T this[TP position]
        {
            get
            {
                return mapCollection[position];
            }
            set
            {
                mapCollection[position] = value;
            }
        }

        public Map2D()
        {
            mapCollection = new Dictionary<TP, T>();
        }

        public void Add(TP position, T item)
        {
            mapCollection.Add(position, item);
        }

        public void Add(KeyValuePair<TP, T> pair)
        {
            mapCollection.Add(pair.Key, pair.Value);
        }

        public bool Remove(TP position)
        {
            return mapCollection.Remove(position);
        }

        public bool Contains(TP position)
        {
            return mapCollection.ContainsKey(position);
        }

        public bool TryGetValue(TP position, out T item)
        {
            return mapCollection.TryGetValue(position, out item);
        }

        public void Clear()
        {
            mapCollection.Clear();
        }

        public IEnumerator<KeyValuePair<TP, T>> GetEnumerator()
        {
            return mapCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
