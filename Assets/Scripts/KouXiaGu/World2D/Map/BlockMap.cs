using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 块地图结构;
    /// </summary>
    /// <typeparam name="TBlock"></typeparam>
    [Serializable]
    public class BlockMap<TBlock> : IBlockMap<ShortVector2, TBlock>
    {

        [SerializeField]
        ShortVector2 partitionSizes;
        Dictionary<ShortVector2, TBlock> mapCollection = new Dictionary<ShortVector2, TBlock>();

        public ShortVector2 PartitionSizes
        {
            get { return partitionSizes; }
        }

        public IEnumerable<ShortVector2> Addresses
        {
            get { return mapCollection.Keys; }
        }

        public IEnumerable<TBlock> Blocks
        {
            get { return mapCollection.Values; }
        }

        public IEnumerable<KeyValuePair<ShortVector2, TBlock>> BlocksPair
        {
            get { return mapCollection; }
        }

        public TBlock this[ShortVector2 position]
        {
            get { return mapCollection[position]; }
        }

        public virtual void Add(ShortVector2 position, TBlock item)
        {
            mapCollection.Add(position, item);
        }

        public bool Remove(ShortVector2 position)
        {
            return mapCollection.Remove(position);
        }

        public bool Contains(ShortVector2 position)
        {
            return mapCollection.ContainsKey(position);
        }

        public bool TryGetValue(ShortVector2 position, out TBlock item)
        {
            return mapCollection.TryGetValue(position, out item);
        }

        public void Clear()
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

    }

}
