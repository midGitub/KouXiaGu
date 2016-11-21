//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace KouXiaGu.World2D
//{

//    /// <summary>
//    /// 区分修改过的地图块和原本地图块;
//    /// 只要尝试获取到一个节点,则将其加入到保存合集内用于保存;
//    /// 获取到继承 IReadOnly 的这个接口的类,将不作为保存;
//    /// 每个节点只存在一个实例,预制合集内的元素包含或相等于归档合集;
//    /// </summary>
//    public class MapBlock<T, TRead> : IMap<ShortVector2, T>, IReadOnlyMap<ShortVector2, TRead>
//        where TRead : class, IReadOnly<T>
//        where T : class, TRead
//    {
//        public MapBlock()
//        {
//            this.prefabMap = new Dictionary<ShortVector2, T>();
//            this.archiveMap = new Dictionary<ShortVector2, T>();
//        }

//        public MapBlock(Dictionary<ShortVector2, T> map, bool isPrefabMap = true)
//        {
//            if (isPrefabMap)
//            {
//                this.prefabMap = map;
//                this.archiveMap = new Dictionary<ShortVector2, T>(map.Count);
//            }
//            else
//            {
//                this.prefabMap = new Dictionary<ShortVector2, T>(map);
//                this.archiveMap = map;
//            }
//        }
//        public MapBlock(Dictionary<ShortVector2, T> prefabMap, Dictionary<ShortVector2, T> archiveMap)
//        {
//            this.prefabMap = prefabMap;
//            this.archiveMap = archiveMap;
//            prefabMap.AddOrReplace(archiveMap);
//        }

//        /// <summary>
//        ///  预制的地图;
//        /// </summary>
//        private Dictionary<ShortVector2, T> prefabMap;
//        /// <summary>
//        /// 修改过的节点合集,节点数不会多余预制地图;
//        /// </summary>
//        private Dictionary<ShortVector2, T> archiveMap;

//        /// <summary>
//        /// 预制 和 改变了的节点合集;
//        /// </summary>
//        public Dictionary<ShortVector2, T> PrefabMap
//        {
//            get { return prefabMap; }
//        }
//        /// <summary>
//        /// 修改过的节点合集,节点数不会多与预制地图;
//        /// </summary>
//        public Dictionary<ShortVector2, T> ArchiveMap
//        {
//            get { return archiveMap; }
//        }

//        public T this[ShortVector2 position]
//        {
//            get
//            {
//                T node;
//                if (prefabMap.TryGetValue(position, out node))
//                {
//                    TryAddArchiveMap(position, node);
//                    return node;
//                }
//                throw new KeyNotFoundException();
//            }
//            set
//            {
//                PrefabMap[position] = value;

//                if (ArchiveMap.ContainsKey(position))
//                    ArchiveMap[position] = value;
//                else
//                    ArchiveMap.Add(position, value);
//            }
//        }

//        public void Add(ShortVector2 position, T item)
//        {
//            this.prefabMap.Add(position, item);
//            this.archiveMap.Add(position, item);
//        }

//        public bool Remove(ShortVector2 position)
//        {
//            this.archiveMap.Remove(position);
//            return this.prefabMap.Remove(position);
//        }

//        public bool Contains(ShortVector2 position)
//        {
//            return this.prefabMap.ContainsKey(position);
//        }

//        public bool TryGetValue(ShortVector2 position, out T item)
//        {
//            if (this.prefabMap.TryGetValue(position, out item))
//            {
//                TryAddArchiveMap(position, item);
//                return true;
//            }
//            return false;
//        }

//        public void Clear()
//        {
//            this.prefabMap.Clear();
//            this.archiveMap.Clear();
//        }

//        /// <summary>
//        /// 尝试加入到合集,若不存在则加入到;
//        /// </summary>
//        private void TryAddArchiveMap(ShortVector2 position, T node)
//        {
//            if (!archiveMap.ContainsKey(position))
//                archiveMap.Add(position, node);
//        }


//        TRead IReadOnlyMap<ShortVector2, TRead>.this[ShortVector2 position]
//        {
//            get { return this.prefabMap[position] as TRead; }
//        }

//        bool IReadOnlyMap<ShortVector2, TRead>.Contains(ShortVector2 position)
//        {
//            return this.prefabMap.ContainsKey(position);
//        }

//        bool IReadOnlyMap<ShortVector2, TRead>.TryGetValue(ShortVector2 position, out TRead item)
//        {
//            T node;
//            if (prefabMap.TryGetValue(position, out node))
//            {
//                item = node as TRead;
//                return true;
//            }
//            item = default(TRead);
//            return false;
//        }

//    }

//}
