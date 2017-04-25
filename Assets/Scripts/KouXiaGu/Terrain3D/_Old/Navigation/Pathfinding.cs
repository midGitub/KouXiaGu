//using System;
//using System.Collections.Generic;
//using System.Linq;
//using KouXiaGu.Grids;
//using KouXiaGu.Navigation;
//using KouXiaGu.Collections;
//using UnityEngine;
//using KouXiaGu.World.Map;

//namespace KouXiaGu.Terrain3D.Navigation
//{

//    /// <summary>
//    /// 寻路方法;
//    /// </summary>
//    public static class Pathfinding
//    {

//        static IDictionary<CubicHexCoord, MapNode> Map
//        {
//            get { throw new NotImplementedException(); }
//        }

//        #region 同步;

//        static readonly AStarPathFinding<CubicHexCoord, MapNode> astar = new AStarPathFinding<CubicHexCoord, MapNode>();

//        /// <summary>
//        /// 获取到导航路径(同步);
//        /// </summary>
//        public static Path<CubicHexCoord, MapNode> FindPath(
//            CubicHexCoord starting,
//            CubicHexCoord destination,
//            IPathFindingCost<CubicHexCoord, MapNode> obstruction,
//            IRange<CubicHexCoord> searchRange)
//        {
//            var path = astar.Search(Map, obstruction, searchRange, starting, destination);
//            return new Path<CubicHexCoord, MapNode>(path, Map);
//        }

//        #endregion

//        #region 异步;

//        /// <summary>
//        /// 获取到导航路径(异步);
//        /// </summary>
//        public static void FindPathAsync(
//            CubicHexCoord starting,
//            CubicHexCoord destination,
//            IPathFindingCost<CubicHexCoord, MapNode> obstruction,
//            IRange<CubicHexCoord> range)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion

//    }

//}
