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
    /// 寻路方法;
    /// </summary>
    public static class Pathfinding
    {

        static IDictionary<CubicHexCoord, TerrainNode> Map
        {
            get { return OTerrainInitializer.Map; }
        }

        #region 同步;

        static readonly AStarPathFinding<CubicHexCoord, TerrainNode> astar = new AStarPathFinding<CubicHexCoord, TerrainNode>();

        /// <summary>
        /// 获取到导航路径(同步);
        /// </summary>
        public static Path<CubicHexCoord, TerrainNode> FindPath(
            CubicHexCoord starting,
            CubicHexCoord destination,
            IPathFindingCost<CubicHexCoord, TerrainNode> obstruction,
            IRange<CubicHexCoord> searchRange)
        {
            var path = astar.Search(Map, obstruction, searchRange, starting, destination);
            return new Path<CubicHexCoord, TerrainNode>(path, Map);
        }

        #endregion

        #region 异步;

        /// <summary>
        /// 获取到导航路径(异步);
        /// </summary>
        public static void FindPathAsync(
            CubicHexCoord starting,
            CubicHexCoord destination,
            IPathFindingCost<CubicHexCoord, TerrainNode> obstruction,
            IRange<CubicHexCoord> range)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
