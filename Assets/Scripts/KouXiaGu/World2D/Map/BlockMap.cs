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

            address.X = (short)(position.X / partitionSize.X);
            address.Y = (short)(position.Y / partitionSize.Y);

            return address;
        }

        /// <summary>
        /// 将地图坐标转换成地图块的坐标 和 地图块内的坐标;
        /// </summary>
        public RectCoord MapPointToAddress(RectCoord position, out RectCoord realPosition)
        {
            RectCoord address = new RectCoord();
            int realPositionX, realPositionY;

            address.X = (short)(Math.DivRem(position.X, partitionSize.X, out realPositionX));
            address.Y = (short)(Math.DivRem(position.Y, partitionSize.Y, out realPositionY));
            realPosition = new RectCoord((short)realPositionX, (short)realPositionY);

            return address;
        }

        /// <summary>
        /// 将地图块坐标转换成 地图坐标;
        /// </summary>
        public RectCoord AddressToMapPoint(RectCoord address, RectCoord addressPoint)
        {
            RectCoord mapPoint = new RectCoord();

            mapPoint.X = (short)(address.X * partitionSize.X + addressPoint.X);
            mapPoint.Y = (short)(address.Y * partitionSize.Y + addressPoint.Y);

            return mapPoint;
        }

        /// <summary>
        /// 获取到这个块包含的所有点;
        /// </summary>
        public IEnumerable<RectCoord> GetBlockRange(RectCoord address)
        {
            RectCoord southwestMapPoint = GetSouthwestMapPoint(address);
            RectCoord northeastMapPoint = GetNortheastMapPoint(address);

            return RectCoord.RectRange(southwestMapPoint, northeastMapPoint);
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
            address.X -= partitionSize.X;
            address.Y -= partitionSize.Y;
            return address;
        }

        /// <summary>
        /// 获取到这个块最东北角的点;
        /// </summary>
        RectCoord GetNortheastAddressPoint(RectCoord address)
        {
            address.X += partitionSize.X;
            address.Y += partitionSize.Y;
            return address;
        }

    }

}
