using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 生产组;
    /// </summary>
    public class ProductionGroup
    {

        public ProductionGroup()
        {
            Productions = new List<Production>();
        }

        /// <summary>
        /// 生产内容;
        /// </summary>
        public List<Production> Productions { get; private set; }





    }

}
