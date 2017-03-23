using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.World.Commerce;

namespace KouXiaGu.World
{

    /// <summary>
    /// 城镇信息;
    /// </summary>
    public class Town : IEquatable<Town>
    {

        public Town(int id)
        {
            TownID = id;
            Warehouse = new ProductWarehouse();
        }


        /// <summary>
        /// 唯一编号;
        /// </summary>
        public int TownID { get; private set; }

        /// <summary>
        /// 城镇所归属的国家;
        /// </summary>
        public Country Ascription { get; private set; }

        /// <summary>
        /// 城镇仓库;
        /// </summary>
        public ProductWarehouse Warehouse { get; private set; }

        /// <summary>
        /// 产品信息;
        /// </summary>
        public ProductInfoGroup ProductInfo
        {
            get { return Ascription.ProductInfo; }
        }

        /// <summary>
        /// 设置新的归属;
        /// </summary>
        public void SetAscription(Country ascription)
        {
            Ascription = ascription;
        }



        public bool Equals(Town other)
        {
            return other.TownID == this.TownID;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Town;

            if (item == null)
                return false;

            return Equals(item);
        }

        public override int GetHashCode()
        {
            return this.TownID;
        }

    }

}
