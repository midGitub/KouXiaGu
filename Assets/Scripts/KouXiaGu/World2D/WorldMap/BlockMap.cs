using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 根据块加载的地图;
    /// </summary>
    /// <typeparam name="T">节点</typeparam>
    /// <typeparam name="TBlock">地图块</typeparam>
    public class BlockMap<T, TBlock> : IMap<IntVector2, T>, IBlockMap<ShortVector2, TBlock>
        where TBlock : IMap<ShortVector2, T>
    {

        [SerializeField]
        ShortVector2 partitionSizes;
        Dictionary<ShortVector2, TBlock> mapCollection = new Dictionary<ShortVector2, TBlock>();

        public ShortVector2 PartitionSizes
        {
            get { return partitionSizes; }
            set { partitionSizes = value; }
        }

        IEnumerable<ShortVector2> IBlockMap<ShortVector2, TBlock>.Addresses
        {
            get { return mapCollection.Keys; }
        }

        IEnumerable<TBlock> IBlockMap<ShortVector2, TBlock>.Blocks
        {
            get { return mapCollection.Values; }
        }

        TBlock IMap<ShortVector2, TBlock>.this[ShortVector2 position]
        {
            get { return mapCollection[position]; }
            set { mapCollection[position] = value; }
        }

        T IMap<IntVector2, T>.this[IntVector2 position]
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
   
        void IMap<IntVector2, T>.Add(IntVector2 position, T item)
        {
            ShortVector2 realPosition;
            TBlock block = TransformToBlock(position, out realPosition);
            block.Add(realPosition, item);
        }

        bool IMap<IntVector2, T>.Remove(IntVector2 position)
        {
            ShortVector2 realPosition;
            TBlock block = TransformToBlock(position, out realPosition);
            return block.Remove(realPosition);
        }

        bool IMap<IntVector2, T>.Contains(IntVector2 position)
        {
            ShortVector2 realPosition;
            TBlock block = TransformToBlock(position, out realPosition);
            return block.Contains(realPosition);
        }

        bool IMap<IntVector2, T>.TryGetValue(IntVector2 position, out T item)
        {
            ShortVector2 realPosition;
            TBlock block = TransformToBlock(position, out realPosition);
            return block.TryGetValue(realPosition, out item);
        }

        /// <summary>
        /// 转换成块的信息;
        /// </summary>
        TBlock TransformToBlock(IntVector2 position, out ShortVector2 realPosition)
        {
            TBlock block;
            ShortVector2 address = GetAddress(position, out realPosition);

            if (mapCollection.TryGetValue(address, out block))
            {
                return block;
            }
            throw BlockNotFoundException(address);

        }

        void IMap<IntVector2, T>.Clear()
        {
            mapCollection.Clear();
        }

        /// <summary>
        /// 返回地图块错误信息;
        /// </summary>
        BlockNotFoundException BlockNotFoundException(ShortVector2 address)
        {
            return new BlockNotFoundException(address.ToString() + "地图块未载入!\n" +
                mapCollection.Keys.ToString());
        }

        void IMap<ShortVector2, TBlock>.Add(ShortVector2 position, TBlock item)
        {
            mapCollection.Add(position, item);
        }

        bool IMap<ShortVector2, TBlock>.Remove(ShortVector2 position)
        {
           return mapCollection.Remove(position);
        }

        bool IMap<ShortVector2, TBlock>.Contains(ShortVector2 position)
        {
            return mapCollection.ContainsKey(position);
        }

        bool IMap<ShortVector2, TBlock>.TryGetValue(ShortVector2 position, out TBlock item)
        {
            return mapCollection.TryGetValue(position, out item);
        }

        void IMap<ShortVector2, TBlock>.Clear()
        {
            mapCollection.Clear();
        }

        /// <summary>
        /// 将地图坐标转换成地图块的坐标;
        /// </summary>
        public ShortVector2 GetAddress(IntVector2 position)
        {
            ShortVector2 address = new ShortVector2();

            address.x = (short)(position.x / partitionSizes.x);
            address.y = (short)(position.y / partitionSizes.y);

            return address;
        }

        /// <summary>
        /// 将地图坐标转换成地图块的坐标 和 地图块内的坐标;
        /// </summary>
        public ShortVector2 GetAddress(IntVector2 position, out ShortVector2 realPosition)
        {
            ShortVector2 address = new ShortVector2();
            int realPositionX, realPositionY;

            address.x = (short)(Math.DivRem(position.x, partitionSizes.x, out realPositionX));
            address.y = (short)(Math.DivRem(position.y, partitionSizes.y, out realPositionY));
            realPosition = new ShortVector2((short)realPositionX, (short)realPositionY);

            return address;
        }

        /// <summary>
        /// 将地图块坐标转换成 地图坐标;
        /// </summary>
        public IntVector2 AddressToPosition(ShortVector2 address, ShortVector2 realPosition)
        {
            IntVector2 position = new IntVector2();

            position.x = address.x * partitionSizes.x + realPosition.x;
            position.y = address.y * partitionSizes.y + realPosition.y;

            return position;
        }

        IEnumerator<KeyValuePair<ShortVector2, TBlock>> IEnumerable<KeyValuePair<ShortVector2, TBlock>>.GetEnumerator()
        {
            return mapCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<KeyValuePair<ShortVector2, TBlock>>).GetEnumerator();
        }
    }

}
