using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 可以访问块内容的块地图;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TBlock"></typeparam>
    public class ReadableBlockMap<T, TBlock> : IMap<ShortVector2, T>
        where T : struct
        where TBlock : IMap<ShortVector2, T>
    {
        protected ReadableBlockMap() { }

        public ReadableBlockMap(ShortVector2 partitionSizes)
        {
            blockMap = new BlockMap<TBlock>(partitionSizes);
        }

        BlockMap<TBlock> blockMap;

        public BlockMap<TBlock> BlockMap
        {
            get { return blockMap; }
        }

        public IEnumerable<KeyValuePair<ShortVector2, T>> NodePair
        {
            get
            {
                foreach (var block in blockMap.BlocksPair)
                {
                    foreach (var node in block.Value.NodePair)
                    {
                        ShortVector2 position = blockMap.AddressToMapPoint(block.Key, node.Key);
                        yield return new KeyValuePair<ShortVector2, T>(position, node.Value);
                    }
                }
            }
        }

        public T this[ShortVector2 position]
        {
            get
            {
                ShortVector2 realPosition;
                TBlock block = TransformToBlock(position, out realPosition);
                return block[realPosition];
            }
            set
            {
                ShortVector2 realPosition;
                TBlock block = TransformToBlock(position, out realPosition);
                block[realPosition] = value;
            }
        }

        public void Add(ShortVector2 position, T item)
        {
            ShortVector2 realPosition;
            TBlock block = TransformToBlock(position, out realPosition);
            block.Add(realPosition, item);
        }

        public bool Remove(ShortVector2 position)
        {
            ShortVector2 realPosition;
            TBlock block = TransformToBlock(position, out realPosition);
            return block.Remove(realPosition);
        }

        public bool Contains(ShortVector2 position)
        {
            ShortVector2 realPosition;
            TBlock block = TransformToBlock(position, out realPosition);
            return block.Contains(realPosition);
        }

        public bool TryGetValue(ShortVector2 position, out T item)
        {
            ShortVector2 realPosition;
            TBlock block = TransformToBlock(position, out realPosition);
            return block.TryGetValue(realPosition, out item);
        }

        /// <summary>
        /// 转换成块的信息;
        /// </summary>
        TBlock TransformToBlock(ShortVector2 position, out ShortVector2 realPosition)
        {
            TBlock block;
            ShortVector2 address = blockMap.MapPointToAddress(position, out realPosition);

            if (blockMap.TryGetValue(address, out block))
            {
                return block;
            }
            throw BlockNotFoundException(address);

        }

        /// <summary>
        /// 实质清除块内容;
        /// </summary>
        void IMap<ShortVector2, T>.Clear()
        {
            blockMap.Clear();
        }

        /// <summary>
        /// 返回地图块错误信息;
        /// </summary>
        BlockNotFoundException BlockNotFoundException(ShortVector2 address)
        {
            return new BlockNotFoundException(address.ToString() + "地图块未载入!");
        }

    }
}
