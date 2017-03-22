using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.World.Commerce
{


    /// <summary>
    /// 资源信息;
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


        #region 本地化;

        /// <summary>
        /// 名字的本地化标识;
        /// </summary>
        [XmlElement("Name")]
        public string LocalizationNameID { get; set; }

        /// <summary>
        /// 描述的本地化标识;
        /// </summary>
        [XmlElement("Description")]
        public string LocalizationDescID { get; set; }

        #endregion


        #region 生产;

        /// <summary>
        /// 每日损失的比例 0 ~ 1;
        /// </summary>
        [XmlElement("SpoilPercent")]
        public float SpoilPercent { get; set; }

        /// <summary>
        /// 在这些月份正常产出;
        /// </summary>
        [XmlElement("MonthOfProduction")]
        public Months MonthOfProduction { get; set; }

        /// <summary>
        /// 非季节产出比例 0 ~ 1;
        /// </summary>
        [XmlElement("NonSeasonalPercent")]
        public float NonSeasonalPercent { get; set; }

        #endregion


        #region 商业;

        /// <summary>
        /// 价值;1个单位的产品对应的价值;
        /// </summary>
        [XmlElement("Worth")]
        public int Worth { get; set; }

        #endregion

    }

    /// <summary>
    /// 资源信息;
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

        /// <summary>
        /// 资源编号;
        /// </summary>
        public int ProductID
        {
            get { return Type.ProductID; }
        }

        /// <summary>
        /// 资源属于的类别;
        /// </summary>
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

        public static bool operator ==(Product v1, Product v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Product v1, Product v2)
        {
            return !v1.Equals(v2);
        }

    }


}
