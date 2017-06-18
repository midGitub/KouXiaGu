﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{
   
    /// <summary>
    /// 城镇产品信息;
    /// </summary>
    public class TownProductInfo
    {


        /// <summary>
        /// 产品类型;
        /// </summary>
        public ProductType ProductType { get; private set; }

        /// <summary>
        /// 稀缺度;
        /// </summary>
        public int ScarcityDegree { get; set; }
    }
}
