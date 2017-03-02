using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 指定资源的成品;
    /// </summary>
    public class ProductRoom
    {

        public ProductRoom(int resourceType)
        {
            this.ResourceType = resourceType;

            this.Capacity = 0;
            this.ProductNumber = 0;
        }


        /// <summary>
        /// 资源类型;
        /// </summary>
        public int ResourceType { get; private set; }

        /// <summary>
        /// 最大容量;
        /// </summary>
        public uint Capacity { get; set; }

        /// <summary>
        /// 当前的数目;
        /// </summary>
        public uint ProductNumber { get; set; }


        /// <summary>
        /// 添加产品到仓库,若超出仓库容量则丢弃超出的部分;
        /// </summary>
        public void AddProduct(uint number)
        {
            ProductNumber += number;
            ProductNumber = Math.Min(ProductNumber, Capacity);
        }

        /// <summary>
        /// 清除所有信息;
        /// </summary>
        public void Clear()
        {
            this.Capacity = 0;
            this.ProductNumber = 0;
        }

    }

}
