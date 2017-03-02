using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{


    public class Factory
    {

        /// <summary>
        /// 生产内容;
        /// </summary>
        public List<ProductionLine> ProductionLines { get; private set; }

        /// <summary>
        /// 成品储存到;
        /// </summary>
        public Product Storage { get; private set; }


        /// <summary>
        /// 开始生产该物品;
        /// </summary>
        /// <param name="type">物品类型;</param>
        /// <param name="productType">生产者数目;</param>
        public void StartProduction(Resource type, byte producerCount)
        {

        }


        public class ProductionLine : IEquatable<ProductionLine>
        {
            public ProductionLine()
            {
            }

            /// <summary>
            /// 生产资源类型;
            /// </summary>
            public Resource Type { get; set; }

            /// <summary>
            /// 产品存储到的位置;
            /// </summary>
            public ProductRoom ProductRoom { get; set; }

            /// <summary>
            /// 生产者数目;
            /// </summary>
            public byte ProducerCount { get; set; }


            /// <summary>
            /// 生产当前资源,并存储到;
            /// </summary>
            public void Producing()
            {

            }


            public bool Equals(ProductionLine other)
            {
                return other.Type == this.Type;
            }

            public override bool Equals(object obj)
            {
                var item = obj as ProductionLine;

                if (item == null)
                    return false;

                return Equals(item);
            }

            public override int GetHashCode()
            {
                return Type.GetHashCode();
            }

        }

    }

}
