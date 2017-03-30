using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{

    /// <summary>
    /// 基本信息;
    /// </summary>
    public class BasicInformation
    {

        /// <summary>
        /// 资源\产品;
        /// </summary>
        public ProductManager Product { get; private set; }

        /// <summary>
        /// 建筑物;
        /// </summary>
        public BuildingManager Building { get; private set; }

    }

}
