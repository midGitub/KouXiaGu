using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
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
        public BlockMap(RectCoord partitionSizes)
        {
            this.partitionSize = partitionSizes;
        }

        /// <summary>
        /// 块半径;
        /// </summary>
        RectCoord partitionSize;
        Dictionary<RectCoord, TBlock> mapCollection = new Dictionary<RectCoord, TBlock>();

        public RectCoord PartitionSizes
        {
            get { return partitionSize; }
        }

        public IEnumerable<RectCoord> Addresses
        {
            get { return mapCollection.Keys; }
        }

        public IEnumerable<TBlock> Blocks
        {
            get { return mapCollection.Values; }
        }

        public IEnumerable<KeyValuePair<RectCoord, TBlock>> BlocksPair
        {
            get { return mapCollection; }
        }

        public TBlock this[RectCoord position]
        {
            get { return mapCollection[position]; }
        }

        public void Add(RectCoord position, TBlock item)
        {
            mapCollection.Add(position, item);
        }

        public bool Remove(RectCoord position)
        {
            return mapCollection.Remove(position);
        }

        public bool Contains(RectCoord position)
        {
            return mapCollection.ContainsKey(position);
        }

        public bool TryGetValue(RectCoord position, out TBlock item)
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
        public RectCoord MapPointToAddress(RectCoord position)
        {
            RectCoord address = new RectCoord();

            address.x = (short)(position.x / partitionSize.x);
            address.y = (short)(position.y / partitionSize.y);

            return address;
        }

        /// <summary>
        /// 将地图坐标转换成地图块的坐标 和 地图块内的坐标;
        /// </summary>
        public RectCoord MapPointToAddress(RectCoord position, out RectCoord realPosition)
        {
            RectCoord address = new RectCoord();
            int realPositionX, realPositionY;

            address.x = (short)(Math.DivRem(position.x, partitionSize.x, out realPositionX));
            address.y = (short)(Math.DivRem(position.y, partitionSize.y, out realPositionY));
            realPosition = new RectCoord((short)realPositionX, (short)realPositionY);

            return address;
        }

        /// <summary>
        /// 将地图块坐标转换成 地图坐标;
        /// </summary>
        public RectCoord AddressToMapPoint(RectCoord address, RectCoord addressPoint)
        {
            RectCoord mapPoint = new RectCoord();

            mapPoint.x = (short)(address.x * partitionSize.x + addressPoint.x);
            mapPoint.y = (short)(address.y * partitionSize.y + addressPoint.y);

            return mapPoint;
        }

        /// <summary>
        /// 获取到这个块包含的所有点;
        /// </summary>
        public IEnumerable<RectCoord> GetBlockRange(RectCoord address)
        {
            RectCoord southwestMapPoint = GetSouthwestMapPoint(address);
            RectCoord northeastMapPoint = GetNortheastMapPoint(address);

            return RectCoord.Range(southwestMapPoint, northeastMapPoint);
        }

        /// <summary>
        /// 获取到这个块最西南角的点;
        /// </summary>
        RectCoord GetSouthwestMapPoint(RectCoord address)
        {
            RectCoord southwestAddressPoint = GetSouthwestAddressPoint(address);
            return AddressToMapPoint(address, southwestAddressPoint);
        }

        /// <summary>
        /// 获取到这个块最东北角的点;
        /// </summary>
        RectCoord GetNortheastMapPoint(RectCoord address)
        {
            RectCoord northeastAddressPoint = GetNortheastAddressPoint(address);
            return AddressToMapPoint(address, northeastAddressPoint);
        }

        /// <summary>
        /// 获取到这个块最西南角的点;
        /// </summary>
        RectCoord GetSouthwestAddressPoint(RectCoord address)
        {
            address.x -= partitionSize.x;
            address.y -= partitionSize.y;
            return address;
        }

        /// <summary>
        /// 获取到这个块最东北角的点;
        /// </summary>
        RectCoord GetNortheastAddressPoint(RectCoord address)
        {
            address.x += partitionSize.x;
            address.y += partitionSize.y;
            return address;
        }

    }

}
