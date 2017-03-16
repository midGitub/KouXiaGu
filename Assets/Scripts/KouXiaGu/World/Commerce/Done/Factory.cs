using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{



    public class Factory
    {



        public class ProductionLine
        {
            public ProductionLine(Product product, IWareroom wareroom)
            {
                this.Product = product;
                this.Wareroom = wareroom;
                productionInfos = new List<IProductionInfo>();
            }


            List<IProductionInfo> productionInfos;
            public Product Product { get; private set; }
            public IWareroom Wareroom { get; private set; }

            int production;

            /// <summary>
            /// 获取到产量;
            /// </summary>
            public int GetProduction(Months month)
            {
                if (IsProductionMonth(month))
                {
                    return (int)(
                        production * 
                        Product.ProportionOfProduction * 
                        Product.Categorie.ProportionOfProduction);
                }
                else
                {
                    return (int)(
                        production *
                        Product.ProportionOfProduction *
                        Product.Categorie.ProportionOfProduction *
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
