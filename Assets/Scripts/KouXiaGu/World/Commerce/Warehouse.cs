using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{


    /// <summary>
    /// 存放\记录 一类型产品的数目;
    /// </summary>
    public class Warehouse : IEnumerable<Warehouse.Room>
    {

        const int DefaultNumber = 0;


        public Warehouse(ProductCategorie categorie)
        {
            if (!categorie.IsStorable)
                throw new ArgumentException();

            this.Categorie = categorie;
            this.Rooms = new List<Room>();
            this.Number = DefaultNumber;
        }


        /// <summary>
        /// 存放的资源类型;
        /// </summary>
        public ProductCategorie Categorie { get; private set; }

        /// <summary>
        /// 资源库房;
        /// </summary>
        public List<Room> Rooms { get; private set; }

        /// <summary>
        /// 总数;
        /// </summary>
        public int Number { get; private set; }


        /// <summary>
        /// 获取到资源对应的库房;
        /// </summary>
        public Room Find(Product type)
        {
            return Rooms.Find(item => item.Type == type);
        }

        /// <summary>
        /// 获取到资源对应的库房,若不存在则创建一个;
        /// </summary>
        public Room FindOrCreate(Product type)
        {
            var room = Rooms.Find(item => item.Type == type);

            if (room == null)
            {
                room = new Room(type, this);
                Rooms.Add(room);
            }

            return room;
        }

        /// <summary>
        /// 移除这类产品数目,并返回未能移除的数目;
        /// </summary>
        public int RemoveProduct(int number)
        {
            foreach (var productRoom in Rooms)
            {
                number = productRoom.RemoveProduct(number);

                if (number == 0)
                    return number;
            }
            return number;
        }

        /// <summary>
        /// 尝试移除这类产品数目,若无法移除则返回false,移除成功返回true;
        /// </summary>
        public bool TryRemoveProduct(int number)
        {
            if (Number < number)
                return false;

            RemoveProduct(number);
            return true;
        }

        /// <summary>
        /// 移除所有空的房间,返回 移除的元素数;
        /// </summary>
        public int RemoveEmptyRooms()
        {
            return Rooms.RemoveAll(room => room.IsEmpty);
        }


        public IEnumerator<Room> GetEnumerator()
        {
            return Rooms.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Rooms.GetEnumerator();
        }


        /// <summary>
        /// 存放\记录 产品的数目;
        /// </summary>
        public class Room : StorageRoom
        {

            const int DefaultProductNumber = 0;


            internal Room(Product type, Warehouse warehouse)
            {
                this.Type = type;
                this.Warehouse = warehouse;
                this.Number = DefaultProductNumber;
            }


            public Product Type { get; private set; }
            public Warehouse Warehouse { get; private set; }
            public int Number { get; private set; }


            /// <summary>
            /// 房间是否为空?
            /// </summary>
            public override bool IsEmpty
            {
                get { return base.IsEmpty && Number == 0; }
            }


            /// <summary>
            /// 增加或减少产品数目;
            /// 若减少数目大于已存在数,那结果都为0;
            /// </summary>
            [Obsolete]
            public void ChangeProduct(int increment)
            {
                int result = this.Number + increment;

                if (result < 0)
                {
                    Warehouse.Number -= Number;
                    Number = 0;
                }
                else
                {
                    Warehouse.Number += increment;
                    Number = result;
                }
            }

            /// <summary>
            /// 增加产品数目; number 大于或等于 0;
            /// </summary>
            public void AddProduct(int number)
            {
                Number += number;
                Warehouse.Number += number;
            }

            /// <summary>
            /// 移除产品数目,并返回未能移除的数目; number 大于或等于 0;
            /// </summary>
            public int RemoveProduct(int number)
            {
                int result = Number - number;

                if (result <= 0)
                {
                    Warehouse.Number -= Number;
                    Number = 0;
                    return Math.Abs(result);
                }
                else
                {
                    Number = result;
                    Warehouse.Number -= number;
                    return 0;
                }
            }

            /// <summary>
            /// 尝试移除这类产品数目,若无法移除则返回false,移除成功返回true;
            /// </summary>
            public bool TryRemoveProduct(int number)
            {
                if (Number < number)
                    return false;

                RemoveProduct(number);
                return true;
            }

        }

    }

}
