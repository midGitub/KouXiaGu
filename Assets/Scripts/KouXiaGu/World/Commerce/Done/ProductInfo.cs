using System;
using System.Collections.Generic;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// ProductInfo 合集;
    /// </summary>
    public class ProductInfoGroup : IMonthObserver
    {

        public ProductInfoGroup(IEnumerable<Product> items)
        {
            infos = CreateCollection(items);
        }

        /// <summary>
        /// 产品信息合集;
        /// </summary>
        Dictionary<int, ProductInfo> infos;

        /// <summary>
        /// 产品信息合集;
        /// </summary>
        public IDictionary<int, ProductInfo> Infos
        {
            get { return infos; }
        }

        /// <summary>
        /// 构建信息合集;
        /// </summary>
        Dictionary<int, ProductInfo> CreateCollection(IEnumerable<Product> items)
        {
            var collection = new Dictionary<int, ProductInfo>();

            foreach (var item in items)
            {
                var info = new ProductInfo(item);
                collection.Add(info.ProductID, info);
            }

            return collection;
        }

        void IMonthObserver.OnNext(Months month)
        {
            foreach (var info in infos.Values)
            {
                (info as IMonthObserver).OnNext(month);
            }
        }

    }

    /// <summary>
    /// 产品信息;
    /// </summary>
    public class ProductInfo : Product, IMonthObserver
    {

        public ProductInfo(Product product) : base(product)
        {
            Production = new ProductProductionInfo(product);
        }

        /// <summary>
        /// 生产信息;
        /// </summary>
        public ProductProductionInfo Production { get; private set; }

        //#region 本地化;

        ///// <summary>
        ///// 产品名字的本地化标识;
        ///// </summary>
        //public string LocalizationNameID
        //{
        //    get { return Type.LocalizationNameID; }
        //}

        ///// <summary>
        ///// 产品描述的本地化标识;
        ///// </summary>
        //public string LocalizationDescID
        //{
        //    get { return Type.LocalizationDescID; }
        //}

        //#endregion


        //#region 商业;

        ///// <summary>
        ///// 价值;1个单位的产品对应的价值;
        ///// </summary>
        //public int Worth
        //{
        //    get { return Type.Worth; }
        //}

        //#endregion

        void IMonthObserver.OnNext(Months month)
        {
            (Production as IMonthObserver).OnNext(month);
        }

    }

    /// <summary>
    /// 产品产出信息;
    /// </summary>
    public class ProductProductionInfo : Product, IMonthObserver
    {
        public ProductProductionInfo(Product product) : base(product)
        {
            ProportionOfProduction = new ProportionItems(1);
            SpoilPercent = new ProportionItems(Type.SpoilPercent);
            MonthOfProduction = Type.MonthOfProduction;
            NonSeasonalPercent = new ProportionItems(Type.NonSeasonalPercent);
        }

        /// <summary>
        /// 每日损失\腐坏的比例;
        /// </summary>
        public ProportionItems SpoilPercent { get; private set; }

        /// <summary>
        /// 生产加成\比例,默认为 1;
        /// </summary>
        public ProportionItems ProportionOfProduction { get; private set; }

        /// <summary>
        /// 在这些月份正常产出;
        /// </summary>
        public Months MonthOfProduction { get; private set; }

        /// <summary>
        /// 非季节产出比例;
        /// </summary>
        public ProportionItems NonSeasonalPercent { get; private set; }

        /// <summary>
        /// 产量加成;
        /// </summary>
        public float YieldProduction
        {
            get { return yieldProportion(); }
        }

        /// <summary>
        /// 得到产出的方法;
        /// </summary>
        event Func<float> yieldProportion;


        void IMonthObserver.OnNext(Months item)
        {
            if (IsProductionMonth(item))
                yieldProportion = YieldProportion;
            else
                yieldProportion = NonSeasonalYieldProportion;
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
        float YieldProportion()
        {
            return ProportionOfProduction * Categorie.ProportionOfProduction;
        }

        /// <summary>
        /// 不符合季节产出的产量比例;
        /// </summary>
        float NonSeasonalYieldProportion()
        {
            return ProportionOfProduction * Categorie.ProportionOfProduction * NonSeasonalPercent;
        }

    }

}
