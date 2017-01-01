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

        #region 同步;

        static readonly AStarPathFinding<CubicHexCoord, TerrainNode> astar = new AStarPathFinding<CubicHexCoord, TerrainNode>();

        static readonly HexRadiusRange hexRange = new HexRadiusRange();

        /// <summary>
        /// 获取到导航路径(同步);
        /// </summary>
        public static NavPath<CubicHexCoord, TerrainNode> FindPath(
            CubicHexCoord starting,
            CubicHexCoord destination,
            IObstructive<CubicHexCoord, TerrainNode> obstruction,
            int radius)
        {
            hexRange.Radius = radius;
            hexRange.Starting = starting;

            var path = astar.Search(Map, obstruction, hexRange, starting, destination);
            return new NavPath<CubicHexCoord, TerrainNode>(path, Map);
        }

        /// <summary>
        /// 获取到导航路径(同步);
        /// </summary>
        public static NavPath<CubicHexCoord, TerrainNode> FindPath(
            CubicHexCoord starting,
            CubicHexCoord destination,
            IObstructive<CubicHexCoord, TerrainNode> obstruction,
            IRange<CubicHexCoord> range)
        {
            var path = astar.Search(Map, obstruction, range, starting, destination);
            return new NavPath<CubicHexCoord, TerrainNode>(path, Map);
        }

        #endregion


        #region 异步;

        /// <summary>
        /// 获取到导航路径(异步);
        /// </summary>
        public static void FindPathAsync(
            CubicHexCoord starting,
            CubicHexCoord destination,
            IObstructive<CubicHexCoord, TerrainNode> obstruction,
            IRange<CubicHexCoord> range)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
