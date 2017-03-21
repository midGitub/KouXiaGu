using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 表示 产品 和其 数量;
    /// </summary>
    public struct ProductContainer : IEquatable<ProductContainer>
    {
        public ProductContainer(Product product, int count)
        {
            this.Product = product;
            this.Count = count;
        }

        public Product Product { get; private set; }
        public int Count { get; private set; }

        public bool Equals(ProductContainer other)
        {
            return
                this.Product == other.Product &&
                this.Count == other.Count;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ProductContainer))
                return false;

            return Equals((ProductContainer)obj);
        }

        public override int GetHashCode()
        {
            return Product.GetHashCode() ^ Count.GetHashCode();
        }

        public override string ToString()
        {
            return "[Product:" + Product.ToString() + ",Count:" + Count + "]";
        }

    }

}
