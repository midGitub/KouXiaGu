﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 按范围读取地图块;
    /// 根据定义的加载范围读取到其内的地图块;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TBlock"></typeparam>
    [Serializable]
    public class LoadBlockByRange<TBlock>
    {
        protected LoadBlockByRange() { }

        public LoadBlockByRange(IMapBlockIO<TBlock> mapBlockIO, BlockMap<TBlock> blockMap)
        {
            this.MapBlockIO = mapBlockIO;
            this.BlockMap = blockMap;
        }

        /// <summary>
        /// 地图加载的范围;
        /// </summary>
        [SerializeField]
        ShortVector2 loadRange = new ShortVector2(40, 40);

        public IMapBlockIO<TBlock> MapBlockIO { get; set; }
        public BlockMap<TBlock> BlockMap { get; set; }

        /// <summary>
        /// 已经加载到的地图块编号;
        /// </summary>
        IEnumerable<ShortVector2> loadedBlock
        {
            get { return BlockMap.Addresses; }
        }

        /// <summary>
        /// 更新地图的中心点;
        /// </summary>
        public void UpdateCenterPoint(ShortVector2 position)
        {
            IEnumerable<ShortVector2> newBlock = GetBlock(position);
            ShortVector2[] unloadPoints = loadedBlock.Except(newBlock).ToArray();
            ShortVector2[] loadPoints = newBlock.Except(loadedBlock).ToArray();

            Load(loadPoints);
            Unload(unloadPoints);
        }

        /// <summary>
        /// 读取到这些位置的资源;
        /// </summary>
        void Load(IEnumerable<ShortVector2> addresses)
        {
            foreach (var address in addresses)
            {
                TBlock block = MapBlockIO.Load(address);
                BlockMap.Add(address, block);
            }
        }

        /// <summary>
        /// 卸载这些区域的资源;
        /// </summary>
        void Unload(IEnumerable<ShortVector2> addresses)
        {
            foreach (var address in addresses)
            {
                TBlock block = BlockMap[address];
                MapBlockIO.Unload(address, block);
                BlockMap.Remove(address);
            }
        }

        /// <summary>
        /// 获取到需要读取的地图块;
        /// </summary>
        public IEnumerable<ShortVector2> GetBlock(ShortVector2 centerPoint)
        {
            ShortVector2 southwestAddress = GetSouthwestAddress(centerPoint);
            ShortVector2 northeastAddress = GetNortheastAddress(centerPoint);

            for (short x = southwestAddress.x; x <= northeastAddress.x; x++)
            {
                for (short y = southwestAddress.y; y <= northeastAddress.y; y++)
                {
                    yield return new ShortVector2(x, y);
                }
            }
        }

        ShortVector2 GetSouthwestAddress(ShortVector2 centerPoint)
        {
            ShortVector2 southwestPoint = GetSouthwestPoint(centerPoint);
            ShortVector2 southwestAddress = BlockMap.MapPointToAddress(southwestPoint);
            return southwestAddress;
        }

        ShortVector2 GetNortheastAddress(ShortVector2 centerPoint)
        {
            ShortVector2 northeastPoint = GetNortheastPoint(centerPoint);
            ShortVector2 northeastAddress = BlockMap.MapPointToAddress(northeastPoint);
            return northeastAddress;
        }

        /// <summary>
        /// 获取到西南角的点;
        /// </summary>
        ShortVector2 GetSouthwestPoint(ShortVector2 centerPoint)
        {
            centerPoint.x -= loadRange.x;
            centerPoint.y -= loadRange.y;
            return centerPoint;
        }

        /// <summary>
        /// 获取到东北角的点;
        /// </summary>
        ShortVector2 GetNortheastPoint(ShortVector2 centerPoint)
        {
            centerPoint.x += loadRange.x;
            centerPoint.y += loadRange.y;
            return centerPoint;
        }

    }

}