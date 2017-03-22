using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.World.Commerce
{


    /// <summary>
    /// 资源分类信息;
    /// </summary>
    [XmlType("ProductCategorie")]
    public struct ProductCategorieType : IEquatable<ProductCategorieType>
    {

        public ProductCategorieType(int id) : this()
        {
            this.CategorieID = id;
        }

        /// <summary>
        /// 资源属于的类别(记录编号);
        /// </summary>
        [XmlAttribute("id")]
        public int CategorieID { get; set; }


        public override string ToString()
        {
            return base.ToString() + "[CategorieID:" + CategorieID + "]";
        }

        public bool Equals(ProductCategorieType other)
        {
            return other.CategorieID == this.CategorieID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ProductCategorieType))
                return false;

            return Equals((ProductCategorieType)obj);
        }

        public override int GetHashCode()
        {
            return CategorieID;
        }

        public static bool operator ==(ProductCategorieType v1, ProductCategorieType v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(ProductCategorieType v1, ProductCategorieType v2)
        {
            return !v1.Equals(v2);
        }


        #region 本地化;

        /// <summary>
        /// 名字的本地化标识;
        /// </summary>
        [XmlElement("Name")]
        public string LocalizationNameID { get; set; }

        #endregion

    }


    /// <summary>
    /// 资源类别;
    /// </summary>
    public class ProductCategorie : IEquatable<ProductCategorie>
    {

        internal ProductCategorie(ProductManager manager, ProductCategorie type)
        {
            this.Manager = manager;
            this.Type = type;
        }

        public ProductManager Manager { get; private set; }
        public ProductCategorie Type { get; private set; }

        /// <summary>
        /// 编号;
        /// </summary>
        public int CategorieID
        {
            get { return Type.CategorieID; }
        }

        public bool Equals(ProductCategorie other)
        {
            return other.CategorieID == this.CategorieID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ProductCategorie))
                return false;

            return Equals((ProductCategorie)obj);
        }

        public override int GetHashCode()
        {
            return CategorieID;
        }

        public static bool operator ==(ProductCategorie v1, ProductCategorie v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(ProductCategorie v1, ProductCategorie v2)
        {
            return !v1.Equals(v2);
        }

    }


}
