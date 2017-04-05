using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{

    /// <summary>
    /// 国家信息;
    /// </summary>
    public class Country : IEquatable<Country>
    {

        public Country(int id, WorldManager manager)
        {
            CountryID = id;
            ProductInfo = new ProductInfoGroup(manager.Product);
            ManpowerFactor = new ManpowerNationalFactor(manager.Info);
        }


        /// <summary>
        /// 唯一编号;
        /// </summary>
        public int CountryID { get; private set; }

        /// <summary>
        /// 产品信息;
        /// </summary>
        public ProductInfoGroup ProductInfo { get; private set; }

        /// <summary>
        /// 人口影响因素;
        /// </summary>
        public ManpowerNationalFactor ManpowerFactor { get; private set; }


        public bool Equals(Country other)
        {
            return other.CountryID == CountryID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Country))
                return false;
            return Equals((Country)obj);
        }

        public override int GetHashCode()
        {
            return CountryID;
        }

    }

}
