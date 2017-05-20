using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 建筑物预制;
    /// </summary>
    [DisallowMultipleComponent]
    public class BuildingPrefab : MonoBehaviour, IBuildingPrefab
    {
        static Transform _objectParent;

        public static Transform objectParent
        {
            get { return _objectParent ?? (_objectParent = new GameObject("LandformBuildings").transform); }
        }

        [SerializeField]
        Building prefab;

        /// <summary>
        /// 创建的预制物体;
        /// </summary>
        public Building Prefab
        {
            get { return prefab; }
            set { prefab = value; }
        }

        public Building BuildAt(CubicHexCoord coord, MapNode node, BuildingBuilder builder)
        {
            BuildingNode buildingNode = node.Building;
            Vector3 position = coord.GetTerrainPixel();
            Quaternion angle = Quaternion.Euler(0, buildingNode.Angle, 0);
            Building instance = Instantiate(prefab, position, angle, objectParent);
            instance.Build(coord, node, builder);
            return instance;
        }

        IBuilding IBuildingPrefab.BuildAt(CubicHexCoord coord, MapNode node, BuildingBuilder builder)
        {
            return BuildAt(coord, node, builder);
        }
    }
}
