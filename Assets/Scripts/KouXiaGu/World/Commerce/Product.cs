using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.World
{


    /// <summary>
    /// 用于序列化的产品信息;
    /// </summary>
    [XmlType("Product")]
    public struct ProductType : IEquatable<ProductType>
    {

        public ProductType(int id, int categorieID) : this()
        {
            this.ProductID = id;
            this.CategorieID = categorieID;
        }

        /// <summary>
        /// 资源编号;
        /// </summary>
        [XmlAttribute("id")]
        public int ProductID { get; set; }

        /// <summary>
        /// 资源属于的类别(记录编号);
        /// </summary>
        [XmlAttribute("categorie")]
        public int CategorieID { get; set; }


        public ProductionInfo Production { get; set; }
        public SpoilInfo Spoil { get; set; }


        public override string ToString()
        {
            return base.ToString() + "[ProductID:" + ProductID + "]";
        }

        public bool Equals(ProductType other)
        {
            return other.ProductID == this.ProductID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ProductType))
                return false;

            return Equals((ProductType)obj);
        }

        public override int GetHashCode()
        {
            return ProductID;
        }

        public static bool operator ==(ProductType v1, ProductType v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(ProductType v1, ProductType v2)
        {
            return !v1.Equals(v2);
        }

    }


    /// <summary>
    /// 产品基类;
    /// </summary>
    public class Product : IEquatable<Product>
    {

        public Product(Product product) : this(product.Manager, product.Type)
        {
        }

        public Product(ProductManager manager, ProductType type)
        {
            this.Manager = manager;
            this.Type = type;
        }

        public ProductManager Manager { get; private set; }
        public ProductType Type { get; private set; }

        public int ProductID
        {
            get { return Type.ProductID; }
        }

        public ProductCategorie Categorie
        {
            get { return Manager.CategorieDictionary[Type.CategorieID]; }
        }

        public bool Equals(Product other)
        {
            return other.ProductID == this.ProductID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Product))
                return false;

            return Equals((Product)obj);
        }

        public override int GetHashCode()
        {
            return ProductID;
        }


        public static implicit operator int(Product v)
        {
            return v.ProductID;
        }

        public static bool operator ==(Product v1, Product v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Product v1, Product v2)
        {
            return !v1.Equals(v2);
        }

    }


    /// <summary>
    /// 产品信息,用于后期拓展内容的产品实例;
    /// </summary>
    public class ProductInfo : Product, IMonthObserver
    {

        public ProductInfo(Product product) : this(product.Manager, product.Type)
        {
        }

        public ProductInfo(ProductManager manager, ProductType type) : base(manager, type)
        {
            Production = new ProductProductionInfo(type.Production);
            Spoil = new ProductSpoilInfo(type.Spoil);
        }

        public ProductProductionInfo Production { get; private set; }
        public ProductSpoilInfo Spoil { get; private set; }

        void IMonthObserver.OnNext(Months month)
        {
            (Production as IMonthObserver).OnNext(month);
        }

    }


    /// <summary>
    /// 产品信息合集;比如存放单个 国家 科技组 的加成信息;
    /// </summary>
    public class ProductInfoGroup : IMonthObserver
    {

        public ProductInfoGroup()
        {
            infos = new Dictionary<int, ProductInfo>();
        }

        public ProductInfoGroup(IEnumerable<Product> items)
        {
            infos = CreateCollection(items);
        }

        public ProductInfoGroup(ProductManager info)
        {
            infos = CreateCollection(info.ProductDictionary.Values);
        }

        /// <summary>
        /// 产品信息合集;
        /// </summary>
        Dictionary<int, ProductInfo> infos;

        /// <summary>
        /// 产品信息合集;
        /// </summary>
        public IDictionary<int, ProductInfo> Infos
        {
            get { return infos; }
        }

        /// <summary>
        /// 获取这个产品的信息;
        /// </summary>
        public ProductInfo this[Product type]
        {
            get { return infos[type.ProductID]; }
        }

        /// <summary>
        /// 构建信息合集;
        /// </summary>
        Dictionary<int, ProductInfo> CreateCollection(IEnumerable<Product> items)
        {
            var collection = new Dictionary<int, ProductInfo>();

            foreach (var item in items)
            {
                var info = new ProductInfo(item);
                collection.Add(info.ProductID, info);
            }

            return collection;
        }

        void IMonthObserver.OnNext(Months month)
        {
            foreach (var info in infos.Values)
            {
                (info as IMonthObserver).OnNext(month);
            }
        }

    }

}
