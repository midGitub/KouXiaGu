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
                this.BasicYields = 0;
                productionInfos = new List<IProductionInfo>();
                this.Product = product;
                this.Wareroom = wareroom;
            }

            /// <summary>
            /// 基础产量;
            /// </summary>
            public int BasicYields { get; private set; }
            List<IProductionInfo> productionInfos;
            public Product Product { get; private set; }
            public IWareroom Wareroom { get; private set; }

            public ProductCategorie Categorie
            {
                get { return Product.Categorie; }
            }

            /// <summary>
            /// 添加生产项目;
            /// </summary>
            public IDisposable Add(IProductionInfo productionInfo)
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
