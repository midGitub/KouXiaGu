using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu3D.Map
{

    /// <summary>
    /// 对更改的地图进行存档;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Map2D<T> : IMap2D<T>, IReadOnlyMap2D<T>
        where T : struct
    {
        /// <summary>
        /// 地图结构;
        /// </summary>
        Dictionary<ShortVector2, T> mapCollection;

        public IEnumerable<ShortVector2> Points
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

        public T this[ShortVector2 position]
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
            mapCollection = new Dictionary<ShortVector2, T>();
        }

        public void Add(ShortVector2 position, T item)
        {
            mapCollection.Add(position, item);
        }

        public bool Remove(ShortVector2 position)
        {
            return mapCollection.Remove(position);
        }

        public bool Contains(ShortVector2 position)
        {
            return mapCollection.ContainsKey(position);
        }

        public bool TryGetValue(ShortVector2 position, out T item)
        {
            return mapCollection.TryGetValue(position, out item);
        }

        public void Clear()
        {
            mapCollection.Clear();
        }

        public IEnumerator<KeyValuePair<ShortVector2, T>> GetEnumerator()
        {
            return mapCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
