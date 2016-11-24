using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 地图结构;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MapCollection<T> : IMap<ShortVector2, T>
    {

        public MapCollection()
        {
            mapCollection = new Dictionary<ShortVector2, T>();
        }
        public MapCollection(Dictionary<ShortVector2, T> mapCollection)
        {
            this.mapCollection = mapCollection;
        }


        Dictionary<ShortVector2, T> mapCollection;


        public T this[ShortVector2 position]
        {
            get { return mapCollection[position]; }
            set { mapCollection[position] = value; }
        }

        public IEnumerable<T> Nodes
        {
            get { return mapCollection.Values; }
        }

        public IEnumerable<KeyValuePair<ShortVector2, T>> NodePair
        {
            get { return mapCollection; }
        }

        public void Add(ShortVector2 position, T item)
        {
            mapCollection.Add(position, item);
        }

        public void Clear()
        {
            mapCollection.Clear();
        }

        public bool Contains(ShortVector2 position)
        {
            return mapCollection.ContainsKey(position);
        }

        public bool Remove(ShortVector2 position)
        {
            return mapCollection.Remove(position);
        }

        public bool TryGetValue(ShortVector2 position, out T item)
        {
            return mapCollection.TryGetValue(position, out item);
        }

    }

}
