using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.Navigation
{

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
            }
            return false;
        }

        public bool IsWalkable(BuildingNode node)
        {
            int buildingType = node.BuildingType;
            NavigationBuildingInfo buildingInfo;
            if (resource.Building.TryGetValue(buildingType, out buildingInfo))
            {
                return buildingInfo.IsWalkable;
            }
            return false;
        }

        public bool IsWalkable(LandformNode node)
        {
            int landformType = node.LandformType;
            NavigationLandformInfo landformInfo;
            if (resource.Landform.TryGetValue(landformType, out landformInfo))
            {
                return landformInfo.IsWalkable;
            }
            return false;
        }

        public IEnumerable<CubicHexCoord> GetNeighbors(CubicHexCoord position)
        {
            return position.GetNeighbours().Select(item => item.Point);
        }

        public NavigationNode<CubicHexCoord> GetInfo(CubicHexCoord position, CubicHexCoord destination)
        {
            throw new NotImplementedException();
        }
    }
}
