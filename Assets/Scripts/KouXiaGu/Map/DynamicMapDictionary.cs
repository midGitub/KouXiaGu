using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 动态地图数据结构;
    /// 因为是动态加载的,所有取值和赋值时需要检查是否在目标范围内;
    /// </summary>
    public class DynamicMapDictionary<T> : IGameMap<IntVector2, T>, IReadOnlyMap<IntVector2, T>
    {
        public DynamicMapDictionary(DynamicPagingInfo dynamicMapInfo, IDynamicMapIO<DynamicMapPaging<T>, T> dynamicMapIO)
        {
            this.pagingInfo = dynamicMapInfo;
            this.mapCollection = new Dictionary<ShortVector2, IGameMap<ShortVector2, T>>();
            this.dynamicMapIO = dynamicMapIO;
        }

        public DynamicMapDictionary(DynamicPagingInfo dynamicMapInfo)
        {
            this.pagingInfo = dynamicMapInfo;
            mapCollection = new Dictionary<ShortVector2, IGameMap<ShortVector2, T>>();
            dynamicMapIO = new DynamicMapIO<T>(dynamicMapInfo);
        }

        /// <summary>
        /// 地图动态读取信息,地图块信息;
        /// </summary>
        private DynamicPagingInfo pagingInfo;

        /// <summary>
        /// 地图保存的数据结构;
        /// </summary>
        private Dictionary<ShortVector2, IGameMap<ShortVector2, T>> mapCollection;

        /// <summary>
        /// 动态地图文件输出输入;
        /// </summary>
        private IDynamicMapIO<DynamicMapPaging<T>, T> dynamicMapIO;

        /// <summary>
        /// 上一次更新目标所在的地图块;
        /// </summary>
        private ShortVector2 lastUpdateTargetAddress;


        /// <summary>
        /// 地图动态读取信息,地图块信息;
        /// </summary>
        public DynamicPagingInfo PagingInfo
        {
            get { return pagingInfo; }
        }

        /// <summary>
        /// 目标辐射范围,既以为目标中心,需要读取的地图范围;
        /// </summary>
        private ShortVector2 TargetRadiationRange
        {
            get { return pagingInfo.TargetRadiationRange; }
        }

        /// <summary>
        /// 地图分区大小;
        /// </summary>
        private ShortVector2 PartitionSizes
        {
            get { return pagingInfo.PartitionSizes; }
        }

        /// <summary>
        /// 未知是否为空!始终返回false;
        /// </summary>
        public bool IsEmpty
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
        public T GetItem(IntVector2 position)
        {
            ShortVector2 realPosition;
            ShortVector2 address = TransfromToAddress(position, out realPosition);
            IGameMap<ShortVector2, T> mapPaging = mapCollection[address];
            return mapPaging[realPosition];
        }

        /// <summary>
        /// 设置这个值;若不存在则返回异常;
        /// KeyNotFoundException : 这个点不存在,或超出读取范围;
        /// </summary>
        public void SetItem(IntVector2 position, T item)
        {
            ShortVector2 realPosition;
            ShortVector2 address = TransfromToAddress(position, out realPosition);
            IGameMap<ShortVector2, T> mapPaging = mapCollection[address];
            mapPaging[realPosition] = item;
        }

        /// <summary>
        /// 将这个元素加入到地图,若无法保存则返回异常;
        /// KeyNotFoundException : 超出范围;
        /// </summary>
        public void Add(IntVector2 position, T item)
        {
            ShortVector2 realPosition;
            ShortVector2 address = TransfromToAddress(position, out realPosition);
            IGameMap<ShortVector2, T> mapPaging = mapCollection[address];
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
            IGameMap<ShortVector2, T> mapPaging = mapCollection[address];
            return mapPaging.Remove(realPosition);
        }

        /// <summary>
        /// 确认地图是否存在这个点;
        /// </summary>
        public bool ContainsPosition(IntVector2 position)
        {
            IGameMap<ShortVector2, T> mapPaging;
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
            IGameMap<ShortVector2, T> mapPaging;
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

        /// <summary>
        /// 根据目标所在位置更新地图数据;
        /// 若目标所在地块位置与上次更新相同,则不做更新(除非 check 为false);
        /// </summary>
        public void UpdateMapData(IntVector2 targetPosition, bool check = true)
        {
            ShortVector2 targetAddress = TransfromToAddress(targetPosition);

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
            IEnumerable<ShortVector2> radiationAddresses = GetRadiationAddresses(targetAddress);
            Unload(radiationAddresses);
            Load(radiationAddresses);
        }

        /// <summary>
        /// 根据分页地址加入到地图;
        /// </summary>
        private void Load(IEnumerable<ShortVector2> radiationAddresses)
        {
            IEnumerable<ShortVector2> addRadiationAddresses = radiationAddresses.
                Where(address => !mapCollection.ContainsKey(address));
            foreach (var address in addRadiationAddresses)
            {
                Load(address);
            }
        }

        /// <summary>
        /// 根据分页地址从地图内移除;
        /// </summary>
        private void Unload(IEnumerable<ShortVector2> radiationAddresses)
        {
            IEnumerable<ShortVector2> removeRadiationAddresses = mapCollection.Keys.
                Where(address => !radiationAddresses.Contains(address));
            foreach (var address in removeRadiationAddresses)
            {
                Unload(address);
            }
        }

        /// <summary>
        /// 获取到这个分页,并且加入到地图;
        /// </summary>
        /// <param name="address"></param>
        private void Load(ShortVector2 address)
        {
            IGameMap<ShortVector2, T> mapPaging;
            if (dynamicMapIO.TryLoad(address, out mapPaging))
            {
                mapCollection.Add(address, mapPaging);
            }
            return;
        }

        /// <summary>
        /// 移除这个地图块;
        /// 移除成功返回true,否则返回false;
        /// </summary>
        /// <param name="targetAddress"></param>
        private bool Unload(ShortVector2 address)
        {
            IGameMap<ShortVector2, T> mapPaging;
            if (mapCollection.TryGetValue(address, out mapPaging))
            {
                SaveMapPaging(mapPaging);
                return mapCollection.Remove(address);
            }
            return false;
        }

        /// <summary>
        /// 保存地图所有的地图块;
        /// ReadOnlyException : 不允许编辑的
        /// </summary>
        /// <param name="compulsorySave"></param>
        public void SaveMapPagingAll()
        {
            foreach (var mapPaging in mapCollection.Values)
            {
                SaveMapPaging(mapPaging);
            }
        }

        /// <summary>
        /// 保存这个地图块若地图块内不存在内容,则删除这个地图块;
        /// </summary>
        private void SaveMapPaging(IGameMap<ShortVector2, T> mapPaging)
        {
            if (!mapPaging.IsEmpty)
            {
                dynamicMapIO.Save(mapPaging);
            }
            else
            {
                dynamicMapIO.Delete(mapPaging.Address);
            }
        }

        /// <summary>
        /// 获取到目标辐射到的地图分页地址;
        /// </summary>
        private IEnumerable<ShortVector2> GetRadiationAddresses(IntVector2 targetPosition)
        {
            ShortVector2 address = TransfromToAddress(targetPosition);
            return GetRadiationAddresses(address);
        }

        /// <summary>
        /// 获取到目标辐射到的地图分页地址;
        /// </summary>
        private IEnumerable<ShortVector2> GetRadiationAddresses(ShortVector2 address)
        {
            ShortVector2 targetRadiationRange = TargetRadiationRange;

            for (short x = (short)(-targetRadiationRange.x); x <= targetRadiationRange.x; x++)
            {
                for (short y = (short)(-targetRadiationRange.y); y <= targetRadiationRange.y; y++)
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
