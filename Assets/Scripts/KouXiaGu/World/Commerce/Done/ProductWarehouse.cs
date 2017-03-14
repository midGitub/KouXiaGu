using System;
using System.Collections.Generic;

namespace KouXiaGu.World.Commerce
{

    public interface ICategorieWareroom
    {
        /// <summary>
        /// 存储量;
        /// </summary>
        int Total { get; }

        /// <summary>
        /// 产品类别;
        /// </summary>
        ProductCategorie Categorie { get; }

        /// <summary>
        /// 移除产品数目,并返回未能移除的数目; number 大于或等于 0;
        /// </summary>
        int Remove(int number);

        /// <summary>
        /// 尝试移除这类产品数目,若无法移除则返回false,移除成功返回true;
        /// </summary>
        bool TryRemove(int number);
    }

    public interface IWareroom
    {
        /// <summary>
        /// 存储量;
        /// </summary>
        int Total { get; }

        /// <summary>
        /// 产品类型;
        /// </summary>
        Product Product { get; }

        /// <summary>
        /// 是否为有效的库房;
        /// </summary>
        bool IsEffective { get; }

        /// <summary>
        /// 标记占用;即当存储量为 0 时也不移除此库房;
        /// </summary>
        IDisposable Occupy(object sender);

        /// <summary>
        /// 增加产品数目; number 大于或等于 0;
        /// </summary>
        void Add(int number);

        /// <summary>
        /// 移除产品数目,并返回未能移除的数目; number 大于或等于 0;
        /// </summary>
        int Remove(int number);

        /// <summary>
        /// 按百分百增加产品数目;
        /// </summary>
        /// <param name="percent">0 ~ max</param>
        void AddPercent(float percent);

        /// <summary>
        /// 按百分百移除产品数目;
        /// </summary>
        /// <param name="percent">0 ~ 1</param>
        void RemovePercent(float percent);

        /// <summary>
        /// 尝试移除这类产品数目,若无法移除则返回false,移除成功返回true;
        /// </summary>
        bool TryRemove(int number);
    }


    /// <summary>
    /// 资源存储,记录资源数目,更新损耗;
    /// </summary>
    public class ProductWarehouse
    {

        public ProductWarehouse()
        {
            rooms = new List<CategorieRoom>();
        }

        /// <summary>
        /// 资源存储单元;
        /// </summary>
        List<CategorieRoom> rooms;

        /// <summary>
        /// 获取此类资源存储到的单元实例,若不存在则返回 null;
        /// </summary>
        public IWareroom Find(Product product)
        {
            var categorieRoom = rooms.Find(item => item.Categorie == product.Categorie);
            var room = categorieRoom.Find(product);
            return room;
        }

        /// <summary>
        /// 获取此类资源存储到的单元实例,若不存在则返回 null;
        /// </summary>
        public ICategorieWareroom Find(ProductCategorie categorie)
        {
            var categorieRoom = rooms.Find(item => item.Categorie == categorie);
            return categorieRoom;
        }

        /// <summary>
        /// 获取此类资源存储到的单元实例,若不存在则创建到;
        /// </summary>
        public IWareroom FindOrCreate(Product product)
        {
            var room = Find(product);

            if (room == null)
            {
                var categorieRoom = new CategorieRoom(this, product.Categorie);
                rooms.Add(categorieRoom);
                room = categorieRoom.FindOrCreate(product);
            }

            return room;
        }

        /// <summary>
        /// 获取此类资源存储到的单元实例,若不存在则创建到;
        /// </summary>
        public ICategorieWareroom FindOrCreate(ProductCategorie categorie)
        {
            var categorieRoom = rooms.Find(item => item.Categorie == categorie);

            if (categorieRoom == null)
            {
                categorieRoom = new CategorieRoom(this, categorie);
                rooms.Add(categorieRoom);
            }

            return categorieRoom;
        }

        /// <summary>
        /// 移除指定分类库房;
        /// </summary>
        /// <param name="room"></param>
        void DestroyRoom(CategorieRoom room)
        {
            rooms.Remove(room);
        }

        /// <summary>
        /// 资源类别合集;
        /// </summary>
        class CategorieRoom : Occupied<object>, ICategorieWareroom
        {

            public CategorieRoom(ProductWarehouse parent, ProductCategorie categorie)
            {
                this.Parent = parent;
                this.rooms = new List<Room>();
                this.Categorie = categorie;
                this.Total = 0;
                this.IsEffective = true;
            }

            
            List<Room> rooms;

            /// <summary>
            /// 这类资源总数;
            /// </summary>
            public int Total { get; private set; }
            public ProductWarehouse Parent { get; private set; }
            public ProductCategorie Categorie { get; private set; }
            public bool IsEffective { get; private set; }

            /// <summary>
            /// 存在库房数量;
            /// </summary>
            public int Count
            {
                get { return rooms.Count; }
            }

            /// <summary>
            /// 移除产品数目,并返回未能移除的数目; number 大于或等于 0;
            /// </summary>
            public int Remove(int number)
            {
                foreach (var room in rooms)
                {
                    number = room.Remove(number);

                    if (number == 0)
                        break;
                }
                return number;
            }

            /// <summary>
            /// 尝试移除这类产品数目,若无法移除则返回false,移除成功返回true;
            /// </summary>
            public bool TryRemove(int number)
            {
                if (number > Total)
                    return false;

                this.Total -= number;
                return true;
            }

            public IWareroom Find(Product product)
            {
                Room room = rooms.Find(item => item.Product == product);
                return room;
            }

            public IWareroom FindOrCreate(Product product)
            {
                Room room = rooms.Find(item => item.Product == product);

                if (room == null)
                {
                    room = new Room(this, product);
                    rooms.Add(room);
                }

                return room;
            }

            /// <summary>
            /// 移除指定库房;
            /// </summary>
            void DestroyRoom(Room room)
            {
                rooms.Remove(room);

                if (rooms.Count == 0 && !IsOccupy)
                {
                    Parent.DestroyRoom(this);
                    IsEffective = false;
                }
            }


            /// <summary>
            /// 资源数量记录;
            /// </summary>
            internal class Room : Occupied<object>, IWareroom
            {

                public Room(CategorieRoom parent, Product product) : this(parent, product, 0)
                {
                }

                public Room(CategorieRoom parent, Product product, int total)
                {
                    this.Parent = parent;
                    this.Product = product;
                    this.Total = total;
                    IsEffective = true;
                }

                public CategorieRoom Parent { get; private set; }
                public Product Product { get; private set; }
                public int Total { get; private set; }
                public bool IsEffective { get; private set; }

                /// <summary>
                /// 增加产品数目; number 大于或等于 0;
                /// </summary>
                public void Add(int number)
                {
                    if (number < 0)
                        throw new ArgumentOutOfRangeException();

                    Total += number;
                    Parent.Total += number;
                }

                /// <summary>
                /// 按百分百增加产品数目;
                /// </summary>
                /// <param name="percent">0 ~ max</param>
                public void AddPercent(float percent)
                {
                    if (percent < 0)
                        throw new ArgumentOutOfRangeException();

                    Total = (int)(Total * percent);
                }

                /// <summary>
                /// 移除产品数目,并返回未能移除的数目; number 大于或等于 0;
                /// </summary>
                public int Remove(int number)
                {
                    if (number > Total)
                    {
                        int result = Total - number;
                        Total = 0;
                        Parent.Total -= Total;
                        return Math.Abs(result);
                    }
                    else
                    {
                        Total -= number;
                        Parent.Total -= number;
                        return 0;
                    }
                }

                /// <summary>
                /// 按百分百移除产品数目;
                /// </summary>
                /// <param name="percent">0 ~ 1</param>
                public void RemovePercent(float percent)
                {
                    if (percent < 0 || percent > 1)
                        throw new ArgumentOutOfRangeException();

                    int result = (int)(Total * percent);
                    Total -= result;
                    Parent.Total -= result;
                }

                /// <summary>
                /// 尝试移除这类产品数目,若无法移除则返回false,移除成功返回true;
                /// </summary>
                public bool TryRemove(int number)
                {
                    if (Total < number)
                        return false;

                    Total -= number;
                    Parent.Total -= number;
                    return true;
                }

                /// <summary>
                /// 尝试摧毁这个房间,若还有其它类标记占用则不移除,返回false;若移除成功,并返回true;
                /// </summary>
                public bool TryDestroy()
                {
                    if (!IsOccupy)
                    {
                        Reset();
                        Parent.DestroyRoom(this);
                        IsEffective = false;
                        return true;
                    }
                    return false;
                }

                /// <summary>
                /// 重置计数;
                /// </summary>
                public void Reset()
                {
                    Parent.Total -= Total;
                    Total = 0;
                }

            }

        }

    }


    public class Occupied<T>
    {

        List<T> occupySenders = new List<T>();

        /// <summary>
        /// 是否存在占用?
        /// </summary>
        public bool IsOccupy
        {
            get { return occupySenders.Count != 0; }
        }

        /// <summary>
        /// 标记占用;
        /// </summary>
        public IDisposable Occupy(T sender)
        {
            var item = new Canceler(occupySenders, sender);
            occupySenders.Add(sender);
            return item;
        }

        class Canceler : IDisposable
        {
            public Canceler(List<T> occupySenders, T sender)
            {
                this.occupySenders = occupySenders;
                this.sender = sender;
            }

            List<T> occupySenders;
            T sender;

            public virtual void Dispose()
            {
                if (occupySenders != null)
                {
                    occupySenders.Remove(sender);
                    occupySenders = null;
                    sender = default(T);
                }
            }
        }

    }

}
