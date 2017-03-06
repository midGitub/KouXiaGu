using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    ///// <summary>
    ///// 整合仓库内容;
    ///// </summary>
    //public class Storage
    //{

    //    public Storage()
    //    {
    //        warehouses = new List<Warehouse>();
    //    }


    //    List<Warehouse> warehouses;


    //    /// <summary>
    //    /// 获取到这个类型的库房;若不存在则返回 null;
    //    /// </summary>
    //    public Warehouse Find(int categorie)
    //    {
    //        return warehouses.Find(item => item.Categorie == categorie);
    //    }

    //    /// <summary>
    //    /// 获取到这个类型的库房索引值;若不存在则返回 -1;
    //    /// </summary>
    //    public int FindIndex(int categorie)
    //    {
    //        return warehouses.FindIndex(item => item.Categorie == categorie);
    //    }

    //    /// <summary>
    //    /// 获取到这个类型的库房;若不存在则创建并返回;
    //    /// </summary>
    //    public Warehouse FindOrCreate(int categorie)
    //    {
    //        var house = warehouses.Find(item => item.Categorie == categorie);

    //        if (house == null)
    //        {
    //            house = new Warehouse(categorie);
    //            warehouses.Add(house);
    //        }

    //        return house;
    //    }

    //    /// <summary>
    //    /// 尝试摧毁这个仓库,若不存在引用则摧毁,返回true;若不存在,或者存在引用,则不摧毁返回false;
    //    /// </summary>
    //    public bool TryDestroy(int categorie)
    //    {
    //        var roomIndex = FindIndex(categorie);

    //        if (roomIndex != -1 && !warehouses[roomIndex].ExistsReference())
    //        {
    //            warehouses.RemoveAt(roomIndex);
    //            return true;
    //        }

    //        return false;
    //    }

    //    /// <summary>
    //    /// 移除所有空的库房,并返回移除数目;
    //    /// </summary>
    //    public int RemoveEmptyAll()
    //    {
    //        return warehouses.RemoveAll(item => !item.ExistsReference() && item.Total == 0);
    //    }

    //}

    
    /// <summary>
    /// 对资源进行分类的组;
    /// </summary>
    public interface ICategorieGroup
    {

    }

    /// <summary>
    /// 存放\记录 一类型产品的数目;
    /// </summary>
    public class Warehouse : IEnumerable<ProductRoom>
    {

        public Warehouse()
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
        public ProductRoom Find(int productType)
        {
            return Rooms.Find(item => item.ProductType == productType);
        }

        /// <summary>
        /// 获取到资源对应的库房,若不存在则创建一个;
        /// </summary>
        public ProductRoom FindOrCreate(int productType)
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
        public bool TryDestroy(int productType)
        {
            int index = Rooms.FindIndex(room => room.ProductType == productType);

            if (index == -1)
            {
                return false;
            }

            var productRoom = Rooms[index];
            if (!productRoom.ExistsReference)
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
            return room.IsEmpty && !room.ExistsReference;
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


    /// <summary>
    /// 存放\记录 产品的数目;
    /// </summary>
    public class ProductRoom : StorageRoom
    {

        public ProductRoom(int productType, Warehouse warehouse)
        {
            this.ProductType = productType;
            this.House = warehouse;
            this.Total = 0;
        }


        /// <summary>
        /// 产品类型;
        /// </summary>
        public int ProductType { get; private set; }

        /// <summary>
        /// 归属;
        /// </summary>
        public Warehouse House { get; private set; }

        /// <summary>
        /// 资源总数;
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// 房间是否为空?
        /// </summary>
        public bool IsEmpty
        {
            get { return Total == 0; }
        }


        /// <summary>
        /// 增加产品数目; number 大于或等于 0;
        /// </summary>
        public void AddProduct(int number)
        {
            Total += number;
        }

        /// <summary>
        /// 移除产品数目,并返回未能移除的数目; number 大于或等于 0;
        /// </summary>
        public int RemoveProduct(int number)
        {
            if (number > Total)
            {
                int result = Total - number;
                Total = 0;
                return Math.Abs(result);
            }
            else
            {
                Total -= number;
                return 0;
            }
        }

        /// <summary>
        /// 尝试移除这类产品数目,若无法移除则返回false,移除成功返回true;
        /// </summary>
        public bool TryRemoveProduct(int number)
        {
            if (Total < number)
                return false;

            Total -= number;
            return true;
        }


        public void DayUpdate()
        {

        }

        /// <summary>
        /// 产品存储时的影响因素;
        /// </summary>
        public class InfluencingFactor
        {

        }

    }

}
