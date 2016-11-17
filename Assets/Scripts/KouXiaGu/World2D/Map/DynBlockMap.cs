using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KouXiaGu.World2D
{


    public abstract class DynBlockMapEx<T, TRead, TBlock> : DynBlockMap<T, TBlock>, IReadOnlyMap<IntVector2, TRead>
        where T : class, TRead
        where TRead : class, IReadOnly<T>
        where TBlock : IMap<ShortVector2, T>, IReadOnlyMap<ShortVector2, TRead>
    {
        protected DynBlockMapEx() { }

        TRead IReadOnlyMap<IntVector2, TRead>.this[IntVector2 position]
        {
            get { return base[position]; }
        }

        bool IReadOnlyMap<IntVector2, TRead>.TryGetValue(IntVector2 position, out TRead item)
        {
            T worldNode;
            if (base.TryGetValue(position, out worldNode))
            {
                item = worldNode;
                return true;
            }
            item = default(TRead);
            return false;
        }

    }


    /// <summary>
    /// 动态读取的分块地图;
    /// </summary>
    public abstract class DynBlockMap<T, TBlock> : BlockMap<T, TBlock>
        where TBlock : IMap<ShortVector2, T>
    {
        protected DynBlockMap() { }

        [SerializeField]
        private ShortVector2 minRadiationRange;
        [SerializeField]
        private ShortVector2 maxRadiationRange;
        private ShortVector2 lastUpdateCenterAddress;

        public abstract TBlock GetBlock(ShortVector2 blockAddress);
        public abstract void ReleaseBlock(ShortVector2 blockAddress, TBlock block);

        /// <summary>
        /// 是否有必要更新;
        /// </summary>
        protected bool NeedToUpdate(IntVector2 position, out ShortVector2 address)
        {
            address = GetAddress(position);
            return this.lastUpdateCenterAddress != address || MapCollection.Count == 0;
        }

        protected void UpdateBlocks(IntVector2 position)
        {
            ShortVector2 address;

            if (NeedToUpdate(position, out address))
            {
                UpdateBlocks(address);
            }
        }

        protected void UpdateBlocks(ShortVector2 address)
        {
            this.lastUpdateCenterAddress = address;
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
                Where(loadedAddress => !MapCollection.ContainsKey(loadedAddress));

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
            ShortVector2[] removeAddresses = MapCollection.Keys.
                Where(loadedAddress => !radiationAddresses.Contains(loadedAddress)).ToArray();
            foreach (var removeAddress in removeAddresses)
            {
                UnloadBlock(removeAddress);
            }
        }

        private void LoadBlock(ShortVector2 address)
        {
            TBlock mapBlock = GetBlock(address);
            MapCollection.Add(address, mapBlock);
        }

        private void UnloadBlock(ShortVector2 address)
        {
            TBlock mapBlock;
            if (MapCollection.TryGetValue(address, out mapBlock))
            {
                ReleaseBlock(address, mapBlock);
                MapCollection.Remove(address);
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

    }

}
