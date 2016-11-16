using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 根据块加载的地图;
    /// </summary>
    [Serializable]
    public abstract class BlockMap<T, TBlock> : IMap<IntVector2, T>
        where TBlock : IMap<ShortVector2, T>
    {
        public BlockMap(
            ShortVector2 partitionSizes, 
            ShortVector2 minRadiationRange, 
            ShortVector2 maxRadiationRange)
        {
            this.partitionSizes = ShortVector2.Abs(partitionSizes);
            this.minRadiationRange = ShortVector2.Abs(minRadiationRange);
            this.maxRadiationRange = ShortVector2.Abs(maxRadiationRange);
            mapCollection = new Dictionary<ShortVector2, TBlock>();
        }


        [SerializeField, Header("地图块信息")]
        private ShortVector2 partitionSizes;
        [SerializeField]
        private ShortVector2 minRadiationRange;
        [SerializeField]
        private ShortVector2 maxRadiationRange;
        private Dictionary<ShortVector2, TBlock> mapCollection;
        private ShortVector2 lastUpdateCenterAddress;

        protected Dictionary<ShortVector2, TBlock> MapCollection
        {
            get { return mapCollection; }
        }


        public T this[IntVector2 position]
        {
            get
            {
                ShortVector2 realPosition;
                ShortVector2 address = GetAddress(position, out realPosition);
                TBlock block;

                if (mapCollection.TryGetValue(address, out block))
                {
                    return block[realPosition];
                }
                throw BlockNotFoundException(address);
            }
            set
            {
                ShortVector2 realPosition;
                ShortVector2 address = GetAddress(position, out realPosition);
                TBlock block;

                if (mapCollection.TryGetValue(address, out block))
                {
                    block[realPosition] = value;
                }
                throw BlockNotFoundException(address);
            }
        }
   
        public void Add(IntVector2 position, T item)
        {
            ShortVector2 realPosition;
            ShortVector2 address = GetAddress(position, out realPosition);
            TBlock block;

            if (mapCollection.TryGetValue(address, out block))
            {
                block.Add(realPosition, item);
            }
            throw BlockNotFoundException(address);
        }

        public bool Remove(IntVector2 position)
        {
            ShortVector2 realPosition;
            ShortVector2 address = GetAddress(position, out realPosition);
            TBlock block;

            if (mapCollection.TryGetValue(address, out block))
            {
               return block.Remove(realPosition);
            }
            throw BlockNotFoundException(address);
        }

        public bool Contains(IntVector2 position)
        {
            ShortVector2 realPosition;
            ShortVector2 address = GetAddress(position, out realPosition);
            TBlock block;

            if (mapCollection.TryGetValue(address, out block))
            {
                return block.Contains(realPosition);
            }
            throw BlockNotFoundException(address);
        }

        public bool TryGetValue(IntVector2 position, out TBlock item)
        {
            ShortVector2 address = GetAddress(position);
            return mapCollection.TryGetValue(address, out item);
        }

        public bool TryGetValue(IntVector2 position, out T item)
        {
            ShortVector2 realPosition;
            ShortVector2 address = GetAddress(position, out realPosition);
            TBlock block;

            if (mapCollection.TryGetValue(address, out block))
            {
                return block.TryGetValue(realPosition, out item);
            }
            throw BlockNotFoundException(address);
        }

        public void Clear()
        {
            mapCollection.Clear();
        }

        /// <summary>
        /// 返回地图块错误信息;
        /// </summary>
        private BlockNotFoundException BlockNotFoundException(ShortVector2 address)
        {
            return new BlockNotFoundException(address.ToString() + "地图块未载入!\n" +
                mapCollection.Keys.ToString());
        }


        /// <summary>
        /// 获取到这个地图块;
        /// </summary>
        protected abstract TBlock GetBlock(ShortVector2 blockAddress);
        /// <summary>
        /// 释放这个块的使用;
        /// </summary>
        protected abstract void ReleaseBlock(ShortVector2 blockAddress, TBlock block);


        public void UpdateBlock(IntVector2 position, bool check = true)
        {
            ShortVector2 targetAddress = GetAddress(position);

            if (this.lastUpdateCenterAddress != targetAddress || !check)
            {
                UpdateBlocks(targetAddress);
                this.lastUpdateCenterAddress = targetAddress;
            }
        }

        private void UpdateBlocks(ShortVector2 address)
        {
            CheckUnloadBlocks(address);
            CheckLoadBlocks(address);
        }

        /// <summary>
        /// 根据分页地址加入到地图;
        /// </summary>
        private void CheckLoadBlocks(ShortVector2 address)
        {
            IEnumerable<ShortVector2> radiationAddresses = GetMinRadiationAddresses(address);
            IEnumerable<ShortVector2> addRadiationAddresses = radiationAddresses.
                Where(loadedAddress => !mapCollection.ContainsKey(loadedAddress));

            foreach (var addAddress in addRadiationAddresses)
            {
                LoadBlock(addAddress);
            }
        }

        /// <summary>
        /// 根据分页地址从地图内移除;根据最大辐射移除;
        /// </summary>
        private void CheckUnloadBlocks(ShortVector2 address)
        {
            IEnumerable<ShortVector2> radiationAddresses = GetMaxRadiationAddresses(address);
            ShortVector2[] removeAddresses = mapCollection.Keys.
                Where(loadedAddress => !radiationAddresses.Contains(loadedAddress)).ToArray();
            foreach (var removeAddress in removeAddresses)
            {
                UnloadBlock(removeAddress);
            }
        }

        private void LoadBlock(ShortVector2 address)
        {
            TBlock mapBlock = GetBlock(address);
            mapCollection.Add(address, mapBlock);
        }

        private void UnloadBlock(ShortVector2 address)
        {
            TBlock mapBlock;
            if (mapCollection.TryGetValue(address, out mapBlock))
            {
                ReleaseBlock(address, mapBlock);
                mapCollection.Remove(address);
            }
        }

        /// <summary>
        /// 获取到目标辐射到的最大范围;
        /// </summary>
        private IEnumerable<ShortVector2> GetMaxRadiationAddresses(ShortVector2 address)
        {
            return GetRadiationAddresses(address, maxRadiationRange);
        }

        /// <summary>
        /// 获取到目标辐射到的最小范围;
        /// </summary>
        private IEnumerable<ShortVector2> GetMinRadiationAddresses(ShortVector2 address)
        {
            return GetRadiationAddresses(address, minRadiationRange);
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
        protected ShortVector2 GetAddress(IntVector2 position)
        {
            ShortVector2 address = new ShortVector2();

            address.x = (short)(position.x / partitionSizes.x);
            address.y = (short)(position.y / partitionSizes.y);

            return address;
        }

        /// <summary>
        /// 将地图坐标转换成地图块的坐标 和 地图块内的坐标;
        /// </summary>
        protected ShortVector2 GetAddress(IntVector2 position, out ShortVector2 realPosition)
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
        protected IntVector2 AddressToPosition(ShortVector2 address, ShortVector2 realPosition)
        {
            IntVector2 position = new IntVector2();

            position.x = address.x * partitionSizes.x + realPosition.x;
            position.y = address.y * partitionSizes.y + realPosition.y;

            return position;
        }

    }

}
