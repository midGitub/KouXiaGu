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
    public class BlockedMap<T> : IMap<CubicHexCoord, T>, IMap<RectCoord, Dictionary<CubicHexCoord, T>>
    {

        CubicHexBlock block;

        /// <summary>
        /// Key 保存块的编号, Value 保存块内容;
        /// </summary>
        Map<RectCoord, Dictionary<CubicHexCoord, T>> mapCollection;

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

        public IEnumerable<CubicHexCoord> Keys
        {
            get { return mapCollection.Values.SelectMany(block => block.Keys); }
        }

        public IEnumerable<T> Values
        {
            get { return mapCollection.Values.SelectMany(block => block.Values); }
        }

        public int BlockWidth
        {
            get { return block.Width; }
        }

        public int BlockHeight
        {
            get { return block.Height; }
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="blockSize">必须为奇数</param>
        public BlockedMap(short blockSize)
        {
            block = new CubicHexBlock(blockSize);
            mapCollection = new Map<RectCoord, Dictionary<CubicHexCoord, T>>();
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="blockSize">必须为奇数</param>
        /// <param name="map">加入到地图的结构</param>
        public BlockedMap(short blockSize, IEnumerable<KeyValuePair<CubicHexCoord, T>> map) : this(blockSize)
        {
            Add(map);
        }

        /// <summary>
        /// 加入到,若超出地图块,则创建一个新的地图块;
        /// </summary>
        public void Add(CubicHexCoord position, T item)
        {
            RectCoord coord = GetChunkCoord(position);
            var block = TryCreateBlock(coord);
            block.Add(position, item);
        }

        /// <summary>
        /// 加入到,若超出地图块,则创建一个新的地图块;
        /// </summary>
        void Add(IEnumerable<KeyValuePair<CubicHexCoord, T>> map)
        {
            foreach (var pair in map)
            {
                Add(pair.Key, pair.Value);
            }
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
            return new Dictionary<CubicHexCoord, T>(block.ChunkElementCount);
        }

        /// <summary>
        /// 查找获取到地图块,若不存在则返回异常;
        /// </summary>
        public Dictionary<CubicHexCoord, T> FindBlock(CubicHexCoord position)
        {
            RectCoord coord = GetChunkCoord(position);
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
            RectCoord coord = GetChunkCoord(position);
            return mapCollection.TryGetValue(coord, out block);
        }

        /// <summary>
        /// 获取到所属的块坐标;
        /// </summary>
        public RectCoord GetChunkCoord(CubicHexCoord position)
        {
            return block.GetChunk(position);
        }

        public IEnumerator<KeyValuePair<CubicHexCoord, T>> GetEnumerator()
        {
            return mapCollection.Values.SelectMany(block => block).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        int IMap<RectCoord, Dictionary<CubicHexCoord, T>>.Count
        {
            get { return this.mapCollection.Count; }
        }

        IEnumerable<RectCoord> IMap<RectCoord, Dictionary<CubicHexCoord, T>>.Keys
        {
            get { return ((IMap<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).Keys; }
        }

        IEnumerable<Dictionary<CubicHexCoord, T>> IMap<RectCoord, Dictionary<CubicHexCoord, T>>.Values
        {
            get { return ((IMap<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).Values; }
        }

        public Dictionary<CubicHexCoord, T> this[RectCoord position]
        {
            get { return ((IMap<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection)[position]; }
            set { ((IMap<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection)[position] = value; }
        }

        public void Add(RectCoord position, Dictionary<CubicHexCoord, T> item)
        {
            ((IMap<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).Add(position, item);
        }

        public bool Remove(RectCoord position)
        {
            return ((IMap<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).Remove(position);
        }

        public bool Contains(RectCoord position)
        {
            return ((IMap<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).Contains(position);
        }

        public bool TryGetValue(RectCoord position, out Dictionary<CubicHexCoord, T> item)
        {
            return ((IMap<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).TryGetValue(position, out item);
        }

        IEnumerator<KeyValuePair<RectCoord, Dictionary<CubicHexCoord, T>>> IEnumerable<KeyValuePair<RectCoord, Dictionary<CubicHexCoord, T>>>.GetEnumerator()
        {
            return ((IMap<RectCoord, Dictionary<CubicHexCoord, T>>)this.mapCollection).GetEnumerator();
        }

    }

}
