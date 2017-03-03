using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 产品信息;
    /// </summary>
    public class Product : IEquatable<Product>
    {

        /// <summary>
        /// 资源编号;
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 可生产月份;
        /// </summary>
        public Months ProductionDate { get; private set; }


        /// <summary>
        /// 生产效率; 0 ~ max;
        /// </summary>
        public int Productivity { get; private set; }

        /// <summary>
        /// 升一级的最大工人数量改变;
        /// </summary>
        public int MaxProducerLevel { get; private set; }



        /// <summary>
        /// 资源属于的类别;
        /// </summary>
        public ProductCategorie Categorie { get; set; }


        public bool Equals(Product other)
        {
            return other.ID == this.ID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Product))
                return false;

            return Equals((Product)obj);
        }

        public override int GetHashCode()
        {
            return ID;
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
