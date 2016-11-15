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

        public MapBlock(Dictionary<ShortVector2, T> prefabMap)
        {
            this.prefabMap = prefabMap;
            this.archiveMap = new Dictionary<ShortVector2, T>();
        }

        public MapBlock(Dictionary<ShortVector2, T> prefabMap, Dictionary<ShortVector2, T> archiveMap)
        {
            this.prefabMap = prefabMap;
            this.archiveMap = archiveMap;
        }

        private Dictionary<ShortVector2, T> prefabMap;
        private Dictionary<ShortVector2, T> archiveMap;

        /// <summary>
        /// 预制地图;
        /// </summary>
        public Dictionary<ShortVector2, T> PrefabMap
        {
            get { return prefabMap; }
        }

        /// <summary>
        /// 需要归档的地图;
        /// </summary>
        public Dictionary<ShortVector2, T> ArchiveMap
        {
            get { return archiveMap; }
        }

        /// <summary>
        /// 设置或获取这个位置的元素;
        /// </summary>
        public T this[ShortVector2 position]
        {
            get { return GetItem(position); }
            set { SetItem(position, value); }
        }

        /// <summary>
        /// 归档地图是否为空?
        /// </summary>
        public bool IsEmpty
        {
            get { return ArchiveMap.Count == 0; }
        }

        /// <summary>
        /// 获取到这个位置的元素,先从更改地图内获取,若无法获取到,则在预制物体内获取;
        /// </summary>
        public T GetItem(ShortVector2 position)
        {
            T item;
            if (ArchiveMap.TryGetValue(position, out item))
            {
                return item;
            }
            else
            {
                return PrefabMap[position];
            }
        }

        /// <summary>
        /// 更改归档地图这个位置的元素;
        /// </summary>
        public void SetItem(ShortVector2 position, T item)
        {
            ArchiveMap[position] = item;
        }

        /// <summary>
        /// 加入到归档地图,若超出了预制地图大小,则返回异常 OuterBoundaryException;
        /// </summary>
        public void Add(ShortVector2 position, T item)
        {
            if (!this.PrefabMap.ContainsKey(position))
                throw new OuterBoundaryException("超出预制地图定义范围!");

            this.ArchiveMap.Add(position, item);
        }

        /// <summary>
        /// 对归档地图进行的操作;
        /// </summary>
        public bool Remove(ShortVector2 position)
        {
            return this.ArchiveMap.Remove(position);
        }

        /// <summary>
        /// 对归档地图进行的操作;
        /// </summary>
        public bool ContainsPosition(ShortVector2 position)
        {
            return this.ArchiveMap.ContainsKey(position);
        }

        /// <summary>
        /// 对归档地图进行的操作;
        /// </summary>
        public bool TryGetValue(ShortVector2 position, out T item)
        {
            return this.ArchiveMap.TryGetValue(position, out item);
        }

        /// <summary>
        /// 清空归档地图内容;
        /// </summary>
        public void Clear()
        {
            this.ArchiveMap.Clear();
        }

        public override string ToString()
        {
            return base.ToString() +
                "\n归档元素:" + ArchiveMap.Count;
        }

    }

}
