using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.World.Commerce
{


    public interface IReadOnlyCategorieWareroom
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
        /// 其下的所有库房;
        /// </summary>
        IEnumerable<IReadOnlyWareroom> ReadOnlyWarerooms { get; }

    }

    public interface ICategorieWareroom : IDisposable
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
        /// 尝试移除这类产品数目,若无法移除则返回false,移除成功返回true;
        /// </summary>
        bool Remove(int number);

    }

    public interface IReadOnlyWareroom
    {
        /// <summary>
        /// 存储量;
        /// </summary>
        int Total { get; }

        /// <summary>
        /// 产品类型;
        /// </summary>
        Product Product { get; }
    }

    public interface IWareroom : IDisposable
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
        /// 增加产品数目; number 大于或等于 0;
        /// </summary>
        void Add(int number);

        /// <summary>
        /// 尝试移除这类产品数目,若无法移除则返回false,移除成功返回true;
        /// </summary>
        bool Remove(int number);

    }


    /// <summary>
    /// 资源存储,记录资源数目,更新损耗;
    /// </summary>
    public class ProductWarehouse : National
    {

        public ProductWarehouse(Country belongToCountry) : base(belongToCountry)
        {
            rooms = new List<CategorieRoom>();
            Spoil = new SpoilEffect(belongToCountry);
        }

        List<CategorieRoom> rooms;
        public SpoilEffect Spoil { get; private set; }

        /// <summary>
        /// 获取到保存的所有类型的库房;
        /// </summary>
        public IEnumerable<IReadOnlyCategorieWareroom> ReadOnlyWarerooms
        {
            get { return rooms.OfType<IReadOnlyCategorieWareroom>(); }
        }

        #region Find;

        /// <summary>
        /// 获取此类资源存储到的单元实例,若不存在则返回 null;
        /// </summary>
        public IWareroom Find(object sender, Product product)
        {
            var categorieRoom = rooms.Find(item => item.Categorie == product.Categorie);
            IWareroom room = categorieRoom.Find(sender, product);
            return room;
        }

        /// <summary>
        /// 获取此类资源存储到的单元实例,若不存在则创建到;
        /// </summary>
        public IWareroom FindOrCreate(object sender, Product product)
        {
            var room = Find(sender, product);

            if (room == null)
            {
                var categorieRoom = new CategorieRoom(this, product.Categorie);
                rooms.Add(categorieRoom);
                room = categorieRoom.FindOrCreate(sender, product);
            }

            return room;
        }

        /// <summary>
        /// 获取此类资源存储到的单元实例,若不存在则返回 null;
        /// </summary>
        public ICategorieWareroom Find(object sender, ProductCategorie categorie)
        {
            var categorieRoom = rooms.Find(item => item.Categorie == categorie);

            if (categorieRoom != null)
                return categorieRoom.Occupy(sender);
            else
                return null;
        }

        /// <summary>
        /// 获取此类资源存储到的单元实例,若不存在则创建到;
        /// </summary>
        public ICategorieWareroom FindOrCreate(object sender, ProductCategorie categorie)
        {
            var categorieRoom = rooms.Find(item => item.Categorie == categorie);

            if (categorieRoom == null)
            {
                categorieRoom = new CategorieRoom(this, categorie);
                rooms.Add(categorieRoom);
            }

            return categorieRoom.Occupy(sender);
        }

        #endregion

        /// <summary>
        /// 清除所有空的房间;
        /// </summary>
        public void ClearEmptyRooms()
        {
            rooms.RemoveAll(delegate (CategorieRoom room)
            {
                return room.OnTryDestory();
            });
        }

        /// <summary>
        /// 每日更新一次;
        /// </summary>
        public void DayUpdate()
        {
            Spoil.DayUpdate();
        }

        /// <summary>
        /// 创建一个新库房之后调用;
        /// </summary>
        /// <param name="room">传入的库房不记录占用;</param>
        /// <returns>返回销毁库房时需要同时销毁的内容</returns>
        ICollection<IDisposable> OnCreatedWareroom(IWareroom room)
        {
            IDisposable[] disposer = new IDisposable[]
                {
                    Spoil.OnCreated(room),
                };

            return disposer;
        }


        /// <summary>
        /// 资源类别合集;
        /// </summary>
        public class CategorieRoom : IReadOnlyCategorieWareroom
        {

            public CategorieRoom(ProductWarehouse warehouse, ProductCategorie categorie)
            {
                this.Warehouse = warehouse;
                this.Categorie = categorie;
                this.Total = 0;
                this.IsEffective = true;
                this.rooms = new List<ProductRoom>();
                this.occupiers = new LinkedList<object>();
            }

            List<ProductRoom> rooms;
            LinkedList<object> occupiers;
            public int Total { get; private set; }
            public bool IsEffective { get; private set; }
            public ProductWarehouse Warehouse { get; private set; }
            public ProductCategorie Categorie { get; private set; }

            public int OccupierCount
            {
                get { return occupiers.Count; }
            }

            public bool IsEmpty
            {
                get { return Total <= 0 && OccupierCount == 0; }
            }

            /// <summary>
            /// 存在库房数量;
            /// </summary>
            public int Count
            {
                get { return rooms.Count; }
            }

            /// <summary>
            /// 迭代所有库房;
            /// </summary>
            public IEnumerable<IReadOnlyWareroom> ReadOnlyWarerooms
            {
                get { return rooms.OfType<IReadOnlyWareroom>(); }
            }

            /// <summary>
            /// 尝试移除这类产品数目,若无法移除则返回false,移除成功返回true;
            /// </summary>
            public bool Remove(int number)
            {
                if (number > Total)
                    return false;

                foreach (var room in rooms)
                {
                    if (room.Total < number)
                    {
                        number -= room.Total;
                        room.Remove(room.Total);
                    }
                    else
                    {
                        room.Remove(number);
                        break;
                    }
                }

                return true;
            }

            /// <summary>
            /// 获取到这个产品存储的仓库,若不存在则返回null;(在使用完毕后记得 Dispose() );
            /// </summary>
            public IWareroom Find(object sender, Product product)
            {
                ProductRoom room = rooms.Find(item => item.Product == product);

                if (room != null)
                    return room.Occupy(sender);
                else
                    return null;
            }

            /// <summary>
            /// 获取到这个产品存储的仓库,若不存在则创建并返回;(在使用完毕后记得 Dispose() );
            /// </summary>
            public IWareroom FindOrCreate(object sender, Product product)
            {
                ProductRoom room = rooms.Find(item => item.Product == product);

                if (room == null)
                {
                    room = new ProductRoom(this, product);
                    rooms.Add(room);
                    OnCreatedWareroom(room);
                }

                return room.Occupy(sender);
            }

            /// <summary>
            /// 清除所有空的房间;
            /// </summary>
            public void ClearEmptyRooms()
            {
                rooms.RemoveAll(delegate (ProductRoom room)
                {
                    return room.OnTryDestory();
                });
            }

            /// <summary>
            /// 当销毁时调用;
            /// </summary>
            public bool OnTryDestory()
            {
                ClearEmptyRooms();
                if (IsEmpty)
                {
                    IsEffective = false;
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 标识占用到这个类,既这个类在没有 Dispose 之前,一直处于有效状态;
            /// </summary>
            public ICategorieWareroom Occupy(object sender)
            {
                var node = occupiers.AddLast(sender);
                return new OccupiedWareroom(this, occupiers, node);
            }

            /// <summary>
            /// 创建一个新库房之后调用;
            /// </summary>
            /// <param name="room">传入的库房不记录占用;</param>
            /// <returns>返回销毁库房时需要同时销毁的内容</returns>
            ICollection<IDisposable> OnCreatedWareroom(IWareroom room)
            {
                return Warehouse.OnCreatedWareroom(room);
            }

            /// <summary>
            /// 提供给外部占用和使用;
            /// </summary>
            class OccupiedWareroom : ICategorieWareroom
            {
                public OccupiedWareroom(CategorieRoom room, LinkedList<object> list, LinkedListNode<object> node)
                {
                    this.Room = room;
                    this.list = list;
                    this.node = node;
                }

                LinkedList<object> list;
                LinkedListNode<object> node;
                public CategorieRoom Room { get; private set; }

                public object Sender
                {
                    get { return node.Value; }
                }

                ProductCategorie ICategorieWareroom.Categorie
                {
                    get { return Room.Categorie; }
                }

                int ICategorieWareroom.Total
                {
                    get { return Room.Total; }
                }

                bool ICategorieWareroom.Remove(int number)
                {
                    return Room.Remove(number);
                }

                void IDisposable.Dispose()
                {
                    if (list != null)
                    {
                        list.Remove(node);
                        list = null;
                    }
                }

            }

            /// <summary>
            /// 资源数量记录;
            /// </summary>
            public class ProductRoom : IWareroom, IReadOnlyWareroom
            {

                public ProductRoom(CategorieRoom categorieRoom, Product product, IEnumerable<IDisposable> disposers) :
                    this(categorieRoom, product)
                {
                    Disposers = disposers;
                }

                public ProductRoom(CategorieRoom categorieRoom, Product product) : this(categorieRoom, product, 0)
                {
                }

                public ProductRoom(CategorieRoom categorieRoom, Product product, int total)
                {
                    this.CategorieRoom = categorieRoom;
                    this.Product = product;
                    this.Total = total;
                    this.IsEffective = true;
                    occupiers = new LinkedList<object>();
                }

                public int Total { get; private set; }
                public CategorieRoom CategorieRoom { get; private set; }
                public Product Product { get; private set; }
                public bool IsEffective { get; private set; }
                public IEnumerable<IDisposable> Disposers { get; private set; }
                LinkedList<object> occupiers;

                public IEnumerable<object> Occupiers
                {
                    get { return occupiers; }
                }

                public int OccupierCount
                {
                    get { return occupiers.Count; }
                }

                public bool IsEmpty
                {
                    get { return Total <= 0 && OccupierCount == 0; }
                }

                /// <summary>
                /// 增加产品数目; number 大于或等于 0;
                /// </summary>
                public void Add(int number)
                {
                    if (number < 0)
                        throw new ArgumentOutOfRangeException();

                    Total += number;
                    CategorieRoom.Total += number;
                }

                /// <summary>
                /// 尝试移除这类产品数目,若无法移除则返回false,移除成功返回true;
                /// </summary>
                public bool Remove(int number)
                {
                    if (Total < number)
                        return false;

                    Total -= number;
                    CategorieRoom.Total -= number;
                    return true;
                }

                /// <summary>
                /// 重置计数;
                /// </summary>
                public void Reset()
                {
                    CategorieRoom.Total -= Total;
                    Total = 0;
                }

                /// <summary>
                /// 标识占用到这个类,既这个类在没有 Dispose 之前,一直处于有效状态;
                /// </summary>
                public IWareroom Occupy(object sender)
                {
                    var node = occupiers.AddLast(sender);
                    return new OccupiedWareroom(this, occupiers, node);
                }

                /// <summary>
                /// 尝试销毁这个库房,若成功则返回true,否则返回false;
                /// </summary>
                public bool OnTryDestory()
                {
                    if (IsEmpty)
                    {
                        IsEffective = false;

                        if (Disposers != null)
                        {
                            foreach (var disposer in Disposers)
                            {
                                disposer.Dispose();
                            }
                        }
                        return true;
                    }
                    return false;
                }

                /// <summary>
                /// 不实现;
                /// </summary>
                public void Dispose()
                {
                    return;
                }

                /// <summary>
                /// 提供给需要记录占用的模块;
                /// </summary>
                class OccupiedWareroom : IWareroom
                {
                    public OccupiedWareroom(ProductRoom room, LinkedList<object> list, LinkedListNode<object> node)
                    {
                        this.Room = room;
                        this.list = list;
                        this.node = node;
                    }

                    public ProductRoom Room { get; private set; }
                    LinkedList<object> list;
                    LinkedListNode<object> node;

                    public object Sender
                    {
                        get { return node.Value; }
                    }

                    public Product Product
                    {
                        get { return Room.Product; }
                    }

                    public int Total
                    {
                        get { return Room.Total; }
                    }

                    public void Add(int number)
                    {
                        Room.Add(number);
                    }

                    public bool Remove(int number)
                    {
                        return Room.Remove(number);
                    }

                    public void Dispose()
                    {
                        if (list != null)
                        {
                            list.Remove(node);
                            list = null;
                        }
                    }

                }

            }

        }

    }

}
