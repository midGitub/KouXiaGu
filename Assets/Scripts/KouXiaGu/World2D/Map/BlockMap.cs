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
        protected BlockMap() { }

        public virtual void Awake()
        {
            mapCollection = new Dictionary<ShortVector2, TBlock>();
        }


        [SerializeField]
        protected ShortVector2 partitionSizes;
        private Dictionary<ShortVector2, TBlock> mapCollection;
        /// <summary>
        /// 线程锁;
        /// </summary>
        protected object thisLock = new object();

        public Dictionary<ShortVector2, TBlock> MapCollection
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
                    return;
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
                return;
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
        /// 将地图坐标转换成地图块的坐标;
        /// </summary>
        public ShortVector2 GetAddress(IntVector2 position)
        {
            ShortVector2 address = new ShortVector2();

            address.x = (short)(position.x / partitionSizes.x);
            address.y = (short)(position.y / partitionSizes.y);

            return address;
        }

        /// <summary>
        /// 将地图坐标转换成地图块的坐标 和 地图块内的坐标;
        /// </summary>
        public ShortVector2 GetAddress(IntVector2 position, out ShortVector2 realPosition)
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
        public IntVector2 AddressToPosition(ShortVector2 address, ShortVector2 realPosition)
        {
            IntVector2 position = new IntVector2();

            position.x = address.x * partitionSizes.x + realPosition.x;
            position.y = address.y * partitionSizes.y + realPosition.y;

            return position;
        }

    }

}
