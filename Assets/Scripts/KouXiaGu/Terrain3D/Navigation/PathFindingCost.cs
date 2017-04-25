//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using KouXiaGu.Grids;
//using KouXiaGu.Navigation;
//using KouXiaGu.World.Map;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D.Navigation
//{

//    /// <summary>
//    /// 寻路权重;
//    /// </summary>
//    [Serializable]
//    public class PathFindingCost : IPathFindingCost<CubicHexCoord, MapNode>
//    {

//        public PathFindingCost(float distancesFactor, float landformFactor)
//        {
//            this.distancesFactor = distancesFactor;
//            this.landformFactor = landformFactor;
//        }

//        /// <summary>
//        /// 距离的权重;
//        /// </summary>
//        [SerializeField, Range(1, 10)]
//        float distancesFactor;

//        /// <summary>
//        /// 地貌权重;
//        /// </summary>
//        [SerializeField, Range(1,10)]
//        float landformFactor;

//        /// <summary>
//        /// 是否可行走;
//        /// </summary>
//        public bool CanWalk(MapNode item)
//        {
//            int landform = item.Landform.LandformID;
//            NavigationDescr descr = NavigationRes.GetNavigationDescr(landform);
//            return descr.Walkable;
//        }

//        /// <summary>
//        /// 获取到代价值;
//        /// </summary>
//        public float GetCost(CubicHexCoord targetPoint, MapNode targetNode, CubicHexCoord destination)
//        {
//            float cost = ManhattanDistances(targetPoint, destination) * distancesFactor;
//            cost += GetCost(targetNode) * landformFactor;
//            return cost;
//        }

//        /// <summary>
//        /// 曼哈顿距离;
//        /// </summary>
//        int ManhattanDistances(CubicHexCoord a, CubicHexCoord b)
//        {
//            return CubicHexCoord.ManhattanDistances(a, b);
//        }

//        /// <summary>
//        /// 获取到这个节点的代价值;
//        /// </summary>
//        float GetCost(MapNode node)
//        {
//            int landform = node.Landform.LandformID;
//            return NavigationRes.TerrainInfos[landform].NavigationCost;
//        }

//    }

//}
