using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KouXiaGu.Map
{

    [Serializable]
    public struct BlocksMapInfo
    {
        /// <summary>
        /// 地图分区大小;
        /// </summary>
        public ShortVector2 partitionSizes;
        public ShortVector2 minRadiationRange;
        public ShortVector2 maxRadiationRange;
    }

    /// <summary>
    /// 动态地图数据结构;负责对地图进行动态的读取和保存;
    /// 因为是动态加载的,所有取值和赋值时需要确保是否在目标范围内;
    /// </summary>
    /// <typeparam name="TMapBlock">保存的地图块</typeparam>
    /// <typeparam name="T">地图保存的类型</typeparam>
    public class DynaBlocksMap<TMapBlock, T> : IMap<IntVector2, T>, IReadOnlyMap<IntVector2, T>
        where TMapBlock: IMap<ShortVector2, T>
    {
        protected DynaBlocksMap(BlocksMapInfo info)
        {
            this.partitionSizes = info.partitionSizes;
            this.minRadiationRange = info.minRadiationRange;
            this.maxRadiationRange = info.maxRadiationRange;
            this.mapCollection = new Dictionary<ShortVector2, TMapBlock>();
        }

        public DynaBlocksMap(BlocksMapInfo info, IMapBlockIO<TMapBlock, T> dynamicMapIO) : this(info)
        {
            this.dynamicMapIO = dynamicMapIO;
        }

        private ShortVector2 partitionSizes;
        private ShortVector2 minRadiationRange;
        private ShortVector2 maxRadiationRange;
        private IMapBlockIO<TMapBlock, T> dynamicMapIO;
        private Dictionary<ShortVector2, TMapBlock> mapCollection;

        /// <summary>
        /// 上一次更新目标所在的地图块;
        /// </summary>
        private ShortVector2 lastUpdateTargetAddress;


        /// <summary>
        /// 文件读取保存接口;
        /// </summary>
        public IMapBlockIO<TMapBlock, T> DynamicMapIO
        {
            get { return dynamicMapIO; }
            protected set { dynamicMapIO = value; }
        }

        /// <summary>
        /// 地图分区大小;
        /// </summary>
        private ShortVector2 PartitionSizes
        {
            get { return partitionSizes; }
        }

        /// <summary>
        /// 目标辐射范围,既以为目标中心,需要读取的地图范围;
        /// </summary>
        private ShortVector2 MinRadiationRange
        {
            get { return minRadiationRange; }
        }

        private ShortVector2 MaxRadiationRange
        {
            get { return maxRadiationRange; }
        }

        /// <summary>
        /// 未知是否为空!始终返回false;
        /// </summary>
        bool IReadOnlyMap<IntVector2, T>.IsEmpty
        {
            get { return false; }
        }

        /// <summary>
        /// 获取到这个位置的元素;
        /// </summary>
        public T this[IntVector2 position]
        {
            get { return GetItem(position); }
            set { SetItem(position, value); }
        }


        /// <summary>
        /// 获取到这个位置的值;若不存在则返回异常;
        /// KeyNotFoundException : 这个点不存在,或超出读取范围;
        /// </summary>
        private T GetItem(IntVector2 position)
        {
            ShortVector2 realPosition;
            ShortVector2 address = TransfromToAddress(position, out realPosition);
            IMap<ShortVector2, T> mapPaging = mapCollection[address];
            return mapPaging[realPosition];
        }

        /// <summary>
        /// 设置这个值;若不存在则返回异常;
        /// KeyNotFoundException : 这个点不存在,或超出读取范围;
        /// </summary>
        private void SetItem(IntVector2 position, T item)
        {
            ShortVector2 realPosition;
            ShortVector2 address = TransfromToAddress(position, out realPosition);
            IMap<ShortVector2, T> mapPaging = mapCollection[address];
            mapPaging[realPosition] = item;
        }

        /// <summary>
        /// 将这个元素加入到地图,若无法保存则返回异常;
        /// </summary>
        public void Add(IntVector2 position, T item)
        {
            ShortVector2 realPosition;
            ShortVector2 address = TransfromToAddress(position, out realPosition);
            IMap<ShortVector2, T> mapPaging;
            try
            {
                mapPaging = mapCollection[address];
            }
            catch (KeyNotFoundException e)
            {
                throw BlockNotFoundException(address, e);
            }
            mapPaging.Add(realPosition, item);
        }

        /// <summary>
        /// 从地图上移除这个元素;
        /// KeyNotFoundException : 超出范围;
        /// </summary>
        public bool Remove(IntVector2 position)
        {
            ShortVector2 realPosition;
            ShortVector2 address = TransfromToAddress(position, out realPosition);
            IMap<ShortVector2, T> mapPaging = mapCollection[address];
            return mapPaging.Remove(realPosition);
        }

        /// <summary>
        /// 确认地图是否存在这个点;
        /// </summary>
        public bool ContainsPosition(IntVector2 position)
        {
            TMapBlock mapPaging;
            ShortVector2 realPosition;
            ShortVector2 address = TransfromToAddress(position, out realPosition);
            if (mapCollection.TryGetValue(address, out mapPaging))
            {
                return mapPaging.ContainsPosition(realPosition);
            }
            return false;
        }

        /// <summary>
        /// 尝试获取到这个点的元素;
        /// </summary>
        public bool TryGetValue(IntVector2 position, out T item)
        {
            TMapBlock mapPaging;
            ShortVector2 realPosition;
            ShortVector2 address = TransfromToAddress(position, out realPosition);
            if (mapCollection.TryGetValue(address, out mapPaging))
            {
                if (mapPaging.TryGetValue(realPosition, out item))
                {
                    return true;
                }
            }
            item = default(T);
            return false;
        }

        public void Clear()
        {
            mapCollection.Clear();
        }

        private BlockNotFoundException BlockNotFoundException(ShortVector2 address, Exception e)
        {
            return new BlockNotFoundException(address.ToString() + "地图块未载入!\n" +
                mapCollection.Keys.ToString()
                , e);
        }


        /// <summary>
        /// 根据目标所在位置更新地图数据;
        /// 若目标所在地块位置与上次更新相同,则不做更新(除非 check 为false);
        /// </summary>
        public void UpdateMapData(IntVector2 targetMapPosition, bool check = true)
        {
            ShortVector2 targetAddress = TransfromToAddress(targetMapPosition);

            if (this.lastUpdateTargetAddress != targetAddress || !check)
            {
                UpdateMapData(targetAddress);
                this.lastUpdateTargetAddress = targetAddress;
            }
        }

        /// <summary>
        /// 根据目标所在地图块更新地图数据;
        /// </summary>
        private void UpdateMapData(ShortVector2 targetAddress)
        {
            UnloadBlock(targetAddress);
            LoadBlocks(targetAddress);
        }

        /// <summary>
        /// 根据分页地址加入到地图;
        /// </summary>
        private void LoadBlocks(ShortVector2 targetAddress)
        {
            IEnumerable<ShortVector2> radiationAddresses = GetMinRadiationAddresses(targetAddress);
            IEnumerable<ShortVector2> addRadiationAddresses = radiationAddresses.
                Where(address => !mapCollection.ContainsKey(address));

            foreach (var address in addRadiationAddresses)
            {
                LoadBlockAsyn(address);
            }
        }

        /// <summary>
        /// 获取到这个分页,并且加入到地图;
        /// </summary>
        /// <param name="address"></param>
        private void LoadBlockAsyn(ShortVector2 address)
        {
            Action<TMapBlock> onComplete = mapBlock => mapCollection.Add(address, mapBlock);
            Action<Exception> onFail = e => Debug.LogWarning("不存在地图,跳过;" + address.ToString() + e);
            dynamicMapIO.LoadAsyn(address, onComplete, onFail);
            return;
        }

        /// <summary>
        /// 根据分页地址从地图内移除;根据最大辐射移除;
        /// </summary>
        private void UnloadBlocks(ShortVector2 targetAddress)
        {
            IEnumerable<ShortVector2> radiationAddresses = GetMaxRadiationAddresses(targetAddress);
            IEnumerable<ShortVector2> removeRadiationAddresses = mapCollection.Keys.
                Where(address => !radiationAddresses.Contains(address));
            foreach (var address in removeRadiationAddresses)
            {
                UnloadBlock(address);
            }
        }

        /// <summary>
        /// 移除这个地图块;
        /// 移除成功返回true,否则返回false;
        /// </summary>
        /// <param name="targetAddress"></param>
        private bool UnloadBlock(ShortVector2 address)
        {
            TMapBlock mapPaging;
            if (mapCollection.TryGetValue(address, out mapPaging))
            {
                SaveBlockAsyn(address, mapPaging);
                return mapCollection.Remove(address);
            }
            return false;
        }

        /// <summary>
        /// 异步保存所有在缓存内的地图块;
        /// </summary>
        private void SaveBlockAsyn(ShortVector2 address, TMapBlock mapBlock)
        {
            Action onComplete = () => Debug.Log("保存地图成功!" + address.ToString());
            Action<Exception> onFail = e => Debug.LogWarning("未读取地图成功!" + address.ToString() + e);
            dynamicMapIO.SaveAsyn(address, mapBlock, onComplete, onFail);
        }

        /// <summary>
        /// 同步保存所有在缓存内的地图块;
        /// </summary>
        public void SaveBlocks()
        {
            foreach (var block in mapCollection)
            {
                dynamicMapIO.Save(block.Key, block.Value);
            }
        }

        /// <summary>
        /// 保存地图所有的地图块;
        /// </summary>
        /// <param name="compulsorySave"></param>
        public void SaveBlocksAsyn()
        {
            foreach (var block in mapCollection)
            {
                SaveBlockAsyn(block.Key, block.Value);
            }
        }

        /// <summary>
        /// 获取到目标辐射到的最大范围;
        /// </summary>
        private IEnumerable<ShortVector2> GetMaxRadiationAddresses(ShortVector2 address)
        {
            ShortVector2 targetRadiationRange = MaxRadiationRange;
            return GetRadiationAddresses(address, targetRadiationRange);
        }

        /// <summary>
        /// 获取到目标辐射到的最小范围;
        /// </summary>
        private IEnumerable<ShortVector2> GetMinRadiationAddresses(ShortVector2 address)
        {
            ShortVector2 targetRadiationRange = MinRadiationRange;
            return GetRadiationAddresses(address, targetRadiationRange);
        }

        /// <summary>
        /// 获取到目标辐射地图块地址;
        /// </summary>
        private IEnumerable<ShortVector2> GetRadiationAddresses(ShortVector2 address, ShortVector2 radiationRange)
        {
            for (short x = (short)(-radiationRange.x); x <= radiationRange.x; x++)
            {
                for (short y = (short)(-radiationRange.y); y <= radiationRange.y; y++)
                {
                    ShortVector2 radiationAddresses = new ShortVector2(x, y) + address;
                    yield return radiationAddresses;
                }
            }
        }

        /// <summary>
        /// 将地图坐标转换成地图块的坐标;
        /// </summary>
        private ShortVector2 TransfromToAddress(IntVector2 position)
        {
            ShortVector2 partitionSizes = PartitionSizes;
            ShortVector2 address = new ShortVector2();

            address.x = (short)(position.x / partitionSizes.x);
            address.y = (short)(position.y / partitionSizes.y);

            return address;
        }

        /// <summary>
        /// 将地图坐标转换成地图块的坐标 和 地图块内的坐标;
        /// </summary>
        private ShortVector2 TransfromToAddress(IntVector2 position, out ShortVector2 realPosition)
        {
            ShortVector2 partitionSizes = PartitionSizes;
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
        private IntVector2 TransformToPosition(ShortVector2 address, ShortVector2 realPosition)
        {
            ShortVector2 partitionSizes = PartitionSizes;
            IntVector2 position = new IntVector2();

            position.x = address.x * partitionSizes.x + realPosition.x;
            position.y = address.y * partitionSizes.y + realPosition.y;

            return position;
        }

    }

}
