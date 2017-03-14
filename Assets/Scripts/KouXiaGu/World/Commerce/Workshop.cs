using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 生产线;
    /// </summary>
    public class Workshop
    {

        public Workshop(Product type, int producer)
        {
            if (type == null)
                throw new ArgumentNullException();

            this.Type = type;
            this.Producer = producer;
        }


        /// <summary>
        /// 生产资源类型;
        /// </summary>
        public Product Type { get; private set; }

        /// <summary>
        /// 生产者数目; 小于或等于 最大生产者数目
        /// </summary>
        public int Producer { get; private set; }

        /// <summary>
        /// 产出;
        /// </summary>
        public int Produce
        {
            get { throw new NotImplementedException(); }
        }

    }

}
