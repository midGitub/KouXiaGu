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
    public class ArchiveBlock<T> : IHexMap<RectCoord, T>
        where T : struct
    {

        public ArchiveBlock(Dictionary<RectCoord, T> prefab, Dictionary<RectCoord, T> archive)
        {
            Load(prefab, archive);
        }

        /// <summary>
        /// 地图节点;
        /// </summary>
        Dictionary<RectCoord, T> mapNodeSet;
        /// <summary>
        /// 存在变化的节点;
        /// </summary>
        HashSet<RectCoord> haveChangedSet;

        public IEnumerable<KeyValuePair<RectCoord, T>> NodePair
        {
            get { return mapNodeSet; }
        }

        /// <summary>
        /// 根据预制地图和存档地图获取到地图块文件;
        /// </summary>
        void Load(Dictionary<RectCoord, T> prefab, Dictionary<RectCoord, T> archive)
        {
            if (archive == null && prefab == null)
            {
                mapNodeSet = new Dictionary<RectCoord, T>();
                haveChangedSet = new HashSet<RectCoord>();
            }
            else if (prefab == null)
            {
                mapNodeSet = archive;
                haveChangedSet = new HashSet<RectCoord>(archive.Keys);
            }
            else if (archive == null)
            {
                mapNodeSet = prefab;
                haveChangedSet = new HashSet<RectCoord>();
            }
            else
            {
                haveChangedSet = new HashSet<RectCoord>(archive.Keys);
                prefab.AddOrReplace(archive);
                mapNodeSet = prefab;
            }
        }

        /// <summary>
        /// 获取到所有的更改;
        /// </summary>
        public Dictionary<RectCoord, T> AllData()
        {
            return mapNodeSet;
        }

        /// <summary>
        /// 获取到归档的内容;
        /// </summary>
        public Dictionary<RectCoord, T> Archive()
        {
            T node;
            Dictionary<RectCoord, T> dictionary = new Dictionary<RectCoord, T>();
            foreach (var position in haveChangedSet)
            {
                node = mapNodeSet[position];
                dictionary.Add(position, node);
            }
            return dictionary;
        }

        T IHexMap<RectCoord, T>.this[RectCoord position]
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

        void IHexMap<RectCoord, T>.Add(RectCoord position, T item)
        {
            mapNodeSet.Add(position, item);
            haveChangedSet.Add(position);
        }

        void IHexMap<RectCoord, T>.Clear()
        {
            mapNodeSet.Clear();
            haveChangedSet.Clear();
        }

        bool IHexMap<RectCoord, T>.Contains(RectCoord position)
        {
            return mapNodeSet.ContainsKey(position);
        }

        bool IHexMap<RectCoord, T>.Remove(RectCoord position)
        {
            bool isRemove = mapNodeSet.Remove(position);
            haveChangedSet.Remove(position);
            return isRemove;
        }

        bool IHexMap<RectCoord, T>.TryGetValue(RectCoord position, out T item)
        {
            return mapNodeSet.TryGetValue(position, out item);
        }

    }
}
