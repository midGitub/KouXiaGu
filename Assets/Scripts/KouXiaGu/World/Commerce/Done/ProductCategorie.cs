using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 资源类别;
    /// </summary>
    public class ProductCategorie : IEquatable<ProductCategorie>
    {

        /// <summary>
        /// 编号;
        /// </summary>
        public int CategorieID { get; private set; }

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
