using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{


    /// <summary>
    /// 产出产品;
    /// </summary>
    public interface IProduce
    {
        /// <summary>
        /// 产品类型;
        /// </summary>
        Product Product { get; }

        /// <summary>
        /// 每次更新的产量;
        /// </summary>
        int Yields { get; }
    }

    /// <summary>
    /// 转换产品到另一种产品;
    /// </summary>
    public class TransformProduct
    {

        /// <summary>
        /// 需要的产品;
        /// </summary>
        List<ProductItem> requiredProducts;

        /// <summary>
        /// 产品的产品;
        /// </summary>
        List<ProductItem> productProducts;

        /// <summary>
        /// 
        /// </summary>
        public int MaxProduct { get; private set; }

        public void Product()
        {
            int numer = GetNumberOfProduction(requiredProducts, MaxProduct);
        }

        /// <summary>
        /// 获取到最大可转换的数目;
        /// </summary>
        int GetNumberOfProduction(IEnumerable<ProductItem> products, int maxNumber)
        {
            int number = maxNumber;

            foreach (var item in products)
            {
                int produce = item.Wareroom.Total / item.Box.Number;
                if (number > produce)
                    number = produce;
            }

            return number;
        }


        class ProductItem : IDisposable
        {

            public ProductItem(ProductBox Box, ProductWarehouse Warehouse)
            {
                this.Box = Box;
                this.Wareroom = Warehouse.FindOrCreate(Box.Product);
                this.occupyCanceler = Wareroom.Occupy(this);
            }

            public ProductBox Box { get; private set; }
            public IWareroom Wareroom { get; private set; }
            IDisposable occupyCanceler;

            public void Dispose()
            {
                occupyCanceler.Dispose();
            }
        }

    }


    public class ProductBox
    {
        public ProductBox(Product Product, int Number)
        {
            this.Product = Product;
            this.Number = Number;
        }

        public Product Product { get; private set; }
        public int Number { get; private set; }

    }


    //public class Equivalent
    //{
    //    /// <summary>
    //    /// 需要的产品;
    //    /// </summary>
    //    List<ProductBox> requiredProducts;

    //    /// <summary>
    //    /// 产品的产品;
    //    /// </summary>
    //    List<ProductBox> productProducts;

    //}


    public class Factory
    {



        public class ProductionLine
        {
            public ProductionLine(Product product, IWareroom wareroom)
            {
                this.BasicYields = 0;
                productionInfos = new List<IProduce>();
                this.Product = product;
                this.Wareroom = wareroom;
            }

            /// <summary>
            /// 基础产量;
            /// </summary>
            public int BasicYields { get; private set; }
            List<IProduce> productionInfos;
            public Product Product { get; private set; }
            public IWareroom Wareroom { get; private set; }

            public ProductCategorie Categorie
            {
                get { return Product.Categorie; }
            }

            /// <summary>
            /// 添加生产项目;
            /// </summary>
            public IDisposable Add(IProduce productionInfo)
            {
                productionInfos.Add(productionInfo);
                BasicYields += productionInfo.Yields;

                throw new NotImplementedException();
            }

            /// <summary>
            /// 获取到产量;
            /// </summary>
            public int GetYields(Months month)
            {
                if (IsProductionMonth(month))
                {
                    return (int)(
                        BasicYields * 
                        Product.ProportionOfProduction * 
                        Categorie.ProportionOfProduction);
                }
                else
                {
                    return (int)(
                        BasicYields *
                        Product.ProportionOfProduction *
                        Categorie.ProportionOfProduction *
                        Product.NonSeasonalPercent);
                }
            }

            /// <summary>
            /// 这个月份是否为合适产出的月份?
            /// </summary>
            bool IsProductionMonth(Months month)
            {
                int temp = (int)(Product.MonthOfProduction & month);
                return temp >= 1;
            }


        }

    }

}
