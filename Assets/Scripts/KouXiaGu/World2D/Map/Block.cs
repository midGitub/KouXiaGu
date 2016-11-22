using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 仅一个地图块结构;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Block<T> : IMap<ShortVector2, T>
        where T : struct
    {

        public Block()
        {
            mapCollection = new Dictionary<ShortVector2, T>();
        }

        Dictionary<ShortVector2, T> mapCollection;

        T IMap<ShortVector2, T>.this[ShortVector2 position]
        {
            get { return mapCollection[position]; }
            set { mapCollection[position] = value; }
        }

        IEnumerable<KeyValuePair<ShortVector2, T>> IMap<ShortVector2, T>.NodePair
        {
            get { return mapCollection; }
        }

        void IMap<ShortVector2, T>.Add(ShortVector2 position, T item)
        {
            mapCollection.Add(position, item);
        }

        void IMap<ShortVector2, T>.Clear()
        {
            mapCollection.Clear();
        }

        bool IMap<ShortVector2, T>.Contains(ShortVector2 position)
        {
            return mapCollection.ContainsKey(position);
        }

        bool IMap<ShortVector2, T>.Remove(ShortVector2 position)
        {
            return mapCollection.Remove(position);
        }

        bool IMap<ShortVector2, T>.TryGetValue(ShortVector2 position, out T item)
        {
            return mapCollection.TryGetValue(position, out item);
        }

    }

}
