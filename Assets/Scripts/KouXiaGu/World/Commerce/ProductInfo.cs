using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace KouXiaGu.World
{


    [XmlType("Production")]
    public struct ProductionInfo
    {
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

    }

    /// <summary>
    /// 产品生产信息;
    /// </summary>
    public class ProductProductionInfo : IMonthObserver
    {

        public ProductProductionInfo(ProductionInfo info)
        {
            ProportionOfProduction = new ProportionItems(1);
            MonthOfProduction = info.MonthOfProduction;
            NonSeasonalPercent = new ProportionItems(info.NonSeasonalPercent);

            yieldProportion = GetYieldProportion;
        }

        /// <summary>
        /// 生产加成\比例,默认为 1;
        /// </summary>
        public ProportionItems ProportionOfProduction { get; private set; }

        /// <summary>
        /// 非季节产出比例;
        /// </summary>
        public ProportionItems NonSeasonalPercent { get; private set; }

        /// <summary>
        /// 在这些月份正常产出;
        /// </summary>
        public Months MonthOfProduction { get; private set; }

        /// <summary>
        /// 产量加成;取值范围 0 ~ max
        /// </summary>
        public float YieldProportion
        {
            get { return Math.Max(0, yieldProportion()); }
        }

        /// <summary>
        /// 得到产出的方法;
        /// </summary>
        event Func<float> yieldProportion;


        void IMonthObserver.OnNext(Months item)
        {
            if (IsProductionMonth(item))
                yieldProportion = GetYieldProportion;
            else
                yieldProportion = GetNonSeasonalYieldProportion;
        }

        /// <summary>
        /// 这个月份是否为合适产出的月份?
        /// </summary>
        bool IsProductionMonth(Months month)
        {
            int temp = (int)(MonthOfProduction & month);
            return temp >= 1;
        }

        /// <summary>
        /// 符合季节产出的产量比例;
        /// </summary>
        float GetYieldProportion()
        {
            return ProportionOfProduction;
        }

        /// <summary>
        /// 不符合季节产出的产量比例;
        /// </summary>
        float GetNonSeasonalYieldProportion()
        {
            return ProportionOfProduction * NonSeasonalPercent;
        }

    }


    [XmlType("Spoil")]
    public struct SpoilInfo
    {
        /// <summary>
        /// 每日损失的比例 0 ~ 1;
        /// </summary>
        [XmlElement("SpoilPercent")]
        public float SpoilPercent { get; set; }

    }

    /// <summary>
    /// 产品腐坏信息;
    /// </summary>
    public class ProductSpoilInfo
    {
        public ProductSpoilInfo(SpoilInfo info)
        {
            SpoilItems = new ProportionItems(info.SpoilPercent);
        }

        /// <summary>
        /// 每日损失\腐坏的比例 条目合集;
        /// </summary>
        public ProportionItems SpoilItems { get; private set; }

        /// <summary>
        /// 每日损失\腐坏的比例,取值范围 0 ~ 1;
        /// </summary>
        public float SpoilPercent
        {
            get { return MathI.Clamp01(SpoilItems); }
        }

    }

}
