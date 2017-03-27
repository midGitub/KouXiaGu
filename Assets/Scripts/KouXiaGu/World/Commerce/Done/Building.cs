using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    public class Building : IEquatable<Building>
    {

        public Building(int id)
        {
            BuildingID = id;
        }

        /// <summary>
        /// 编号;
        /// </summary>
        public int BuildingID { get; private set; }

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
    /// 工厂;
    /// </summary>
    public class Factory
    {

    }

    /// <summary>
    /// 单物品产出的工厂;
    /// </summary>
    public class SingleProductFactory : IProducible, IDisposable
    {
        public SingleProductFactory(
            Container<Product> product,
            IRequestor requestor,
            ProductWarehouse warehouse,
            ProductProduction production)
        {
            if (product.Number <= 0)
                throw new ArgumentException();

            ProductContainer = product;
            wareroom = warehouse.FindOrCreate(requestor, product.Product);
            productionCanceler = production.Add(this);
        }

        public Container<Product> ProductContainer { get; private set; }
        IWareroom wareroom;
        IDisposable productionCanceler;

        void IProducible.OnProduce()
        {
            wareroom.Add(ProductContainer.Number);
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

    /// <summary>
    /// 多个产品产出的工厂;
    /// </summary>
    public class ProductFactory : IProducible
    {
        public void OnProduce()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 将这类产品转换成另外一种产品的工厂;
    /// </summary>
    public class ConvertedFactory : IProducible
    {
        public void OnProduce()
        {
            throw new NotImplementedException();
        }
    }

}
