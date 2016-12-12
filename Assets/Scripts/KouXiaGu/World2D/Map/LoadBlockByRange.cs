using System;
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
        RectCoord loadRange = new RectCoord(40, 40);

        public IMapBlockIO<TBlock> MapBlockIO { get; set; }
        public BlockMap<TBlock> BlockMap { get; set; }

        /// <summary>
        /// 已经加载到的地图块编号;
        /// </summary>
        IEnumerable<RectCoord> loadedBlock
        {
            get { return BlockMap.Addresses; }
        }

        /// <summary>
        /// 更新地图的中心点;
        /// </summary>
        public void UpdateCenterPoint(RectCoord position)
        {
            IEnumerable<RectCoord> newBlock = GetBlock(position);
            RectCoord[] unloadPoints = loadedBlock.Except(newBlock).ToArray();
            RectCoord[] loadPoints = newBlock.Except(loadedBlock).ToArray();

            Load(loadPoints);
            Unload(unloadPoints);
        }

        /// <summary>
        /// 读取到这些位置的资源;
        /// </summary>
        void Load(IEnumerable<RectCoord> addresses)
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
        void Unload(IEnumerable<RectCoord> addresses)
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
        public IEnumerable<RectCoord> GetBlock(RectCoord centerPoint)
        {
            RectCoord southwestAddress = GetSouthwestAddress(centerPoint);
            RectCoord northeastAddress = GetNortheastAddress(centerPoint);

            for (short x = southwestAddress.x; x <= northeastAddress.x; x++)
            {
                for (short y = southwestAddress.y; y <= northeastAddress.y; y++)
                {
                    yield return new RectCoord(x, y);
                }
            }
        }

        RectCoord GetSouthwestAddress(RectCoord centerPoint)
        {
            RectCoord southwestPoint = GetSouthwestPoint(centerPoint);
            RectCoord southwestAddress = BlockMap.MapPointToAddress(southwestPoint);
            return southwestAddress;
        }

        RectCoord GetNortheastAddress(RectCoord centerPoint)
        {
            RectCoord northeastPoint = GetNortheastPoint(centerPoint);
            RectCoord northeastAddress = BlockMap.MapPointToAddress(northeastPoint);
            return northeastAddress;
        }

        /// <summary>
        /// 获取到西南角的点;
        /// </summary>
        RectCoord GetSouthwestPoint(RectCoord centerPoint)
        {
            centerPoint.x -= loadRange.x;
            centerPoint.y -= loadRange.y;
            return centerPoint;
        }

        /// <summary>
        /// 获取到东北角的点;
        /// </summary>
        RectCoord GetNortheastPoint(RectCoord centerPoint)
        {
            centerPoint.x += loadRange.x;
            centerPoint.y += loadRange.y;
            return centerPoint;
        }

    }

}
