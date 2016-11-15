using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 地图块,仅存在一个地图的地图块;
    /// </summary>
    public class MapBlockEdit<T> : IMap<ShortVector2, T>
    {
        public MapBlockEdit()
        {
            this.mapCollection = new Dictionary<ShortVector2, T>();
        }
        public MapBlockEdit(Dictionary<ShortVector2, T> mapCollection)
        {
            this.mapCollection = mapCollection;
        }

        private Dictionary<ShortVector2, T> mapCollection;

        /// <summary>
        /// 需要归档的地图;
        /// </summary>
        public Dictionary<ShortVector2, T> MapCollection
        {
            get { return mapCollection; }
        }

        /// <summary>
        /// 设置或获取这个位置的元素;
        /// </summary>
        public T this[ShortVector2 position]
        {
            get { return MapCollection[position]; }
            set { MapCollection[position] = value; }
        }

        /// <summary>
        /// 归档地图是否为空?
        /// </summary>
        public bool IsEmpty
        {
            get { return MapCollection.Count == 0; }
        }

        /// <summary>
        /// 对归档地图进行的操作;
        /// </summary>
        public void Add(ShortVector2 position, T item)
        {
            this.MapCollection.Add(position, item);
        }

        /// <summary>
        /// 对归档地图进行的操作;
        /// </summary>
        public bool Remove(ShortVector2 position)
        {
            return this.MapCollection.Remove(position);
        }

        /// <summary>
        /// 对归档地图进行的操作;
        /// </summary>
        public bool ContainsPosition(ShortVector2 position)
        {
            return this.MapCollection.ContainsKey(position);
        }

        /// <summary>
        /// 对归档地图进行的操作;
        /// </summary>
        public bool TryGetValue(ShortVector2 position, out T item)
        {
            return this.MapCollection.TryGetValue(position, out item);
        }

        /// <summary>
        /// 清空归档地图内容;
        /// </summary>
        public void Clear()
        {
            this.MapCollection.Clear();
        }

        public override string ToString()
        {
            return base.ToString() +
                "\n存在元素:" + MapCollection.Count;
        }


    }

}
