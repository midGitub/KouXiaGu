using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 采用分块保存的地图结构;
    /// </summary>
    public class BlockMap<T> : IMap<CubicHexCoord, T>, IReadOnlyMap<CubicHexCoord, T>, IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>
    {

        /// <param name="blockSize">必须为奇数,若不是则+1</param>
        public BlockMap(short blockSize)
        {
            this.blockSize = (blockSize & 1) == 1 ? blockSize : ++blockSize;
            this.blockCount = blockSize * blockSize;

            mapCollection = new Dictionary<RectCoord, Dictionary<CubicHexCoord, T>>();
        }

        /// <summary>
        /// 块大小(需要是奇数);
        /// </summary>
        readonly short blockSize;

        /// <summary>
        /// 一个块存在的元素个数;
        /// </summary>
        readonly int blockCount;

        /// <summary>
        /// Key 保存块的编号, Value 保存块内容;
        /// </summary>
        Dictionary<RectCoord, Dictionary<CubicHexCoord, T>> mapCollection;


        public T this[CubicHexCoord position]
        {
            get { return FindBlock(position)[position]; }
            set { FindBlock(position)[position] = value; }
        }

        /// <summary>
        /// 元素个数;
        /// </summary>
        public int Count
        {
            get { return mapCollection.Values.Sum(block => block.Count); }
        }

        public IEnumerable<CubicHexCoord> Points
        {
            get { return mapCollection.Values.SelectMany(block => block.Keys); }
        }

        public IEnumerable<T> Nodes
        {
            get { return mapCollection.Values.SelectMany(block => block.Values); }
        }

        /// <summary>
        /// 块大小(需要是奇数);
        /// </summary>
        public short BlockSize
        {
            get { return blockSize; }
        }

        public Dictionary<CubicHexCoord, T> this[RectCoord key]
        {
            get { return ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection)[key]; }
            set { ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection)[key] = value; }
        }

        int ICollection<KeyValuePair<RectCoord, Dictionary<CubicHexCoord, T>>>.Count
        {
            get { return ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).Count; }
        }

        public ICollection<RectCoord> Keys
        {
            get { return ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).Keys; }
        }

        public ICollection<Dictionary<CubicHexCoord, T>> Values
        {
            get { return ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).Values; }
        }

        bool ICollection<KeyValuePair<RectCoord, Dictionary<CubicHexCoord, T>>>.IsReadOnly
        {
            get { return ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).IsReadOnly; }
        }


        /// <summary>
        /// 加入到,若超出地图块,则创建一个新的地图块;
        /// </summary>
        public void Add(CubicHexCoord position, T item)
        {
            RectCoord coord = GetBlockCoord(position);
            var block = TryCreateBlock(coord);
            block.Add(position, item);
        }

        /// <summary>
        /// 移除节点,移除成功返回true,否则返回false;
        /// </summary>
        public bool Remove(CubicHexCoord position)
        {
            Dictionary<CubicHexCoord, T> block;
            if (FindBlock(position, out block))
            {
                return block.Remove(position);
            }
            return false;
        }

        /// <summary>
        /// 确认是否存在这个节点,存在返回 true,否则返回 false;
        /// </summary>
        public bool Contains(CubicHexCoord position)
        {
            Dictionary<CubicHexCoord, T> block;
            if (FindBlock(position, out block))
            {
                return block.ContainsKey(position);
            }
            return false;
        }

        /// <summary>
        /// 尝试获取到这个节点,获取成功返回 true,否则返回 false;
        /// </summary>
        public bool TryGetValue(CubicHexCoord position, out T item)
        {
            Dictionary<CubicHexCoord, T> block;
            if (FindBlock(position, out block))
            {
                return block.TryGetValue(position, out item);
            }
            item = default(T);
            return false;
        }

        public void Clear()
        {
            foreach (var block in mapCollection.Values)
            {
                block.Clear();
            }
            mapCollection.Clear();
        }


        /// <summary>
        /// 创建一个新的地图块,并且加入到地图,若地图中已存在这个地图块,则返回其;
        /// </summary>
        public Dictionary<CubicHexCoord, T> TryCreateBlock(RectCoord coord)
        {
            Dictionary<CubicHexCoord, T> block;
            if (!mapCollection.TryGetValue(coord, out block))
            {
                block = CreateBlock();
                mapCollection.Add(coord, block);
            }
            return block;
        }

        /// <summary>
        /// 创建一个新的地图块;
        /// </summary>
        public Dictionary<CubicHexCoord, T> CreateBlock()
        {
            return new Dictionary<CubicHexCoord, T>(blockCount);
        }

        /// <summary>
        /// 查找获取到地图块,若不存在则返回异常;
        /// </summary>
        public Dictionary<CubicHexCoord, T> FindBlock(CubicHexCoord position)
        {
            RectCoord coord = GetBlockCoord(position);
            try
            {
                return mapCollection[coord];
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException(position + "超出地图定义范围;", e);
            }
        }

        /// <summary>
        /// 查找获取到地图块,若不存在返回false,否则返回true;
        /// </summary>
        public bool FindBlock(CubicHexCoord position, out Dictionary<CubicHexCoord, T> block)
        {
            RectCoord coord = GetBlockCoord(position);
            return mapCollection.TryGetValue(coord, out block);
        }

        /// <summary>
        /// 获取到所属的块坐标;
        /// </summary>
        public RectCoord GetBlockCoord(CubicHexCoord position)
        {
            short x = (short)Math.Round(position.X / (float)BlockSize);
            short y = (short)Math.Round(position.Z / (float)BlockSize);
            return new RectCoord(x, y);
        }


        public IEnumerator<KeyValuePair<CubicHexCoord, T>> GetEnumerator()
        {
            return mapCollection.Values.SelectMany(block => block).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }



        public void Add(RectCoord key, Dictionary<CubicHexCoord, T> value)
        {
            ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).Add(key, value);
        }

        public void Add(KeyValuePair<RectCoord, Dictionary<CubicHexCoord, T>> item)
        {
            ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).Add(item);
        }

        public bool Remove(RectCoord key)
        {
            return ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).Remove(key);
        }

        public bool Remove(KeyValuePair<RectCoord, Dictionary<CubicHexCoord, T>> item)
        {
            return ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).Remove(item);
        }

        public bool ContainsKey(RectCoord key)
        {
            return ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).ContainsKey(key);
        }

        public bool Contains(KeyValuePair<RectCoord, Dictionary<CubicHexCoord, T>> item)
        {
            return ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).Contains(item);
        }

        public bool TryGetValue(RectCoord key, out Dictionary<CubicHexCoord, T> value)
        {
            return ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).TryGetValue(key, out value);
        }


        public void CopyTo(KeyValuePair<RectCoord, Dictionary<CubicHexCoord, T>>[] array, int arrayIndex)
        {
            ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).CopyTo(array, arrayIndex);
        }

        IEnumerator<KeyValuePair<RectCoord, Dictionary<CubicHexCoord, T>>> IEnumerable<KeyValuePair<RectCoord, Dictionary<CubicHexCoord, T>>>.GetEnumerator()
        {
            return ((IDictionary<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).GetEnumerator();
        }

    }

}
