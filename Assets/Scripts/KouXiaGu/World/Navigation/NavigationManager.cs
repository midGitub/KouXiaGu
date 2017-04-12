using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;

namespace KouXiaGu.World.Navigation
{

    /// <summary>
    /// 路径导航管理;
    /// </summary>
    public class NavigationManager
    {

        public NavigationManager(WorldManager world)
        {
            World = world;
        }

        public WorldManager World { get; private set; }

        public IDictionary<CubicHexCoord, MapNode> Map
        {
            get { return World.Map.Map.Map.Data; }
        }



    }

}
