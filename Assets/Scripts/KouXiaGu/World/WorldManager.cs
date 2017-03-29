using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World.Commerce;

namespace KouXiaGu.World
{

    public class WorldManager
    {
        public WorldManager(WorldInfo info)
        {
            Info = info;
        }

        /// <summary>
        /// 世界信息;
        /// </summary>
        public WorldInfo Info { get; private set; }

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
