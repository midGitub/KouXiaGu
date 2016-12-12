using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{


    /// <summary>
    /// 记录修改过的地图块,在保存仅保存修改过的地图块;
    /// 写入, 迭代 上锁,读操作 不锁;
    /// </summary>
    public class BlockMapRecord<T> : IMap<CubicHexCoord, T>, IReadOnlyMap<CubicHexCoord, T>, IBlockArchive<CubicHexCoord, T>
        where T : struct
    {

        /// <param name="blockSize">必须为奇数,若不是则+1</param>
        public BlockMapRecord(short blockSize)
        {
            mapCollection = new BlockMap<T>(blockSize);
            editedBlock = new HashSet<RectCoord>();
        }

        /// <summary>
        /// 块地图结构;
        /// </summary>
        readonly BlockMap<T> mapCollection;

        /// <summary>
        /// 在上次保存之后进行过编辑的块编号;
        /// </summary>
        readonly HashSet<RectCoord> editedBlock;

        /// <summary>
        /// 写入锁;
        /// </summary>
        readonly object syncWriteRoot = new object();

        /// <summary>
        /// 写入锁;
        /// </summary>
        public object SyncWriteRoot
        {
            get { return syncWriteRoot; }
        }

        public T this[CubicHexCoord position]
        {
            get { return this.mapCollection[position]; }
            set
            {
                RectCoord coord = mapCollection.GetBlockCoord(position);

                lock (syncWriteRoot)
                {
                    mapCollection[coord][position] = value;
                    AddChangedCoord(coord);
                }
            }
        }

        public int Count
        {
            get { return this.mapCollection.Count; }
        }

        public IEnumerable<T> Nodes
        {
            get
            {
                lock (syncWriteRoot)
                {
                    return this.mapCollection.Nodes;
                }
            }
        }

        public IEnumerable<CubicHexCoord> Points
        {
            get
            {
                lock (syncWriteRoot)
                {
                    return this.mapCollection.Points;
                }
            }
        }

        /// <summary>
        /// 加入到,若超出地图块,则创建一个新的地图块;
        /// </summary>
        public void Add(CubicHexCoord position, T item)
        {
            RectCoord coord = mapCollection.GetBlockCoord(position);

            lock (syncWriteRoot)
            {
                var block = mapCollection.TryCreateBlock(coord);
                block.Add(position, item);

                AddChangedCoord(coord);
            }
        }

        /// <summary>
        /// 移除节点,移除成功返回true,否则返回false;
        /// </summary>
        public bool Remove(CubicHexCoord position)
        {
            Dictionary<CubicHexCoord, T> block;
            RectCoord coord = mapCollection.GetBlockCoord(position);

            lock (syncWriteRoot)
            {
                if (mapCollection.TryGetValue(coord, out block))
                {
                    if (block.Remove(position))
                    {
                        AddChangedCoord(coord);
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 确认是否存在这个节点,存在返回 true,否则返回 false;
        /// </summary>
        public bool Contains(CubicHexCoord position)
        {
            return this.mapCollection.Contains(position);
        }

        /// <summary>
        /// 尝试获取到这个节点,获取成功返回 true,否则返回 false;
        /// </summary>
        public bool TryGetValue(CubicHexCoord position, out T item)
        {
            return this.mapCollection.TryGetValue(position, out item);
        }

        /// <summary>
        /// 清空所有保存的元素;
        /// </summary>
        public void Clear()
        {
            lock (syncWriteRoot)
            {
                this.mapCollection.Clear();
                editedBlock.Clear();
            }
        }

        /// <summary>
        /// 加入发生变化的块;
        /// </summary>
        void AddChangedCoord(RectCoord coord)
        {
            editedBlock.Add(coord);
        }

        public IEnumerator<KeyValuePair<CubicHexCoord, T>> GetEnumerator()
        {
            lock (syncWriteRoot)
            {
                return this.mapCollection.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.mapCollection.GetEnumerator();
        }


        /// <summary>
        /// 确认是否已经存在这个地图块;
        /// </summary>
        bool IBlockArchive<CubicHexCoord, T>.Contains(RectCoord coord)
        {
           return mapCollection.ContainsKey(coord);
        }

        /// <summary>
        /// 返回需要保存的地图块结构;
        /// </summary>
        BlockArchive<CubicHexCoord, T>[] IBlockArchive<CubicHexCoord, T>.GetArchives()
        {
            BlockArchive<CubicHexCoord, T>[] saveMap = new BlockArchive<CubicHexCoord, T>[editedBlock.Count];
            int index = 0;
            foreach (var coord in editedBlock)
            {
                Dictionary<CubicHexCoord, T> block = mapCollection[coord];
                saveMap[index++] = new BlockArchive<CubicHexCoord, T>(coord, mapCollection.BlockSize, block);
            }
            return saveMap;
        }

        /// <summary>
        /// 返回所有地图块结构;
        /// </summary>
        BlockArchive<CubicHexCoord, T>[] IBlockArchive<CubicHexCoord, T>.GetArchiveAll()
        {
            BlockArchive<CubicHexCoord, T>[] saveMap = new BlockArchive<CubicHexCoord, T>[mapCollection.Count];
            int index = 0;
            foreach (var pair in mapCollection as IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)
            {
                saveMap[index++] = new BlockArchive<CubicHexCoord, T>(pair.Key, mapCollection.BlockSize, pair.Value);
            }
            return saveMap;
        }

        /// <summary>
        /// 将存档结构加入到地图内;
        /// </summary>
        void IBlockArchive<CubicHexCoord, T>.AddArchives(BlockArchive<CubicHexCoord, T> archive)
        {
            if (archive.Size != mapCollection.BlockSize)
                throw new ArgumentOutOfRangeException("传入地图块大小和定义的不同!" + mapCollection.BlockSize + "," + archive.ToString());

            lock (syncWriteRoot)
            {
                mapCollection.Add(archive.Coord, archive.Map);
            }
        }

    }

}
