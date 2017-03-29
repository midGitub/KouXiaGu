using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 表示 产品 和 数量;(泛型版本)
    /// </summary>
    public struct Container<TKey> : IEquatable<Container<TKey>>
    {
        public Container(TKey product, int count)
        {
            this.Product = product;
            this.Number = count;
        }

        public TKey Product { get; private set; }
        public int Number { get; private set; }

        public bool Equals(Container<TKey> other)
        {
            return
                Object.Equals(Product, other.Product) &&
                Number == other.Number;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Container<TKey>))
                return false;

            return Equals((Container<TKey>)obj);
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }

        public override string ToString()
        {
            return "[Product:" + Product.ToString() + ",Count:" + Number + "]";
        }


        public static implicit operator TKey(Container<TKey> v)
        {
            return v.Product;
        }

    }


    ///// <summary>
    ///// 表示 产品 和 数量;
    ///// </summary>
    //[Obsolete]
    //public struct Container : IEquatable<Container>
    //{
    //    public Container(Product product, int count)
    //    {
    //        this.Product = product;
    //        this.Number = count;
    //    }

    //    public Product Product { get; private set; }
    //    public int Number { get; private set; }

    //    public bool Equals(Container other)
    //    {
    //        return
    //           Object.Equals(Product, other.Product) &&
    //           Object.Equals(Number, other.Number);
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (!(obj is Container))
    //            return false;

    //        return Equals((Container)obj);
    //    }

    //    public override int GetHashCode()
    //    {
    //        return Product.GetHashCode() ^ Number.GetHashCode();
    //    }

    //    public override string ToString()
    //    {
    //        return "[Product:" + Product.ToString() + ",Count:" + Number + "]";
    //    }

    //    public static implicit operator Product(Container v)
    //    {
    //        return v.Product;
    //    }

    //}

}
