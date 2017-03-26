using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.World.Commerce;

namespace KouXiaGu.World
{

    /// <summary>
    /// 归属于国家的;
    /// </summary>
    public abstract class National
    {
        public National(Country belongToCountry)
        {
            this.BelongToCountry = belongToCountry;
        }

        /// <summary>
        /// 所归属的国家;
        /// </summary>
        public Country BelongToCountry { get; private set; }

        /// <summary>
        /// 所使用的产品信息;
        /// </summary>
        public ProductInfoGroup ProductInfos
        {
            get { return BelongToCountry.ProductInfo; }
        }

        /// <summary>
        /// 设置新的归属;
        /// </summary>
        public virtual void SetAscription(Country belongToCountry)
        {
            BelongToCountry = belongToCountry;
        }

    }

    /// <summary>
    /// 归属于城镇的;
    /// </summary>
    public abstract class Townish
    {

        public Townish(Town belongToTown)
        {
            BelongToTown = belongToTown;
        }

        /// <summary>
        /// 所归属的城镇;
        /// </summary>
        public Town BelongToTown { get; private set; }

        /// <summary>
        /// 所归属的国家;
        /// </summary>
        public Country BelongToCountry
        {
            get { return BelongToTown.BelongToCountry; }
        }

        /// <summary>
        /// 所使用的产品信息;
        /// </summary>
        public ProductInfoGroup ProductInfo
        {
            get { return BelongToCountry.ProductInfo; }
        }

    }


    /// <summary>
    /// 城镇信息;
    /// </summary>
    public class Town : National, IEquatable<Town>
    {

        public Town(int id, Country belongToCountry) : base(belongToCountry)
        {
            TownID = id;
            Warehouse = new ProductWarehouse(belongToCountry);
            Production = new Production(this);
        }

        /// <summary>
        /// 唯一编号;
        /// </summary>
        public int TownID { get; private set; }

        /// <summary>
        /// 仓库;
        /// </summary>
        public ProductWarehouse Warehouse { get; private set; }

        /// <summary>
        /// 生产;
        /// </summary>
        public Production Production { get; private set; }

        /// <summary>
        /// 消耗;
        /// </summary>
        public ProductConsume Consume { get; private set; }

        /// <summary>
        /// 每日更新项目;
        /// </summary>
        public void DayUpdate()
        {
            Warehouse.DayUpdate();
            Production.DayUpdate();
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
