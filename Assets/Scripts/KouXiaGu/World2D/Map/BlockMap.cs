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
    public class BlockMap<TBlock>
    {
        public BlockMap(ShortVector2 partitionSizes)
        {
            this.partitionSize = partitionSizes;
        }

        /// <summary>
        /// 块半径;
        /// </summary>
        ShortVector2 partitionSize;
        Dictionary<ShortVector2, TBlock> mapCollection = new Dictionary<ShortVector2, TBlock>();

        public ShortVector2 PartitionSizes
        {
            get { return partitionSize; }
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

        public void Add(ShortVector2 position, TBlock item)
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
        public ShortVector2 MapPointToAddress(ShortVector2 position)
        {
            ShortVector2 address = new ShortVector2();

            address.x = (short)(position.x / partitionSize.x);
            address.y = (short)(position.y / partitionSize.y);

            return address;
        }

        /// <summary>
        /// 将地图坐标转换成地图块的坐标 和 地图块内的坐标;
        /// </summary>
        public ShortVector2 MapPointToAddress(ShortVector2 position, out ShortVector2 realPosition)
        {
            ShortVector2 address = new ShortVector2();
            int realPositionX, realPositionY;

            address.x = (short)(Math.DivRem(position.x, partitionSize.x, out realPositionX));
            address.y = (short)(Math.DivRem(position.y, partitionSize.y, out realPositionY));
            realPosition = new ShortVector2((short)realPositionX, (short)realPositionY);

            return address;
        }

        /// <summary>
        /// 将地图块坐标转换成 地图坐标;
        /// </summary>
        public ShortVector2 AddressToMapPoint(ShortVector2 address, ShortVector2 addressPoint)
        {
            ShortVector2 mapPoint = new ShortVector2();

            mapPoint.x = (short)(address.x * partitionSize.x + addressPoint.x);
            mapPoint.y = (short)(address.y * partitionSize.y + addressPoint.y);

            return mapPoint;
        }

        /// <summary>
        /// 获取到这个块包含的所有点;
        /// </summary>
        public IEnumerable<ShortVector2> GetBlockRange(ShortVector2 address)
        {
            ShortVector2 southwestMapPoint = GetSouthwestMapPoint(address);
            ShortVector2 northeastMapPoint = GetNortheastMapPoint(address);

            return ShortVector2.Range(southwestMapPoint, northeastMapPoint);
        }

        /// <summary>
        /// 获取到这个块最西南角的点;
        /// </summary>
        ShortVector2 GetSouthwestMapPoint(ShortVector2 address)
        {
            ShortVector2 southwestAddressPoint = GetSouthwestAddressPoint(address);
            return AddressToMapPoint(address, southwestAddressPoint);
        }

        /// <summary>
        /// 获取到这个块最东北角的点;
        /// </summary>
        ShortVector2 GetNortheastMapPoint(ShortVector2 address)
        {
            ShortVector2 northeastAddressPoint = GetNortheastAddressPoint(address);
            return AddressToMapPoint(address, northeastAddressPoint);
        }

        /// <summary>
        /// 获取到这个块最西南角的点;
        /// </summary>
        ShortVector2 GetSouthwestAddressPoint(ShortVector2 address)
        {
            address.x -= partitionSize.x;
            address.y -= partitionSize.y;
            return address;
        }

        /// <summary>
        /// 获取到这个块最东北角的点;
        /// </summary>
        ShortVector2 GetNortheastAddressPoint(ShortVector2 address)
        {
            address.x += partitionSize.x;
            address.y += partitionSize.y;
            return address;
        }

    }

}
