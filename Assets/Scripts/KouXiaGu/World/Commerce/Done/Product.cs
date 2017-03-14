using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 资源信息;
    /// </summary>
    public class Product : IEquatable<Product>
    {

        /// <summary>
        /// 资源编号;
        /// </summary>
        public int ProductID { get; private set; }

        /// <summary>
        /// 资源属于的类别;
        /// </summary>
        public ProductCategorie Categorie { get; private set; }


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
