﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    public class Factory
    {

        public Factory()
        {

        }


        /// <summary>
        /// 所有生产线;
        /// </summary>
        public List<ProductionLine> ProductionLines { get; private set; }

        /// <summary>
        /// 成品储存到;
        /// </summary>
        public Warehouse Warehouse { get; set; }


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
            public Workshop Workshop { get; private set; }
            public Warehouse.Room Product { get; private set; }

        }

    }

}
