using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World.Commerce
{


    public class ProductType
    {
        public ProductType()
        {
        }

        /// <summary>
        /// 类型ID;
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public Texture Icon { get; set; }

        /// <summary>
        /// 价值;
        /// </summary>
        public int Worth { get; set; }
    }
}
