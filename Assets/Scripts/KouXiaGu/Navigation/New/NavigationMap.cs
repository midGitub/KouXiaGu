using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;

namespace KouXiaGu.Navigation
{

    /// <summary>
    /// 当节点存在建筑时,依据建筑导航信息,若不存在建筑,则依地形导航信息;
    /// </summary>
    public class NavigationMap : INavigationMap<CubicHexCoord>
    {
        public NavigationMap(IReadOnlyDictionary<CubicHexCoord, MapNode> map, NavigationResource resource)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            if (resource == null)
                throw new ArgumentNullException("resource");

            this.map = map;
            this.resource = resource;
        }

        readonly IReadOnlyDictionary<CubicHexCoord, MapNode> map;
        readonly NavigationResource resource;

        public bool IsWalkable(CubicHexCoord position)
        {
            MapNode node;
            if (map.TryGetValue(position, out node))
            {
                return IsWalkable(node);
            }
            return false;
        }

        public bool IsWalkable(MapNode node)
        {
            BuildingNode buildingNode = node.Building;
            if (buildingNode.Exist())
            {
                return IsWalkable(buildingNode);
            }

            LandformNode landformNode = node.Landform;
            if (landformNode.Exist())
            {
                return IsWalkable(landformNode);
            }
            return false;
        }

        bool IsWalkable(BuildingNode node)
        {
            int buildingType = node.BuildingType;
            NavigationBuildingInfo buildingInfo;
            if (resource.Building.TryGetValue(buildingType, out buildingInfo))
            {
                return buildingInfo.IsWalkable;
            }
            return false;
        }

        bool IsWalkable(LandformNode node)
        {
            int landformType = node.LandformType;
            NavigationLandformInfo landformInfo;
            if (resource.Landform.TryGetValue(landformType, out landformInfo))
            {
                return landformInfo.IsWalkable;
            }
            return false;
        }

        /// <summary>
        /// 获取到该位置的导航信息;
        /// </summary>
        public NavigationNode<CubicHexCoord> GetNavigationNode(CubicHexCoord position, CubicHexCoord destination)
        {
            var node = new NavigationNode<CubicHexCoord>()
            {
                Position = position,
                Destination = destination,
            };
            node.IsWalkable = IsWalkable(position, out node.Cost);
            node.Cost += GetDistanceCoset(position, destination);
            return node;
        }

        const int DistanceCoefficient = 5;
        
        /// <summary>
        /// 获取到距离产生的寻路代价值;
        /// </summary>
        public int GetDistanceCoset(CubicHexCoord position, CubicHexCoord destination)
        {
            return CubicHexCoord.ManhattanDistances(position, destination) * DistanceCoefficient;
        }

        /// <summary>
        /// 获取到该位置是否允许行走 和 行走到的代价值;
        /// </summary>
        public bool IsWalkable(CubicHexCoord position, out int cost)
        {
            MapNode node;
            if (map.TryGetValue(position, out node))
            {
                return IsWalkable(node, out cost);
            }
            cost = 0;
            return false;
        }

        bool IsWalkable(MapNode node, out int cost)
        {
            cost = 0;
            if (node.Building.Exist())
            {
                int buildingCost;
                if (!IsWalkable(node.Building, out buildingCost))
                {
                    return false;
                }
                cost += buildingCost;
            }
            else if (node.Landform.Exist())
            {
                int landformCost;
                if (!IsWalkable(node.Landform, out landformCost))
                {
                    return false;
                }
                cost += landformCost;
            }
            else
            {
                return false;
            }

            if (node.Road.Exist())
            {
                cost += GetCost(node.Road);
            }
            return true;
        }

        bool IsWalkable(BuildingNode node, out int cost)
        {
            if (node.Exist())
            {
                int type = node.BuildingType;
                NavigationBuildingInfo info;
                if (resource.Building.TryGetValue(type, out info))
                {
                    cost = info.Cost;
                    return info.IsWalkable;
                }
            }
            cost = 0;
            return false;
        }

        bool IsWalkable(LandformNode node, out int cost)
        {
            if (node.Exist())
            {
                int landformType = node.LandformType;
                NavigationLandformInfo info;
                if (resource.Landform.TryGetValue(landformType, out info))
                {
                    cost = info.Cost;
                    return info.IsWalkable;
                }
            }
            cost = 0;
            return false;
        }

        int GetCost(RoadNode node)
        {
            if (node.Exist())
            {
                int roadType = node.RoadType;
                NavigationRoadInfo info;
                if (resource.Road.TryGetValue(roadType, out info))
                {
                    return info.Cost;
                }
            }
            return 0;
        }

        public IEnumerable<CubicHexCoord> GetNeighbors(CubicHexCoord position)
        {
            return position.GetNeighbours().Select(item => item.Point);
        }
    }
}
