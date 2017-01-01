using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using KouXiaGu.Navigation;
using KouXiaGu.Collections;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Navigation
{

    /// <summary>
    /// 地图导航路径;
    /// </summary>
    public sealed class Navigator : UnitySington<Navigator>
    {
        Navigator() { }

        static IDictionary<CubicHexCoord, TerrainNode> Map
        {
            get { return TerrainController.ActivatedMap; }
        }

        static readonly AStar<CubicHexCoord, TerrainNode> astar = new AStar<CubicHexCoord, TerrainNode>();

        /// <summary>
        /// 获取到导航路径;
        /// </summary>
        public static NavPath<CubicHexCoord, TerrainNode> FindPath(
            Vector3 starting,
            Vector3 destination,
            IObstructive<CubicHexCoord, TerrainNode> obstruction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取到导航路径;
        /// </summary>
        public static NavPath<CubicHexCoord, TerrainNode> FindPath(
            CubicHexCoord starting, 
            CubicHexCoord destination,
            IObstructive<CubicHexCoord, TerrainNode> obstruction)
        {
            var path = astar.Start(Map, new Obstruction(1, 1), new HexRadiusRange(10, starting), starting, destination);
            return new NavPath<CubicHexCoord, TerrainNode>(path, Map);
        }


    }

}
