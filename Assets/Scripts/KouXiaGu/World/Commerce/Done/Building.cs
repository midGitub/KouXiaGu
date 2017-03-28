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
        public abstract ITownBuilding GetTownBuilding(IRequestor requestor, Town belongToTown);

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
    /// 城镇建筑实例;
    /// </summary>
    public interface ITownBuilding : IDisposable
    {
        /// <summary>
        /// 所归属的城镇;
        /// </summary>
        Town BelongToTown { get; }

        /// <summary>
        /// 建筑信息;
        /// </summary>
        Building BuildingInfo { get; }
    }

    /// <summary>
    /// 单物品固定产出的工厂;
    /// </summary>
    public class SingleProductFactory : Building
    {
        public SingleProductFactory(BuildingManager manager, int id, Container<Product> produce) : base(manager, id)
        {
            if (produce.Number <= 0 && produce.Product == null)
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
            return new TownFactory(this, requestor, belongToTown);
        }

        /// <summary>
        /// 城镇建筑实例;
        /// </summary>
        class TownFactory : Townish, IProducible, ITownBuilding
        {
            public TownFactory(SingleProductFactory parent, IRequestor requestor, Town belongToTown) :
                base(belongToTown)
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

            /// <summary>
            /// 所使用的产品信息;
            /// </summary>
            public ProductInfoGroup ProductInfo
            {
                get { return BelongToTown.ProductInfos; }
            }

            /// <summary>
            /// 产出加成信息;
            /// </summary>
            ProductProductionInfo productionInfo
            {
                get { return ProductInfo[ProductContainer.Product].Production; }
            }

            Building ITownBuilding.BuildingInfo
            {
                get { return Parent; }
            }

            void IProducible.OnProduce()
            {
                int yield = (int)(productionInfo.YieldProportion * ProductContainer.Number);
                wareroom.Add(yield);
            }

            public void Dispose()
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
    /// 多个产品固定产出的工厂;
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
            foreach (var product in products)
            {
                if (product.Number <= 0 && product.Product == null)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 获取到城镇建筑类型实例;
        /// </summary>
        public override ITownBuilding GetTownBuilding(IRequestor requestor, Town belongToTown)
        {
            return new TownFactory(this, requestor, belongToTown);
        }


        /// <summary>
        /// 城镇建筑实例;
        /// </summary>
        class TownFactory : Townish, IProducible, ITownBuilding
        {
            public TownFactory(ProductFactory parent, IRequestor requestor, Town belongToTown) :
                base(belongToTown)
            {
                Parent = parent;
                itemList = GetItemList(belongToTown, parent.Products);
                productionCanceler = belongToTown.Production.Add(this);
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

            Building ITownBuilding.BuildingInfo
            {
                get { return Parent; }
            }

            ICollection<Item> GetItemList(Town belongToTown, IEnumerable<Container<Product>> productContainers)
            {
                return productContainers.
                    Select(productContainer => new Item(belongToTown, productContainer)).
                    ToArray();
            }

            void IProducible.OnProduce()
            {
                foreach (var item in itemList)
                {
                    item.OnProduce();
                }
            }

            public void Dispose()
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
                public Item(Town belongToTown, Container<Product> productContainer)
                {
                    Wareroom = belongToTown.Warehouse.FindOrCreate(this, productContainer.Product);
                    ProductionInfo = belongToTown.ProductInfos[productContainer.Product].Production;
                    ProductContainer = productContainer;
                }

                public IWareroom Wareroom { get; private set; }
                public ProductProductionInfo ProductionInfo { get; private set; }
                public Container<Product> ProductContainer { get; private set; }

                public void OnProduce()
                {
                    int yield = (int)(ProductionInfo.YieldProportion * ProductContainer.Number);
                    Wareroom.Add(yield);
                }

                public void Dispose()
                {
                    Wareroom.Dispose();
                }

            }

        }

    }

    /// <summary>
    /// 转换产品类型固定产出的工厂;
    /// </summary>
    public class SingleConvertedFactory : Building
    {
        public SingleConvertedFactory(BuildingManager manager, int id, Container<Product> raw, Container<Product> produce) :
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
                produce.Product != null &&
                produce.Number > 0;
        }

        /// <summary>
        /// 获取到城镇建筑类型实例;
        /// </summary>
        public override ITownBuilding GetTownBuilding(IRequestor requestor, Town belongToTown)
        {
            return new TownFactory(this, requestor, belongToTown);
        }

        /// <summary>
        /// 城镇建筑实例;
        /// </summary>
        class TownFactory : Townish, ITownBuilding, IProducible, IConsumable
        {
            public TownFactory(SingleConvertedFactory parent, IRequestor requestor, Town belongToTown) : base(belongToTown)
            {
                Parent = parent;
                rawWareroom = belongToTown.Warehouse.FindOrCreate(requestor, Raw.Product);
                produceWareroom = belongToTown.Warehouse.FindOrCreate(requestor, Produce.Product);
                productionCanceler = belongToTown.Production.Add(this);
                consumeCanceler = belongToTown.Consume.Add(this);
                IsProcurement = false;
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

            Building ITownBuilding.BuildingInfo
            {
                get { return Parent; }
            }

            void IConsumable.OnConsume()
            {
                IsProcurement = rawWareroom.Remove(Raw.Number);
            }

            void IProducible.OnProduce()
            {
                if (IsProcurement)
                {
                    int yield = (int)(ProductInfos[Produce.Product].Production.YieldProportion * Produce.Number);
                    produceWareroom.Add(yield);
                }
            }

            void IDisposable.Dispose()
            {
                rawWareroom.Dispose();
                produceWareroom.Dispose();
                productionCanceler.Dispose();
                consumeCanceler.Dispose();
            }

        }

    }

    /// <summary>
    /// 单物品浮动产出的工厂;
    /// </summary>
    public class SingleProductDynamicFactory
    {

    }

}
