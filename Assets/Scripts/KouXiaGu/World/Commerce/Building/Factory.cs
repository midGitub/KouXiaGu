using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 城镇建筑抽象类;
    /// </summary>
    public abstract class TownFactory : Townish, ITownBuilding
    {
        public TownFactory(Building building, Town belongToTown) : base(belongToTown)
        {
            BuildingInfo = building;
        }

        /// <summary>
        /// 建筑信息;
        /// </summary>
        public Building BuildingInfo { get; private set; }

        /// <summary>
        /// 仓库;
        /// </summary>
        protected ProductWarehouse Warehouse
        {
            get { return BelongToTown.Warehouse; }
        }

        /// <summary>
        /// 生产模块;
        /// </summary>
        protected ProductProduction Production
        {
            get { return BelongToTown.Production; }
        }

        /// <summary>
        /// 消耗模块;
        /// </summary>
        protected ProductConsumption Consume
        {
            get { return BelongToTown.Consume; }
        }

        /// <summary>
        /// 人力资源;
        /// </summary>
        protected Manpower Manpower
        {
            get { return BelongToTown.Manpower; }
        }

        /// <summary>
        /// 取消这个建筑;
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// 获取到合集结构;
        /// </summary>
        protected ICollection<ProductionItem> GetCollection(IRequestor requestor, IEnumerable<Container<Product>> productContainers)
        {
            return productContainers.
                Select(productContainer => new ProductionItem(requestor, this, productContainer)).
                ToArray();
        }

        /// <summary>
        /// 获取到生产因子;
        /// </summary>
        protected float GetProduceFactor(Product product)
        {
            return GetProduceFactor(product, Manpower);
        }

        /// <summary>
        /// 获取到生产因子,指定人力数量;
        /// </summary>
        protected float GetProduceFactor(Product product, int manpower)
        {
            float productYieldProportion = ProductInfos[product].Production.YieldProportion;
            return productYieldProportion * manpower;
        }


        /// <summary>
        /// 获取到最大的消耗次数;
        /// </summary>
        protected int GetMaxRawTime(IEnumerable<ProductionItem> items)
        {
            int time = Manpower;

            foreach (var item in items)
            {
                int temp = GetMaxRawTime(item);

                if (time > temp)
                    time = temp;
            }

            return time;
        }

        /// <summary>
        /// 获取到最大的消耗次数;
        /// </summary>
        protected int GetMaxRawTime(ProductionItem item)
        {
            return Math.Min(Manpower, item.Wareroom.Total / item.Container.Number);
        }

        /// <summary>
        /// 生产条目;
        /// </summary>
        protected class ProductionItem : IDisposable
        {
            public ProductionItem(IRequestor requestor, TownFactory parent, Container<Product> container)
            {
                Parent = parent;
                Wareroom = parent.Warehouse.FindOrCreate(requestor, container.Product);
                Container = container;
            }

            public TownFactory Parent { get; private set; }
            public IWareroom Wareroom { get; private set; }
            public Container<Product> Container { get; private set; }

            Product Product
            {
                get { return Container.Product; }
            }

            int Number
            {
                get { return Container.Number; }
            }

            /// <summary>
            /// 根据城镇信息增加产品数量;
            /// </summary>
            public void AddProduct()
            {
                float factor = Parent.GetProduceFactor(Product);
                int yield = (int)(factor * Number);
                Wareroom.Add(yield);
            }

            /// <summary>
            /// 根据城镇信息增加产品数量,并且指定人力资源数量;
            /// </summary>
            public void AddProduct(int manpower)
            {
                float factor = Parent.GetProduceFactor(Product, manpower);
                int yield = (int)(factor * Number);
                Wareroom.Add(yield);
            }

            /// <summary>
            /// 移除这个条目多次,若无法移除则返回异常;
            /// </summary>
            /// <param name="time">移除次数,取值 大于0;</param>
            public void RemoveProduct(int time)
            {
                bool isRemove = Wareroom.Remove(Number * time);

                if (!isRemove)
                {
                    throw new ArgumentException("共计[" + Wareroom.Total + "]无法移除[" + Number * time + "];");
                }
            }

            public void Dispose()
            {
                Wareroom.Dispose();
            }

        }

    }

    /// <summary>
    /// 单物品产出的工厂;
    /// </summary>
    public class SingleProductFactory : Building
    {
        public SingleProductFactory(BuildingManager manager, int id, Container<Product> produce) : base(manager, id)
        {
            if (produce.Number <= 0 || produce.Product == null)
                throw new ArgumentException();

            Produce = produce;
        }

        /// <summary>
        /// 生产产品类型;
        /// </summary>
        public Container<Product> Produce { get; private set; }

        /// <summary>
        /// 获取到城镇建筑类型实例;
        /// </summary>
        public override ITownBuilding GetTownBuilding(IRequestor requestor, Town belongToTown)
        {
            return new Factory(this, requestor, belongToTown);
        }

        /// <summary>
        /// 城镇建筑实例;
        /// </summary>
        class Factory : TownFactory, IProducible
        {
            public Factory(SingleProductFactory parent, IRequestor requestor, Town belongToTown) :
                base(parent, belongToTown)
            {
                Parent = parent;
                produce = new ProductionItem(requestor, this, parent.Produce);
                productionCanceler = belongToTown.Production.Add(this);
            }

            ProductionItem produce;
            IDisposable productionCanceler;
            public SingleProductFactory Parent { get; private set; }

            /// <summary>
            /// 生产产品类型;
            /// </summary>
            public Container<Product> ProductContainer
            {
                get { return Parent.Produce; }
            }

            void IProducible.OnProduce()
            {
                produce.AddProduct();
            }

            public override void Dispose()
            {
                produce.Dispose();
                productionCanceler.Dispose();
            }

        }

    }

    /// <summary>
    /// 多个产品产出的工厂;
    /// </summary>
    public class ProductFactory : Building
    {
        public ProductFactory(BuildingManager manager, int id, IEnumerable<Container<Product>> produces) :
            base(manager, id)
        {
            if (!IsEffective(produces))
                throw new ArgumentException();

            this.produces = produces.ToArray();
        }

        /// <summary>
        /// 所有产出;
        /// </summary>
        Container<Product>[] produces;

        /// <summary>
        /// 产出;
        /// </summary>
        public IEnumerable<Container<Product>> Products
        {
            get { return produces; }
        }

        /// <summary>
        /// 生产内容是否符合要求;
        /// </summary>
        bool IsEffective(IEnumerable<Container<Product>> products)
        {
            return produces.Any(produce => produce.Number > 0 || produce.Product != null);
        }

        /// <summary>
        /// 获取到城镇建筑类型实例;
        /// </summary>
        public override ITownBuilding GetTownBuilding(IRequestor requestor, Town belongToTown)
        {
            return new Factory(this, requestor, belongToTown);
        }

        /// <summary>
        /// 城镇建筑实例;
        /// </summary>
        class Factory : TownFactory, IProducible
        {
            public Factory(ProductFactory parent, IRequestor requestor, Town belongToTown) :
                base(parent, belongToTown)
            {
                Parent = parent;
                itemList = GetCollection(requestor, parent.Products);
                productionCanceler = Production.Add(this);
            }

            public ProductFactory Parent { get; private set; }
            ICollection<ProductionItem> itemList;
            IDisposable productionCanceler;

            void IProducible.OnProduce()
            {
                foreach (var item in itemList)
                {
                    item.AddProduct();
                }
            }

            public override void Dispose()
            {
                if (itemList != null)
                {
                    foreach (var item in itemList)
                    {
                        item.Dispose();
                    }

                    productionCanceler.Dispose();
                    itemList = null;
                }
            }

        }

    }

    /// <summary>
    /// 单个产品转换单个类型的工厂;
    /// </summary>
    public class SingleConvertedFactory : Building
    {
        public SingleConvertedFactory(
            BuildingManager manager,
            int id,
            Container<Product> raw,
            Container<Product> produce) :
            base(manager, id)
        {
            if (!IsIsEffective(raw) && !IsIsEffective(produce))
                throw new ArgumentException();

            Raw = raw;
            Produce = produce;
        }

        /// <summary>
        /// 消耗;
        /// </summary>
        public Container<Product> Raw { get; private set; }

        /// <summary>
        /// 产出;
        /// </summary>
        public Container<Product> Produce { get; private set; }

        /// <summary>
        /// 该物体格式是否合法?
        /// </summary>
        bool IsIsEffective(Container<Product> produce)
        {
            return
                produce.Product != null ||
                produce.Number > 0;
        }

        /// <summary>
        /// 获取到城镇建筑类型实例;
        /// </summary>
        public override ITownBuilding GetTownBuilding(IRequestor requestor, Town belongToTown)
        {
            return new Factory(this, requestor, belongToTown);
        }

        /// <summary>
        /// 城镇建筑实例;
        /// </summary>
        class Factory : TownFactory, IProducible, IConsumable
        {
            public Factory(SingleConvertedFactory parent, IRequestor requestor, Town belongToTown) :
                base(parent, belongToTown)
            {
                Parent = parent;
                raw = new ProductionItem(requestor, this, parent.Raw);
                produce = new ProductionItem(requestor, this, parent.Produce);
                productionCanceler = belongToTown.Production.Add(this);
                consumeCanceler = belongToTown.Consume.Add(this);
                IsProcurement = false;
                TempManpower = 0;
            }

            public SingleConvertedFactory Parent { get; private set; }
            ProductionItem raw;
            ProductionItem produce;
            IDisposable productionCanceler;
            IDisposable consumeCanceler;

            /// <summary>
            /// 是否已经采购?
            /// </summary>
            public bool IsProcurement { get; private set; }

            /// <summary>
            /// 消耗时所用的人口数量;
            /// </summary>
            public int TempManpower { get; private set; }

            void IConsumable.OnConsume()
            {
                if (!IsProcurement)
                {
                    int time = GetMaxRawTime(raw);
                    raw.RemoveProduct(time);
                    IsProcurement = true;
                }
            }

            void IProducible.OnProduce()
            {
                if (IsProcurement)
                {
                    produce.AddProduct(TempManpower);
                    IsProcurement = false;
                }
            }

            public override void Dispose()
            {
                raw.Dispose();
                produce.Dispose();
                productionCanceler.Dispose();
                consumeCanceler.Dispose();
            }

        }

    }

    /// <summary>
    /// 多个产品转换成多个产品的工厂;
    /// </summary>
    public class ConvertedFactory : Building
    {
        public ConvertedFactory(
            BuildingManager manager,
            int id,
            IEnumerable<Container<Product>> raws,
            IEnumerable<Container<Product>> produces) :
            base(manager, id)
        {
            if (!IsIsEffective(raws) || !IsIsEffective(produces))
                throw new ArgumentException();

            this.raws = raws.ToArray();
            this.produces = produces.ToArray();
        }

        ICollection<Container<Product>> raws;
        ICollection<Container<Product>> produces;

        /// <summary>
        /// 该物体格式是否合法?
        /// </summary>
        bool IsIsEffective(IEnumerable<Container<Product>> produces)
        {
            return produces.Any(produce => produce.Number > 0 || produce.Product != null);
        }

        public override ITownBuilding GetTownBuilding(IRequestor requestor, Town belongToTown)
        {
            return new Factory(this, belongToTown, requestor);
        }

        /// <summary>
        /// 城镇建筑实例;
        /// </summary>
        class Factory : TownFactory, IProducible, IConsumable
        {
            public Factory(ConvertedFactory parent, Town belongToTown, IRequestor requestor) : base(parent, belongToTown)
            {
                Parent = parent;
                raws = GetCollection(requestor, parent.raws);
                produces = GetCollection(requestor, parent.produces);
                productionCanceler = belongToTown.Production.Add(this);
                consumeCanceler = belongToTown.Consume.Add(this);
                IsProcurement = false;
                TempManpower = 0;
            }

            public ConvertedFactory Parent { get; private set; }
            ICollection<ProductionItem> raws;
            ICollection<ProductionItem> produces;
            IDisposable productionCanceler;
            IDisposable consumeCanceler;

            /// <summary>
            /// 是否已经采购?
            /// </summary>
            public bool IsProcurement { get; private set; }

            /// <summary>
            /// 消耗时所用的人口数量;
            /// </summary>
            public int TempManpower { get; private set; }

            void IConsumable.OnConsume()
            {
                if (!IsProcurement)
                {
                    TempManpower = GetMaxRawTime(raws);

                    foreach (var item in raws)
                    {
                        item.RemoveProduct(TempManpower);
                    }

                    IsProcurement = true;
                }
            }

            void IProducible.OnProduce()
            {
                if (IsProcurement)
                {
                    foreach (var item in raws)
                    {
                        item.AddProduct(TempManpower);
                    }

                    IsProcurement = false;
                }
            }

            public override void Dispose()
            {
                if (raws != null)
                {
                    foreach (var item in raws)
                        item.Dispose();
                    raws = null;
                }

                if (produces != null)
                {
                    foreach (var item in produces)
                        item.Dispose();
                    produces = null;
                }

                productionCanceler.Dispose();
                consumeCanceler.Dispose();
            }

        }

    }


}
