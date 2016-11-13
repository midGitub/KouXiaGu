using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 地图块,分页信息;
    /// </summary>
    [Serializable]
    public struct PagingInfo
    {
        public PagingInfo(string dataDirectoryPath, ShortVector2 partitionSizes, 
            ShortVector2 targetRadiationRange, string addressPrefix)
        {
            this.dataDirectoryPath = dataDirectoryPath;
            this.partitionSizes = partitionSizes;
            this.targetRadiationRange = targetRadiationRange;
            this.addressPrefix = addressPrefix;
        }

        [SerializeField]
        private string dataDirectoryPath;
        [SerializeField]
        private ShortVector2 partitionSizes;
        [SerializeField]
        private ShortVector2 targetRadiationRange;
        [SerializeField]
        private string addressPrefix;

        /// <summary>
        /// 地图文件路径;
        /// </summary>
        public string DataDirectoryPath
        {
            get { return dataDirectoryPath; }
        }

        /// <summary>
        /// 地图分区大小;
        /// </summary>
        public ShortVector2 PartitionSizes
        {
            get { return partitionSizes; }
        }

        /// <summary>
        /// 目标辐射范围,既以为目标中心,需要读取的地图范围;
        /// X,Y应该都为正数;
        /// </summary>
        public ShortVector2 TargetRadiationRange
        {
            get { return targetRadiationRange; }
        }

        /// <summary>
        /// 获取到这个地图块的读取路径;
        /// </summary>
        public string GetMapPagingFilePath(ShortVector2 address)
        {
            return addressPrefix + address.GetHashCode();
        }

        ///// <summary>
        ///// 获取到若有地图块地址;
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<ShortVector2> GetAllAddress()
        //{
        //    throw new NotImplementedException();
        //}

    }


    /// <summary>
    /// 动态地图数据结构;
    /// 因为是动态加载的,所有取值和赋值时需要检查是否在目标范围内;
    /// </summary>
    public class DynamicMapDictionary<T> : IReadOnlyMap<T>
    {
        public DynamicMapDictionary(PagingInfo dynamicMapInfo, bool allowEdit = true)
        {
            this.pagingInfo = dynamicMapInfo;
            this.allowEdit = allowEdit;
            mapCollection = new Dictionary<ShortVector2, MapPaging>();
        }

        /// <summary>
        /// 地图动态读取信息,地图块信息;
        /// </summary>
        private PagingInfo pagingInfo;

        /// <summary>
        /// 地图保存的数据结构;
        /// </summary>
        private Dictionary<ShortVector2, MapPaging> mapCollection;

        /// <summary>
        /// 在卸载地图资源时进行保存;
        /// </summary>
        private bool allowEdit;

        /// <summary>
        /// 上一次更新目标所在的地图块;
        /// </summary>
        private ShortVector2 lastUpdateTargetAddress;


        /// <summary>
        /// 地图动态读取信息,地图块信息;
        /// </summary>
        public PagingInfo PagingInfo
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
        /// 在卸载地图资源时进行保存;
        /// </summary>
        public bool AllowEdit
        {
            get { return allowEdit; }
            set { allowEdit = value; }
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
            MapPaging mapPaging = mapCollection[address];
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
            MapPaging mapPaging = mapCollection[address];
            mapPaging[realPosition] = item;
        }

        /// <summary>
        /// 将这个元素加入到地图,若无法保存则返回异常;
        /// ReadOnlyException : 这是一个不允许编辑的地图;
        /// KeyNotFoundException : 超出范围;
        /// </summary>
        public void Add(IntVector2 position, T item)
        {
            if (!allowEdit)
                throw new ReadOnlyException();

            ShortVector2 realPosition;
            ShortVector2 address = TransfromToAddress(position, out realPosition);
            MapPaging mapPaging = mapCollection[address];
            mapPaging.Add(realPosition, item);
        }

        /// <summary>
        /// 从地图上移除这个元素;
        /// ReadOnlyException : 这是一个不允许编辑的地图;
        /// KeyNotFoundException : 超出范围;
        /// </summary>
        public void Remove(IntVector2 position)
        {
            if (!allowEdit)
                throw new ReadOnlyException();

            ShortVector2 realPosition;
            ShortVector2 address = TransfromToAddress(position, out realPosition);
            MapPaging mapPaging = mapCollection[address];
            mapPaging.Remove(realPosition);
        }

        /// <summary>
        /// 确认地图是否存在这个点;
        /// </summary>
        public bool ContainsPosition(IntVector2 position)
        {
            MapPaging mapPaging;
            ShortVector2 realPosition;
            ShortVector2 address = TransfromToAddress(position, out realPosition);
            if (mapCollection.TryGetValue(address, out mapPaging))
            {
                return mapPaging.ContainsKey(realPosition);
            }
            return false;
        }

        /// <summary>
        /// 尝试获取到这个点的元素;
        /// </summary>
        public bool TryGetValue(IntVector2 position, out T item)
        {
            MapPaging mapPaging;
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
            IEnumerable<ShortVector2> addRadiationAddresses = radiationAddresses.
                Where(address => !mapCollection.ContainsKey(address));
            IEnumerable<ShortVector2> removeRadiationAddresses = mapCollection.Keys.
                Where(address => !radiationAddresses.Contains(address));

            Unload(removeRadiationAddresses);
            Load(addRadiationAddresses);
        }

        /// <summary>
        /// 根据分页地址加入到地图;
        /// </summary>
        private void Load(IEnumerable<ShortVector2> addRadiationAddresses)
        {
            foreach (var address in addRadiationAddresses)
            {
                Load(address);
            }
        }

        /// <summary>
        /// 根据分页地址从地图内移除;
        /// </summary>
        private void Unload(IEnumerable<ShortVector2> removeRadiationAddresses)
        {
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
            try
            {
                MapPaging mapPaging = LoadMapPaging(address);
                mapCollection.Add(address, mapPaging);
            }
            catch (FileNotFoundException)
            {
                Debug.Log("不存在分页 " + address + ",跳过;");
            }
        }

        /// <summary>
        /// 从文件读取到地图分页.若无法获取返回异常;
        /// FileNotFoundException : 不存在此分页;
        /// </summary>
        private MapPaging LoadMapPaging(ShortVector2 address)
        {
            string mapPagingFilePath = GetMapPagingFilePath(address);
            return SerializeHelper.Deserialize_ProtoBuf<MapPaging>(mapPagingFilePath);
        }

        /// <summary>
        /// 移除这个地图块,不做保存的移除(除非 compulsorySave 设为true则不管怎样都会保存);
        /// 移除成功返回true,否则返回false;
        /// </summary>
        /// <param name="targetAddress"></param>
        private bool Unload(ShortVector2 address, bool compulsorySave = false)
        {
            MapPaging mapPaging;
            if (mapCollection.TryGetValue(address, out mapPaging))
            {
                SaveMapPaging(mapPaging, compulsorySave);
                return mapCollection.Remove(address);
            }
            return false;
        }

        /// <summary>
        /// 若允许保存,则进行保存(除非 compulsorySave 设为true则不管怎样都会保存);
        /// </summary>
        private void SaveMapPaging(MapPaging mapPaging, bool compulsorySave = false)
        {
            if (allowEdit || compulsorySave)
            {
                string mapPagingFilePath = GetMapPagingFilePath(mapPaging.Address);
                SerializeHelper.Serialize_ProtoBuf(mapPagingFilePath, mapPaging);
            }
        }

        /// <summary>
        /// 获取到地图块保存到的文件路径
        /// </summary>
        private string GetMapPagingFilePath(ShortVector2 address)
        {
            return pagingInfo.GetMapPagingFilePath(address);
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

            for (short x = (short)(-targetRadiationRange.x); x < targetRadiationRange.x; x++)
            {
                for (short y = (short)(-targetRadiationRange.y); y < targetRadiationRange.y; y++)
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

        [ProtoContract]
        private struct MapPaging : IDictionary<ShortVector2, T>
        {
            public MapPaging(ShortVector2 address, Dictionary<ShortVector2, T> dictionary)
            {
                this.address = address;
                this.dictionary = dictionary;
            }

            /// <summary>
            /// 地图块的坐标;
            /// </summary>
            [ProtoMember(1)]
            private ShortVector2 address;

            /// <summary>
            /// 这个分页保存的信息;
            /// </summary>
            [ProtoMember(2)]
            private Dictionary<ShortVector2, T> dictionary;

            /// <summary>
            /// 地图块的坐标;
            /// </summary>
            public ShortVector2 Address
            {
                get { return address; }
            }

            #region IDictionary<ShortVector2, T>;

            public ICollection<ShortVector2> Keys
            {
                get{ return ((IDictionary<ShortVector2, T>)this.dictionary).Keys; }
            }

            public ICollection<T> Values
            {
                get { return ((IDictionary<ShortVector2, T>)this.dictionary).Values; }
            }

            public int Count
            {
                get { return ((IDictionary<ShortVector2, T>)this.dictionary).Count; }
            }

            public bool IsReadOnly
            {
                get { return ((IDictionary<ShortVector2, T>)this.dictionary).IsReadOnly; }
            }

            public T this[ShortVector2 key]
            {
                get { return ((IDictionary<ShortVector2, T>)this.dictionary)[key]; }
                set { ((IDictionary<ShortVector2, T>)this.dictionary)[key] = value; }
            }

            public void Add(ShortVector2 key, T value)
            {
                ((IDictionary<ShortVector2, T>)this.dictionary).Add(key, value);
            }

            public bool ContainsKey(ShortVector2 key)
            {
                return ((IDictionary<ShortVector2, T>)this.dictionary).ContainsKey(key);
            }

            public bool Remove(ShortVector2 key)
            {
                return ((IDictionary<ShortVector2, T>)this.dictionary).Remove(key);
            }

            public bool TryGetValue(ShortVector2 key, out T value)
            {
                return ((IDictionary<ShortVector2, T>)this.dictionary).TryGetValue(key, out value);
            }

            public void Add(KeyValuePair<ShortVector2, T> item)
            {
                ((IDictionary<ShortVector2, T>)this.dictionary).Add(item);
            }

            public void Clear()
            {
                ((IDictionary<ShortVector2, T>)this.dictionary).Clear();
            }

            public bool Contains(KeyValuePair<ShortVector2, T> item)
            {
                return ((IDictionary<ShortVector2, T>)this.dictionary).Contains(item);
            }

            public void CopyTo(KeyValuePair<ShortVector2, T>[] array, int arrayIndex)
            {
                ((IDictionary<ShortVector2, T>)this.dictionary).CopyTo(array, arrayIndex);
            }

            public bool Remove(KeyValuePair<ShortVector2, T> item)
            {
                return ((IDictionary<ShortVector2, T>)this.dictionary).Remove(item);
            }

            public IEnumerator<KeyValuePair<ShortVector2, T>> GetEnumerator()
            {
                return ((IDictionary<ShortVector2, T>)this.dictionary).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IDictionary<ShortVector2, T>)this.dictionary).GetEnumerator();
            }

            #endregion

            public override string ToString()
            {
                return base.ToString() +
                    "\n地图块地址:" + address.ToString()+
                    "\n元素个数:" + dictionary.Count;
            }

            public override int GetHashCode()
            {
                return address.GetHashCode();
            }

        }

    }

}
