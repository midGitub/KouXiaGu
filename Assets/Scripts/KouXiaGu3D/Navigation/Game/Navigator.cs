using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Navigation.Game
{

    /// <summary>
    /// 地图导航控制;
    /// </summary>
    public sealed class Navigator : UnitySington<Navigator>
    {
        Navigator() { }

        IDictionary<CubicHexCoord, NavNode> Map;

        /// <summary>
        /// 获取到导航路径;
        /// </summary>
        public static NavPath<CubicHexCoord, NavNode> FindPath(CubicHexCoord starting, CubicHexCoord destination)
        {
            AStar<CubicHexCoord, NavNode> pathfinding = new AStar<CubicHexCoord, NavNode>();
            var path = pathfinding.Start(starting, destination);

            throw new NotImplementedException();
        }


    }

}
