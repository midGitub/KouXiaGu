//using System;
//using System.Collections.Generic;
//using System.Linq;
//using KouXiaGu.Grids;
//using KouXiaGu.World.Map;

//namespace KouXiaGu.Navigation
//{

//    /// <summary>
//    /// 影响导航的地图因素;
//    /// </summary>
//    public class LandformFactor : IPathfindFactor
//    {
//        public LandformFactor(IReadOnlyDictionary<CubicHexCoord, MapNode> map, NavigationResource resource)
//        {
//            if (map == null)
//                throw new ArgumentNullException("map");
//            if (resource == null)
//                throw new ArgumentNullException("resource");

//            this.map = map;
//            this.resource = resource;
//        }

//        readonly IReadOnlyDictionary<CubicHexCoord, MapNode> map;
//        readonly NavigationResource resource;

//        /// <summary>
//        /// 获取到该位置是否允许行走;
//        /// </summary>
//        public bool IsWalkable(CubicHexCoord position)
//        {
//            MapNode node;
//            if (map.TryGetValue(position, out node))
//            {
//                return IsWalkable(node);
//            }
//            return false;
//        }

//        public bool IsWalkable(MapNode node)
//        {
//            NavLandformInfo info;
//            if (TryGetLandformInfo(node, out info))
//            {
//                return info.IsWalkable;
//            }
//            return false;
//        }

//        /// <summary>
//        /// 获取到该位置是否允许行走 和 行走到的代价值;
//        /// </summary>
//        public bool IsWalkable(CubicHexCoord position, out int cost)
//        {
//            MapNode node;
//            if (map.TryGetValue(position, out node))
//            {
//                return IsWalkable(node, out cost);
//            }
//            cost = default(int);
//            return false;
//        }

//        public bool IsWalkable(MapNode node, out int cost)
//        {
//            NavLandformInfo info;
//            if (TryGetLandformInfo(node, out info))
//            {
//                cost = info.Cost;
//                return info.IsWalkable;
//            }
//            cost = default(int);
//            return false;
//        }

//        bool TryGetLandformInfo(MapNode node, out NavLandformInfo info)
//        {
//            NavBuildingInfo buildingInfo;
//            if (node.Building.Exist() && resource.Building.TryGetValue(node.Building.BuildingType, out buildingInfo))
//            {
//                if (buildingInfo.IsInfluenceNavigation)
//                {
//                    info = buildingInfo.Navigation;
//                    return true;
//                }
//            }
//            if (node.Landform.Exist() && resource.Landform.TryGetValue(node.Landform.LandformType, out info))
//            {
//                return true;
//            }
//            info = default(NavLandformInfo);
//            return false;
//        }

//        /// <summary>
//        /// 获取到该位置的导航信息;
//        /// </summary>
//        public NavigationNode<CubicHexCoord> GetNavigationNode(CubicHexCoord position, CubicHexCoord destination)
//        {
//            var node = new NavigationNode<CubicHexCoord>()
//            {
//                Position = position,
//                Destination = destination,
//            };
//            node.IsWalkable = IsWalkable(position, out node.Cost);
//            node.Cost += GetDistanceCoset(position, destination);
//            return node;
//        }

//        const int DistanceCoefficient = 5;
        
//        /// <summary>
//        /// 获取到距离产生的寻路代价值;
//        /// </summary>
//        public int GetDistanceCoset(CubicHexCoord position, CubicHexCoord destination)
//        {
//            return CubicHexCoord.ManhattanDistances(position, destination) * DistanceCoefficient;
//        }
//    }
//}
