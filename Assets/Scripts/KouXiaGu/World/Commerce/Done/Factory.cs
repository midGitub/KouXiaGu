using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.World.Commerce
{

    public class ProductTransformPremise
    {

        public ProductTransformPremise()
        {
            Required = new List<ProductBox>();
            Product = new List<ProductBox>();
        }

        public ProductTransformPremise(IEnumerable<ProductBox> required, IEnumerable<ProductBox> product)
        {
            Required = new List<ProductBox>(required);
            Product = new List<ProductBox>(product);
        }

        /// <summary>
        /// 需要的产品;
        /// </summary>
        public List<ProductBox> Required { get; private set; }

        /// <summary>
        /// 产品的产品;
        /// </summary>
        public List<ProductBox> Product { get; private set; }

    }


    /// <summary>
    /// 转换产品到另一种产品;
    /// </summary>
    public class ProductTransformation
    {

        public ProductTransformation(ProductWarehouse warehouse, ProductTransformPremise premise)
        {
            MaxProduct = int.MaxValue;
            this.required = this.CreateList(warehouse, premise.Required);
            this.product = this.CreateList(warehouse, premise.Product);
        }

        /// <summary>
        /// 需要的产品;
        /// </summary>
        List<ProductItem> required;

        /// <summary>
        /// 产品的产品;
        /// </summary>
        List<ProductItem> product;

        /// <summary>
        /// 最大转换次数;
        /// </summary>
        public int MaxProduct { get; set; }

        /// <summary>
        /// 需求;
        /// </summary>
        public IEnumerable<ProductBox> Required
        {
            get { return required.Select(item => item.Box); }
        }

        /// <summary>
        /// 产出;
        /// </summary>
        public IEnumerable<ProductBox> Product
        {
            get { return product.Select(item => item.Box); }
        }


        List<ProductItem> CreateList(ProductWarehouse warehouse, IEnumerable<ProductBox> items)
        {
            List<ProductItem> list = new List<ProductItem>();

            foreach (var item in items)
            {
                list.Add(new ProductItem(item, warehouse));
            }
            return list;
        }

        /// <summary>
        /// 进行转换;
        /// </summary>
        public void Transform()
        {
            int number = GetNumberOfProduction(required, MaxProduct);

            foreach (var required in required)
            {
                if(!required.Remove(number))
                {
                    throw new ArgumentException("无法移除所需的资源");
                }
            }

            foreach (var product in product)
            {
                product.Add(number);
            }

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

            public ProductItem(ProductBox box, ProductWarehouse warehouse)
            {
                this.Box = box;
                this.Wareroom = warehouse.FindOrCreate(box.Product);
                this.occupyCanceler = Wareroom.Occupy(this);
            }

            public ProductBox Box { get; private set; }
            public IWareroom Wareroom { get; private set; }
            IDisposable occupyCanceler;

            /// <summary>
            /// 增加产品;
            /// </summary>
            public void Add(int multiple)
            {
                Wareroom.Add(multiple * Box.Number);
            }

            /// <summary>
            /// 移除产品,若无法移除则返回false;
            /// </summary>
            public bool Remove(int multiple)
            {
                return Wareroom.TryRemove(multiple * Box.Number);
            }

            public void Dispose()
            {
                occupyCanceler.Dispose();
            }

        }

    }

    /// <summary>
    /// 产品 和 数量;
    /// </summary>
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
