using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Collections;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 建筑信息;
    /// </summary>
    public struct BuildingInfo
    {

    }

    /// <summary>
    /// 建筑管理;
    /// </summary>
    public class BuildingManager
    {
        public IReadOnlyDictionary<int, Building> Buildings { get; private set; }
        public IReadOnlyDictionary<int, BuildingInfo> BuildingInfos { get; private set; }
    }

    /// <summary>
    /// 建筑物基类;
    /// </summary>
    public abstract class Building : IEquatable<Building>
    {
        public Building(BuildingManager manager, int id)
        {
            Manager = manager;
            BuildingID = id;
        }

        public BuildingManager Manager { get; private set; }

        /// <summary>
        /// 编号;
        /// </summary>
        public int BuildingID { get; private set; }

        /// <summary>
        /// 建筑物信息;
        /// </summary>
        public BuildingInfo Info
        {
            get { return Manager.BuildingInfos[BuildingID]; }
        }

        /// <summary>
        /// 获取到城镇建筑类型实例;
        /// </summary>
        public abstract TownFactory GetTownBuilding(IRequestor requestor, Town belongToTown);

        /// <summary>
        /// 建造此建筑的前提;
        /// </summary>
        public bool Precondition(Town town)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Building other)
        {
            return other.BuildingID == this.BuildingID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Building))
                return false;
            return Equals((Building)obj);
        }

        public override int GetHashCode()
        {
            return BuildingID;
        }

        public static bool operator ==(Building v1, Building v2)
        {
            return v1.BuildingID == v2.BuildingID;
        }

        public static bool operator !=(Building v1, Building v2)
        {
            return v1.BuildingID != v2.BuildingID;
        }

    }

    /// <summary>
    /// 城镇建筑抽象类;
    /// </summary>
    public abstract class TownFactory : Townish, IDisposable
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
        /// 获取到生产因子;
        /// </summary>
        public float GetProduceFactor(Product product)
        {
            return GetProduceFactor(product, Manpower);
        }

        /// <summary>
        /// 获取到生产因子,指定人力数量;
        /// </summary>
        public float GetProduceFactor(Product product, int manpower)
        {
            float productYieldProportion = ProductInfos[product].Production.YieldProportion;
            return productYieldProportion * manpower;
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
        public override TownFactory GetTownBuilding(IRequestor requestor, Town belongToTown)
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
                wareroom = belongToTown.Warehouse.FindOrCreate(requestor, ProductContainer.Product);
                productionCanceler = belongToTown.Production.Add(this);
            }

            IWareroom wareroom;
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
                float factor = GetProduceFactor(ProductContainer.Product);
                int yield = (int)(factor * ProductContainer.Number);
                wareroom.Add(yield);
            }

            public override void Dispose()
            {
                if (productionCanceler != null)
                {
                    wareroom.Dispose();
                    productionCanceler.Dispose();
                    productionCanceler = null;
                }
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
        public override TownFactory GetTownBuilding(IRequestor requestor, Town belongToTown)
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
                itemList = GetItemList(belongToTown, parent.Products);
                productionCanceler = Production.Add(this);
            }

            public ProductFactory Parent { get; private set; }
            ICollection<Item> itemList;
            IDisposable productionCanceler;

            /// <summary>
            /// 生产产品类型;
            /// </summary>
            public IEnumerable<Container<Product>> ProductContainers
            {
                get { return itemList.Select(item => item.ProductContainer); }
            }

            ICollection<Item> GetItemList(Town belongToTown, IEnumerable<Container<Product>> productContainers)
            {
                return productContainers.
                    Select(productContainer => new Item(this, productContainer)).
                    ToArray();
            }

            void IProducible.OnProduce()
            {
                foreach (var item in itemList)
                {
                    item.OnProduce();
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

            class Item : IDisposable
            {
                public Item(Factory parent, Container<Product> productContainer)
                {
                    Parent = parent;
                    wareroom = parent.BelongToTown.Warehouse.FindOrCreate(this, productContainer.Product);
                    ProductContainer = productContainer;
                }

                public Factory Parent { get; private set; }
                IWareroom wareroom;
                public Container<Product> ProductContainer { get; private set; }

                public void OnProduce()
                {
                    float factor = Parent.GetProduceFactor(ProductContainer.Product);
                    int yield = (int)(factor * ProductContainer.Number);
                    wareroom.Add(yield);
                }

                public void Dispose()
                {
                    wareroom.Dispose();
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
        public override TownFactory GetTownBuilding(IRequestor requestor, Town belongToTown)
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
                rawWareroom = belongToTown.Warehouse.FindOrCreate(requestor, Raw.Product);
                produceWareroom = belongToTown.Warehouse.FindOrCreate(requestor, Produce.Product);
                productionCanceler = belongToTown.Production.Add(this);
                consumeCanceler = belongToTown.Consume.Add(this);

                IsProcurement = false;
                ProcuredTempFactor = 0;
            }

            public SingleConvertedFactory Parent { get; private set; }

            IWareroom rawWareroom;
            IWareroom produceWareroom;
            IDisposable productionCanceler;
            IDisposable consumeCanceler;

            /// <summary>
            /// 是否已经采购?
            /// </summary>
            public bool IsProcurement { get; private set; }

            /// <summary>
            /// 暂存消耗的因子数;
            /// </summary>
            public float ProcuredTempFactor { get; private set; }

            /// <summary>
            /// 消耗;
            /// </summary>
            public Container<Product> Raw
            {
                get { return Parent.Raw; }
            }

            /// <summary>
            /// 产出;
            /// </summary>
            public Container<Product> Produce
            {
                get { return Parent.Produce; }
            }

            void IConsumable.OnConsume()
            {
                IsProcurement = rawWareroom.Remove(Raw.Number);
                if (IsProcurement)
                {
                    ProcuredTempFactor = GetProduceFactor(Produce);
                }
            }

            void IProducible.OnProduce()
            {
                if (IsProcurement)
                {
                    //因子取 昨日"采购" 和 今日"产出" 最小的;
                    float factor = Math.Min(ProcuredTempFactor, GetProduceFactor(Produce));
                    int yield = (int)(factor * Produce.Number);
                    produceWareroom.Add(yield);
                    IsProcurement = false;
                }
            }

            public override void Dispose()
            {
                rawWareroom.Dispose();
                produceWareroom.Dispose();
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

        public override TownFactory GetTownBuilding(IRequestor requestor, Town belongToTown)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 城镇建筑实例;
        /// </summary>
        class Factory : TownFactory, IProducible, IConsumable
        {
            public Factory(Building building, Town belongToTown) : base(building, belongToTown)
            {

            }

            ICollection<Item> raws;
            ICollection<Item> produces;

            ICollection<Item> GetCollection(IEnumerable<Container<Product>> produces)
            {
                throw new NotImplementedException();
            }

            void IConsumable.OnConsume()
            {
                throw new NotImplementedException();
            }

            void IProducible.OnProduce()
            {
                throw new NotImplementedException();
            }

            public override void Dispose()
            {
                throw new NotImplementedException();
            }

            class Item
            {
                public IWareroom Wareroom { get; private set; }
                public Container<Product> ProductContainer { get; private set; }
            }

        }

    }


}
