using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    public class ProductFactory
    {

        public ProductFactory()
        {
            
        }

        /// <summary>
        /// 所有生产线;
        /// </summary>
        public List<ProductionLine> ProductionLines { get; private set; }

        /// <summary>
        /// 所归属的城镇;
        /// </summary>
        public Town Attribution { get; set; }

        /// <summary>
        /// 成品储存到仓库;
        /// </summary>
        public ProductHouse Warehouse
        {
            get { return Attribution.Warehouse; }
        }

        /// <summary>
        /// 可以获取到的人力资源;
        /// </summary>
        DynamicResource humanResource
        {
            get { return Attribution.HumanResource; }
        }


        /// <summary>
        /// 开始生产该物品;
        /// </summary>
        /// <param name="type">物品类型;</param>
        /// <param name="productType">生产者数目;</param>
        public void Add(Product type, byte producerCount)
        {

        }

        /// <summary>
        /// 生产当前资源,并存储到;
        /// </summary>
        public void Producing()
        {
            foreach (var productionLine in ProductionLines)
            {
                if (!productionLine.Enabled)
                    continue;
            }
        }


        public class ProductionLine
        {

            public bool Enabled { get; private set; }

            /// <summary>
            /// 产品类型;
            /// </summary>
            public Product ProductType { get; private set; }

            /// <summary>
            /// 库房;
            /// </summary>
            public ProductRoom ProductRoom { get; private set; }


            public void Producing()
            {

            }

        }

    }

}
