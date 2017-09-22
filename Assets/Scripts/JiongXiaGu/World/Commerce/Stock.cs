using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.World.Commerce
{

    public class Stock
    {
        public Stock()
        {
            stockList = new List<ProductStock>();
        }

        List<ProductStock> stockList;

        /// <summary>
        /// 根据下标获取到库存;
        /// </summary>
        public ProductStock this[int index]
        {
            get { return stockList[index]; }
        }

        /// <summary>
        /// 创建库存,若已经存在则异常;
        /// </summary>
        public ProductStock Create(int productType, int number)
        {
            ProductStock stock = Find(productType);
            if (stock == null)
            {
                stock = new ProductStock(this, productType, number);
                stockList.Add(stock);
                return stock;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public bool Remove(int productType)
        {
            int index = stockList.FindIndex(item => item.ProductType == productType);
            if (index >= 0)
            {
                stockList.RemoveAt(index);
                return true;
            }
            return false;
        }

        public bool Remove(ProductStock stock)
        {
            if (stock.Parent != this)
            {
                throw new InvalidOperationException();
            }
            stock.Parent = null;
            stock.ProductType = default(int);
            stock.Count = default(int);
            return stockList.Remove(stock);
        }

        public ProductStock Find(int productType)
        {
            return stockList.Find(item => item.ProductType == productType);
        }
    }

    public class ProductStock
    {
        internal ProductStock(Stock parent, int productType, int count)
        {
            Parent = parent;
            ProductType = productType;
            Count = count;
        }

        public Stock Parent { get; internal set; }
        public int ProductType { get; set; }
        public int Count { get; set; }
    }
}
