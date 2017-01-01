using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Navigation
{

    /// <summary>
    /// 地图导航路径控制;
    /// </summary>
    public sealed class Navigator : UnitySington<Navigator>
    {
        Navigator() { }

        /// <summary>
        /// 导航地图;
        /// </summary>
        public static NavMap Map { get; private set; }

        /// <summary>
        /// 获取到导航路径;
        /// </summary>
        public static NavPath<CubicHexCoord, TerrainNode> FindPath(
            Vector3 starting,
            Vector3 destination,
            NavObstruction obstruction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取到导航路径;
        /// </summary>
        public static NavPath<CubicHexCoord, TerrainNode> FindPath(
            CubicHexCoord starting, 
            CubicHexCoord destination,
            NavObstruction obstruction)
        {
            throw new NotImplementedException();
        }


    }

}
