using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.World.Commerce
{

    public interface IProductionInfo
    {
        /// <summary>
        /// 产品类型;
        /// </summary>
        Product Product { get; }

        /// <summary>
        /// 每次更新的产量;
        /// </summary>
        int Capacity { get; }
    }

    /// <summary>
    /// 生产模块;
    /// </summary>
    public class Production
    {
        public Production()
        {
            productionLines = new LinkedList<ProductionLine>();
        }

        LinkedList<ProductionLine> productionLines;
        public ProductWarehouse Warehouse { get; private set; }

        public IDisposable Create(IProductionInfo production)
        {
            var productionLine = new ProductionLine(this, production);
            return productionLine;
        }

        public void Update()
        {
            foreach (var productionLine in productionLines)
            {
                productionLine.Update();
            }
        }

        /// <summary>
        /// 生产线;
        /// </summary>
        class ProductionLine : IDisposable
        {

            public ProductionLine(Production production, IProductionInfo info)
            {
                this.production = production;

                Wareroom = productWarehouse.FindOrCreate(info.Product);
                occupyCanceler = Wareroom.Occupy(this);

                node = productionLines.AddLast(this);
            }

            Production production;
            LinkedListNode<ProductionLine> node;
            IDisposable occupyCanceler;
            public IWareroom Wareroom { get; private set; }
            public IProductionInfo Production { get; private set; }

            LinkedList<ProductionLine> productionLines
            {
                get { return production.productionLines; }
            }

            ProductWarehouse productWarehouse
            {
                get { return production.Warehouse; }
            }

            /// <summary>
            /// 更新生产内容;
            /// </summary>
            public void Update()
            {
                Wareroom.Add(Production.Capacity);
            }

            /// <summary>
            /// 销毁这个生产线;
            /// </summary>
            public void Dispose()
            {
                if (node != null)
                {
                    occupyCanceler.Dispose();
                    productionLines.Remove(node);
                    node = null;
                }
            }

        }

    }

}
