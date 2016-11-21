using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 确定地图块读取的范围;
    /// </summary>
    public class BlockLoader<T, TBlock> : BlockMap<T, TBlock>
        where TBlock : IMap<ShortVector2, T>
    {

        public BlockLoader(ShortVector2 partitionSizes , IntVector2 loadRange, IMapBlockIO<TBlock> mapBlockIO) : base(partitionSizes)
        {
            this.loadRange = loadRange;
            this.mapBlockIO = mapBlockIO;
        }

        /// <summary>
        /// 地图加载的范围;
        /// </summary>
        IntVector2 loadRange;
        IMapBlockIO<TBlock> mapBlockIO;

        /// <summary>
        /// 已经加载到的地图块编号;
        /// </summary>
        private IEnumerable<ShortVector2> loadedBlock
        {
            get { return mapCollection.Keys; }
        }

        /// <summary>
        /// 更新地图的中心点;
        /// </summary>
        public void UpdateCenterPoint(IntVector2 position)
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
        private void Load(IEnumerable<ShortVector2> addresses)
        {
            IMap<ShortVector2, TBlock> blockMap = (this as IMap<ShortVector2, TBlock>);
            foreach (var address in addresses)
            {
                TBlock block = mapBlockIO.Load(address);
                blockMap.Add(address, block);
            }
        }

        /// <summary>
        /// 卸载这些区域的资源;
        /// </summary>
        private void Unload(IEnumerable<ShortVector2> addresses)
        {
            IMap<ShortVector2, TBlock> blockMap = (this as IMap<ShortVector2, TBlock>);
            foreach (var address in addresses)
            {
                TBlock block = blockMap[address];
                mapBlockIO.Save(address, block);
                blockMap.Remove(address);
            }
        }

        /// <summary>
        /// 获取到需要读取的地图块;
        /// </summary>
        public IEnumerable<ShortVector2> GetBlock(IntVector2 centerPoint)
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

        private ShortVector2 GetSouthwestAddress(IntVector2 centerPoint)
        {
            IntVector2 southwestPoint = GetSouthwestPoint(centerPoint);
            ShortVector2 southwestAddress = GetAddress(southwestPoint);
            return southwestAddress;
        }

        private ShortVector2 GetNortheastAddress(IntVector2 centerPoint)
        {
            IntVector2 northeastPoint = GetNortheastPoint(centerPoint);
            ShortVector2 northeastAddress = GetAddress(northeastPoint);
            return northeastAddress;
        }

        /// <summary>
        /// 获取到西南角的点;
        /// </summary>
        private IntVector2 GetSouthwestPoint(IntVector2 centerPoint)
        {
            centerPoint.x -= loadRange.x;
            centerPoint.y -= loadRange.y;
            return centerPoint;
        }

        /// <summary>
        /// 获取到东北角的点;
        /// </summary>
        private IntVector2 GetNortheastPoint(IntVector2 centerPoint)
        {
            centerPoint.x += loadRange.x;
            centerPoint.y += loadRange.y;
            return centerPoint;
        }

    }

}
