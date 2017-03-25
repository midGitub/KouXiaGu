using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 工厂;
    /// </summary>
    public class Factory : Building
    {

        public Factory(int id, IEnumerable<ProductContainer> output) : base(id)
        {
            this.output = new List<ProductContainer>(output);
        }

        List<ProductContainer> output;

        /// <summary>
        /// 产出;
        /// </summary>
        public IEnumerable<ProductContainer> Output
        {
            get { return output; }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Factory v1, Factory v2)
        {
            return v1.BuildingID == v2.BuildingID;
        }

        public static bool operator !=(Factory v1, Factory v2)
        {
            return v1.BuildingID != v2.BuildingID;
        }

    }


    /// <summary>
    /// 负责城镇生产;
    /// </summary>
    public class Production : Townish
    {

        public Production(Town belongToTown) : base(belongToTown)
        {
            if (belongToTown == null)
                throw new ArgumentNullException();

            productionLines = new List<ProductionLine>();
            factorys = new List<FactoryItem>();
        }

        List<ProductionLine> productionLines;
        List<FactoryItem> factorys;

        /// <summary>
        /// 所以已启用的工厂类型;
        /// </summary>
        public IEnumerable<Factory> Factorys
        {
            get { return factorys.Select(item => item.Factory); }
        }

        /// <summary>
        /// 城镇仓库;
        /// </summary>
        public ProductWarehouse Warehouse
        {
            get { return BelongToTown.Warehouse; }
        }

        /// <summary>
        /// 添加生产内容,若已经存在则返回false;
        /// </summary>
        public bool Add(Factory factory)
        {
            if (factorys.First(i => i.Factory == factory) != null)
                return false;

            FactoryItem item = new FactoryItem(this, factory);
            factorys.Add(item);
            return true;
        }

        /// <summary>
        /// 移除这个生产内容,不存在返回false;
        /// </summary>
        public bool Remove(Factory factory)
        {
            int index = factorys.FindIndex(i => i.Factory == factory);

            if (index >= 0)
            {
                FactoryItem item = factorys[index];
                item.Dispose();
                factorys.RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 将生产内容加到库房;
        /// </summary>
        public void Produce()
        {
            foreach (var productionLine in productionLines)
            {
                productionLine.Produce();
            }
        }

        /// <summary>
        /// 找到对应的生产线,若找不到则创建到;
        /// </summary>
        ProductionLine FindOrCreate(Product product)
        {
            var item = productionLines.Find(i => i.Product == product);

            if (item == null)
            {
                item = new ProductionLine(Warehouse, product);
                productionLines.Add(item);
            }

            return item;
        }


        class FactoryItem : IDisposable
        {
            public FactoryItem(Production production, Factory factory)
            {
                Production = production;
                Factory = factory;
                Initialize();
            }

            List<IDisposable> disposers;
            public Production Production { get; private set; }
            public Factory Factory { get; private set; }

            void Initialize()
            {
                disposers = new List<IDisposable>();

                foreach (var productInfo in Factory.Output)
                {
                    var productionLine = Production.FindOrCreate(productInfo);
                    var disposer = productionLine.Increase(productInfo.Count);
                    disposers.Add(disposer);
                }
            }

            public void Dispose()
            {
                if (disposers != null)
                {
                    foreach (var item in disposers)
                    {
                        item.Dispose();
                    }

                    disposers = null;
                    Production = null;
                    Factory = null;
                }
            }
        }

        class ProductionLine : IDisposable
        {
            public ProductionLine(ProductWarehouse warehouse, Product product)
            {
                Product = product;
                Wareroom = warehouse.FindOrCreate(this, product);
                Number = 0;
            }

            public Product Product { get; private set; }
            public IWareroom Wareroom { get; private set; }
            public int Number { get; private set; }

            /// <summary>
            /// 增加产量;
            /// </summary>
            public IDisposable Increase(int number)
            {
                return new Canceler(this, number);
            }

            public void Produce()
            {
                Wareroom.Add(Number);
            }

            public void Dispose()
            {
                if (Wareroom != null)
                {
                    Wareroom.Dispose();
                    Wareroom = null;
                }
            }

            void Add(int number)
            {
                Number += number;
            }

            void Remove(int number)
            {
                Number -= number;
            }

            class Canceler : IDisposable
            {
                public Canceler(ProductionLine productionLine, int number)
                {
                    ProductionLine = productionLine;
                    Number = number;
                    ProductionLine.Add(number);
                }

                public ProductionLine ProductionLine { get; private set; }
                public int Number { get; private set; }

                public void Dispose()
                {
                    if (ProductionLine != null)
                    {
                        ProductionLine.Remove(Number);
                        ProductionLine = null;
                    }
                }
            }

        }

    }


}
