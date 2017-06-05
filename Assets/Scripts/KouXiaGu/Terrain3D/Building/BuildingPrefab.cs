using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World;
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

        public Building BuildAt(IWorld world, CubicHexCoord position, float angele, BuildingResource info)
        {
            Vector3 pixelPosition = position.GetTerrainPixel();
            Quaternion angle = Quaternion.Euler(0, angele, 0);
            Building instance = Instantiate(prefab, pixelPosition, angle, objectParent);
            instance.Build(world, position);
            return instance;
        }

        IBuilding IBuildingPrefab.BuildAt(IWorld world, CubicHexCoord position, float angele, BuildingResource info)
        {
            return BuildAt(world, position, angele, info);
        }
    }
}
