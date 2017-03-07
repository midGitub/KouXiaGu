using System;
using System.Collections;
using System.Collections.Generic;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 存放\记录 一类型产品的数目;
    /// </summary>
    public class ProductHouse : IEnumerable<ProductRoom>
    {

        public ProductHouse()
        {
            this.Rooms = new List<ProductRoom>();
        }

        /// <summary>
        /// 资源库房;
        /// </summary>
        public List<ProductRoom> Rooms { get; private set; }

        /// <summary>
        /// 获取到资源对应的库房;
        /// </summary>
        public ProductRoom Find(Product productType)
        {
            return Rooms.Find(item => item.ProductType == productType);
        }

        /// <summary>
        /// 获取到资源对应的库房,若不存在则创建一个;
        /// </summary>
        public ProductRoom FindOrCreate(Product productType)
        {
            var room = Rooms.Find(item => item.ProductType == productType);

            if (room == null)
            {
                room = new ProductRoom(productType, this);
                Rooms.Add(room);
            }

            return room;
        }


        /// <summary>
        /// 尝试摧毁这个产品的房间,若摧毁成功返回true;若未找到,或被引用,则返回false;
        /// </summary>
        public bool TryDestroy(Product productType)
        {
            int index = Rooms.FindIndex(room => room.ProductType == productType);

            if (index == -1)
            {
                return false;
            }

            var productRoom = Rooms[index];
            if (productRoom.IsRemovable())
            {
                Rooms.RemoveAt(index);
                return true;
            }

            return false;
        }


        /// <summary>
        /// 每日更新 ,更新所有存储产品数量;
        /// </summary>
        public void DayUpdate()
        {
            Rooms.RemoveAll(DayUpdateAndRemove);
        }

        /// <summary>
        /// 更新产品数量,并且返回是否需要移除;
        /// </summary>
        bool DayUpdateAndRemove(ProductRoom room)
        {
            room.DayUpdate();
            return room.IsEmpty && room.IsRemovable();
        }


        public IEnumerator<ProductRoom> GetEnumerator()
        {
            return Rooms.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Rooms.GetEnumerator();
        }

    }

}
