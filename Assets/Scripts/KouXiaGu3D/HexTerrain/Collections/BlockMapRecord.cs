using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.HexTerrain
{


    /// <summary>
    /// 记录修改过的地图块,在保存仅保存修改过的地图块;
    /// </summary>
    public class BlockMapRecord<T> : IMap2D<CubicHexCoord, T>, IReadOnlyMap2D<CubicHexCoord, T>
        where T : struct
    {

        /// <param name="blockSize">必须为奇数,若不是则+1</param>
        public BlockMapRecord(short blockSize)
        {
            mapCollection = new BlockMap<T>(blockSize);
            editedBlock = new HashSet<ShortVector2>();
        }

        /// <summary>
        /// 块地图结构;
        /// </summary>
        BlockMap<T> mapCollection;

        /// <summary>
        /// 在上次保存之后进行过编辑的块编号;
        /// </summary>
        HashSet<ShortVector2> editedBlock;


        public T this[CubicHexCoord position]
        {
            get { return ((IMap2D<CubicHexCoord, T>)this.mapCollection)[position]; }
            set
            {
                ShortVector2 coord = mapCollection.GetBlockCoord(position);
                mapCollection[coord][position] = value;
                AddChangedCoord(coord);
            }
        }

        public int Count
        {
            get { return ((IMap2D<CubicHexCoord, T>)this.mapCollection).Count; }
        }

        public IEnumerable<T> Nodes
        {
            get { return ((IMap2D<CubicHexCoord, T>)this.mapCollection).Nodes; }
        }

        public IEnumerable<CubicHexCoord> Points
        {
            get { return ((IMap2D<CubicHexCoord, T>)this.mapCollection).Points; }
        }

        /// <summary>
        /// 加入到,若超出地图块,则创建一个新的地图块;
        /// </summary>
        public void Add(CubicHexCoord position, T item)
        {
            ShortVector2 coord = mapCollection.GetBlockCoord(position);
            var block = mapCollection.TryCreateBlock(coord);
            block.Add(position, item);

            AddChangedCoord(coord);
        }

        /// <summary>
        /// 移除节点,移除成功返回true,否则返回false;
        /// </summary>
        public bool Remove(CubicHexCoord position)
        {
            Dictionary<CubicHexCoord, T> block;
            ShortVector2 coord = mapCollection.GetBlockCoord(position);

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

        /// <summary>
        /// 确认是否存在这个节点,存在返回 true,否则返回 false;
        /// </summary>
        public bool Contains(CubicHexCoord position)
        {
            return ((IMap2D<CubicHexCoord, T>)this.mapCollection).Contains(position);
        }

        /// <summary>
        /// 尝试获取到这个节点,获取成功返回 true,否则返回 false;
        /// </summary>
        public bool TryGetValue(CubicHexCoord position, out T item)
        {
            return ((IMap2D<CubicHexCoord, T>)this.mapCollection).TryGetValue(position, out item);
        }

        /// <summary>
        /// 清空所有保存的元素;
        /// </summary>
        public void Clear()
        {
            ((IMap2D<CubicHexCoord, T>)this.mapCollection).Clear();
            editedBlock.Clear();
        }

        public IEnumerator<KeyValuePair<CubicHexCoord, T>> GetEnumerator()
        {
            return ((IMap2D<CubicHexCoord, T>)this.mapCollection).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IMap2D<CubicHexCoord, T>)this.mapCollection).GetEnumerator();
        }


        /// <summary>
        /// 加入发生变化的块;
        /// </summary>
        void AddChangedCoord(ShortVector2 coord)
        {
            editedBlock.Add(coord);
        }

        /// <summary>
        /// 返回需要保存的地图块结构;
        /// </summary>
        public BlockArchive<T>[] Save()
        {
            BlockArchive<T>[] saveMap = new BlockArchive<T>[editedBlock.Count];
            int index = 0;
            foreach (var coord in editedBlock)
            {
                Dictionary<CubicHexCoord, T> block = mapCollection[coord];
                saveMap[index++] = new BlockArchive<T>(coord, mapCollection.BlockSize, block);
            }
            return saveMap;
        }

        /// <summary>
        /// 将存档结构加入到地图内;
        /// </summary>
        /// <param name="isCheck">是否在加入时进行检查?</param>
        public void Load(BlockArchive<T> archive, bool isCheck = true)
        {
            if (isCheck && archive.Size != mapCollection.BlockSize)
                throw new ArgumentOutOfRangeException("传入地图块大小和定义的不同!" + mapCollection.BlockSize + "," + archive.ToString());

            mapCollection.Add(archive.Coord, archive.Map);
        }

    }

}
