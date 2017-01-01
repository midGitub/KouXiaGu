using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using KouXiaGu.Navigation;
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
            throw new NotImplementedException();
        }


    }

}
