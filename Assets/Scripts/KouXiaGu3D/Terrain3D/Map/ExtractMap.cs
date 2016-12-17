using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 对修改的节点进行保存;
    /// </summary>
    public class ExtractMap<TKey, TValue> : IDictionary<TKey,TValue>
        where TValue : struct
    {

        Dictionary<TKey, TValue> mapCollection;

        /// <summary>
        /// 记录发生变化的位置;
        /// </summary>
        Dictionary<TKey, TValue> archiveMap;

        public TValue this[TKey key]
        {
            get { return this.mapCollection[key]; }
            set
            {
                this.mapCollection[key] = value;
                archiveMap.AddOrUpdate(key, value);
            }
        }

        public int Count
        {
            get { return this.mapCollection.Count; }
        }

        public ICollection<TKey> Keys
        {
            get { return this.mapCollection.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return this.mapCollection.Values; }
        }

        public int ArchiveCount
        {
            get { return this.archiveMap.Count; }
        }

        public ICollection<TKey> ArchiveKeys
        {
            get { return this.archiveMap.Keys; }
        }

        public bool IsReadOnly
        {
            get { return ((IDictionary<TKey, TValue>)this.mapCollection).IsReadOnly; }
        }

        /// <summary>
        /// 创建一个空地图;
        /// </summary>
        public ExtractMap()
        {
            this.mapCollection = new Dictionary<TKey, TValue>();
            this.archiveMap = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// 读取预制地图数据,并且清空存档信息;
        /// </summary>
        public void Load(string prefabMapPath)
        {
            if (this.mapCollection != null)
                this.mapCollection.Clear();
            if (this.archiveMap != null)
                this.archiveMap.Clear();

            this.mapCollection = LoadMapData(prefabMapPath);
        }

        /// <summary>
        /// 读取地图数据,若存档地图不存在则返回 异常;
        /// </summary>
        public void Load(string prefabMapPath, string archiveMapPath)
        {
            if (this.mapCollection != null)
                this.mapCollection.Clear();
            if (this.archiveMap != null)
                this.archiveMap.Clear();

            this.mapCollection = LoadMapData(prefabMapPath);
            this.archiveMap = LoadMapData(archiveMapPath);
            this.mapCollection.AddOrUpdate(archiveMap);
        }

        /// <summary>
        /// 清除归档地图;
        /// </summary>
        public void ClearArchive()
        {
            this.archiveMap.Clear();
        }

        /// <summary>
        /// 读取到地图数据;
        /// </summary>
        Dictionary<TKey, TValue> LoadMapData(string filePath)
        {
            Dictionary<TKey, TValue> archive = SerializeHelper.DeserializeProtoBuf<Dictionary<TKey, TValue>>(filePath);
            return archive;
        }

        /// <summary>
        /// 保存存档地图到;
        /// </summary>
        public void SaveArchive(string filePath)
        {
            SaveMapData(filePath, this.archiveMap);
        }

        /// <summary>
        /// 保存预制地图到;
        /// </summary>
        public void SavePrefab(string filePath)
        {
            SaveMapData(filePath, this.mapCollection);
        }

        /// <summary>
        /// 保存地图数据到文件;
        /// </summary>
        void SaveMapData(string filePath, Dictionary<TKey, TValue> map)
        {
            SerializeHelper.SerializeProtoBuf(filePath, map);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((IDictionary<TKey, TValue>)this.mapCollection).Add(item);
            ((IDictionary<TKey, TValue>)this.archiveMap).Add(item);
        }

        public void Add(TKey key, TValue value)
        {
            this.mapCollection.Add(key, value);
            this.archiveMap.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return this.mapCollection.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            bool remove = this.mapCollection.Remove(key);
            this.archiveMap.Remove(key);
            return remove;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.mapCollection.TryGetValue(key, out value);
        }


        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)this.mapCollection).Contains(item);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            bool remove = ((IDictionary<TKey, TValue>)this.mapCollection).Remove(item);
            ((IDictionary<TKey, TValue>)this.archiveMap).Remove(item);
            return remove;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)this.mapCollection).CopyTo(array, arrayIndex);
        }

        public void Clear()
        {
            this.mapCollection.Clear();
            this.archiveMap.Clear();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)this.mapCollection).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)this.mapCollection).GetEnumerator();
        }

    }

}
