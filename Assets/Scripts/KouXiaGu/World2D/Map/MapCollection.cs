using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 地图结构;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MapCollection<T> : IHexMap<RectCoord, T>
    {

        public MapCollection()
        {
            mapCollection = new Dictionary<RectCoord, T>();
        }
        public MapCollection(Dictionary<RectCoord, T> mapCollection)
        {
            this.mapCollection = mapCollection;
        }


        Dictionary<RectCoord, T> mapCollection;


        public T this[RectCoord position]
        {
            get { return mapCollection[position]; }
            set { mapCollection[position] = value; }
        }

        public IEnumerable<T> Nodes
        {
            get { return mapCollection.Values; }
        }

        public IEnumerable<KeyValuePair<RectCoord, T>> NodePair
        {
            get { return mapCollection; }
        }

        public void Add(RectCoord position, T item)
        {
            mapCollection.Add(position, item);
        }

        public void Clear()
        {
            mapCollection.Clear();
        }

        public bool Contains(RectCoord position)
        {
            return mapCollection.ContainsKey(position);
        }

        public bool Remove(RectCoord position)
        {
            return mapCollection.Remove(position);
        }

        public bool TryGetValue(RectCoord position, out T item)
        {
            return mapCollection.TryGetValue(position, out item);
        }

    }

}
