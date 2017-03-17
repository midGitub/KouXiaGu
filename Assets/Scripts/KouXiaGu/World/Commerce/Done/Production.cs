//using System;
//using System.Collections;
//using System.Collections.Generic;

//namespace KouXiaGu.World.Commerce
//{

//    /// <summary>
//    /// 生产信息;
//    /// </summary>
//    public interface IProductionInfo
//    {
//        /// <summary>
//        /// 产品类型;
//        /// </summary>
//        Product Product { get; }

//        /// <summary>
//        /// 每次更新的产量;
//        /// </summary>
//        int Yields { get; }
//    }

//    /// <summary>
//    /// 生产信息合集;
//    /// </summary>
//    public class ProductionInfoGroup : IEnumerable<IProductionInfo>
//    {
//        public ProductionInfoGroup()
//        {
//            productionInfos = new List<IProductionInfo>();
//            endProductionList = new List<IDisposable>();
//        }

//        List<IProductionInfo> productionInfos;
//        List<IDisposable> endProductionList;
//        Production production;

//        public bool IsEnable
//        {
//            get { return production != null; }
//        }

//        public void Add(IProductionInfo productionInfo)
//        {
//            productionInfos.Add(productionInfo);

//            if (IsEnable)
//            {
//                IDisposable endProduction = production.Create(productionInfo);
//                endProductionList.Add(endProduction);
//            }
//        }

//        public bool Remove(IProductionInfo productionInfo)
//        {
//            int index = productionInfos.FindIndex(item => item == productionInfo);

//            if (index >= 0)
//            {
//                IDisposable endProduction = endProductionList[index];
//                endProduction.Dispose();
//                productionInfos.RemoveAt(index);
//                endProductionList.RemoveAt(index);
//                return true;
//            }
//            return false;
//        }

//        public void Enable(Production production)
//        {
//            if (IsEnable)
//                throw new ArgumentException("已经启用;");

//            this.production = production;
//            for (int i = 0; i < productionInfos.Count; i++)
//            {
//                IProductionInfo productionInfo = productionInfos[i];
//                IDisposable endProduction = production.Create(productionInfo);
//                endProductionList.Add(endProduction);
//            }
//        }

//        public bool Disable()
//        {
//            if (IsEnable)
//            {
//                foreach (var item in endProductionList)
//                {
//                    item.Dispose();
//                }
//                endProductionList.Clear();
//                production = null;
//                return true;
//            }
//            return false;
//        }

//        public IEnumerator<IProductionInfo> GetEnumerator()
//        {
//            return ((IEnumerable<IProductionInfo>)this.productionInfos).GetEnumerator();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return ((IEnumerable<IProductionInfo>)this.productionInfos).GetEnumerator();
//        }

//    }

//    /// <summary>
//    /// 生产模块;
//    /// </summary>
//    public class Production
//    {
//        public Production()
//        {
//            productionLines = new LinkedList<ProductionLine>();
//        }

//        LinkedList<ProductionLine> productionLines;
//        public ProductWarehouse Warehouse { get; private set; }

//        public IDisposable Create(IProductionInfo production)
//        {
//            var productionLine = new ProductionLine(this, production);
//            return productionLine;
//        }

//        public void Update()
//        {
//            foreach (var productionLine in productionLines)
//            {
//                productionLine.Update();
//            }
//        }

//        /// <summary>
//        /// 生产线;
//        /// </summary>
//        class ProductionLine : IDisposable
//        {

//            public ProductionLine(Production production, IProductionInfo info)
//            {
//                this.production = production;

//                Wareroom = productWarehouse.FindOrCreate(info.Product);
//                occupyCanceler = Wareroom.Occupy(this);

//                node = productionLines.AddLast(this);
//            }

//            Production production;
//            LinkedListNode<ProductionLine> node;
//            IDisposable occupyCanceler;
//            public IWareroom Wareroom { get; private set; }
//            public IProductionInfo Production { get; private set; }

//            LinkedList<ProductionLine> productionLines
//            {
//                get { return production.productionLines; }
//            }

//            ProductWarehouse productWarehouse
//            {
//                get { return production.Warehouse; }
//            }

//            /// <summary>
//            /// 更新生产内容;
//            /// </summary>
//            public void Update()
//            {
//                Wareroom.Add(Production.Yields);
//            }

//            /// <summary>
//            /// 销毁这个生产线;
//            /// </summary>
//            public void Dispose()
//            {
//                if (node != null)
//                {
//                    occupyCanceler.Dispose();
//                    productionLines.Remove(node);
//                    node = null;
//                }
//            }

//        }

//    }

//}
