using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 进行归档的地图块;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArchiveBlock<T> : IHexMap<ShortVector2, T>
        where T : struct
    {

        public ArchiveBlock(Dictionary<ShortVector2, T> prefab, Dictionary<ShortVector2, T> archive)
        {
            Load(prefab, archive);
        }

        /// <summary>
        /// 地图节点;
        /// </summary>
        Dictionary<ShortVector2, T> mapNodeSet;
        /// <summary>
        /// 存在变化的节点;
        /// </summary>
        HashSet<ShortVector2> haveChangedSet;

        public IEnumerable<KeyValuePair<ShortVector2, T>> NodePair
        {
            get { return mapNodeSet; }
        }

        /// <summary>
        /// 根据预制地图和存档地图获取到地图块文件;
        /// </summary>
        void Load(Dictionary<ShortVector2, T> prefab, Dictionary<ShortVector2, T> archive)
        {
            if (archive == null && prefab == null)
            {
                mapNodeSet = new Dictionary<ShortVector2, T>();
                haveChangedSet = new HashSet<ShortVector2>();
            }
            else if (prefab == null)
            {
                mapNodeSet = archive;
                haveChangedSet = new HashSet<ShortVector2>(archive.Keys);
            }
            else if (archive == null)
            {
                mapNodeSet = prefab;
                haveChangedSet = new HashSet<ShortVector2>();
            }
            else
            {
                haveChangedSet = new HashSet<ShortVector2>(archive.Keys);
                prefab.AddOrReplace(archive);
                mapNodeSet = prefab;
            }
        }

        /// <summary>
        /// 获取到所有的更改;
        /// </summary>
        public Dictionary<ShortVector2, T> AllData()
        {
            return mapNodeSet;
        }

        /// <summary>
        /// 获取到归档的内容;
        /// </summary>
        public Dictionary<ShortVector2, T> Archive()
        {
            T node;
            Dictionary<ShortVector2, T> dictionary = new Dictionary<ShortVector2, T>();
            foreach (var position in haveChangedSet)
            {
                node = mapNodeSet[position];
                dictionary.Add(position, node);
            }
            return dictionary;
        }

        T IHexMap<ShortVector2, T>.this[ShortVector2 position]
        {
            get
            {
                return mapNodeSet[position];
            }
            set
            {
                mapNodeSet[position] = value;
                haveChangedSet.Add(position);
            }
        }

        void IHexMap<ShortVector2, T>.Add(ShortVector2 position, T item)
        {
            mapNodeSet.Add(position, item);
            haveChangedSet.Add(position);
        }

        void IHexMap<ShortVector2, T>.Clear()
        {
            mapNodeSet.Clear();
            haveChangedSet.Clear();
        }

        bool IHexMap<ShortVector2, T>.Contains(ShortVector2 position)
        {
            return mapNodeSet.ContainsKey(position);
        }

        bool IHexMap<ShortVector2, T>.Remove(ShortVector2 position)
        {
            bool isRemove = mapNodeSet.Remove(position);
            haveChangedSet.Remove(position);
            return isRemove;
        }

        bool IHexMap<ShortVector2, T>.TryGetValue(ShortVector2 position, out T item)
        {
            return mapNodeSet.TryGetValue(position, out item);
        }

    }
}
