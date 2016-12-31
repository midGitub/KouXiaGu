using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Navigation.Game
{

    /// <summary>
    /// 地图导航控制;
    /// </summary>
    public sealed class Navigator : UnitySington<Navigator>
    {
        Navigator() { }

        /// <summary>
        /// 导航地图;
        /// </summary>
        public static NavMap Map { get; private set; }

        public static NavObstruction Obstruction { get; private set; }

        /// <summary>
        /// 获取到导航路径;
        /// </summary>
        public static NavPath<CubicHexCoord, NavNode> FindPath(Vector3 starting, Vector3 destination)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取到导航路径;
        /// </summary>
        public static NavPath<CubicHexCoord, NavNode> FindPath(CubicHexCoord starting, CubicHexCoord destination)
        {
            throw new NotImplementedException();
        }


    }

}
