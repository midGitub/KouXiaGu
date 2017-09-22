using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Grids;
using JiongXiaGu.World;
using JiongXiaGu.World.Map;
using UnityEngine;
using JiongXiaGu.World.Resources;

namespace JiongXiaGu.Terrain3D
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
        BuildingUnit prefab;

        /// <summary>
        /// 创建的预制物体;
        /// </summary>
        public BuildingUnit Prefab
        {
            get { return prefab; }
            set { prefab = value; }
        }

        public BuildingUnit BuildAt(IWorld world, CubicHexCoord position, float angele, BuildingInfo info)
        {
            BuildingUnit instance = Instantiate(prefab, objectParent);
            instance.Build(world, position, angele, info);
            return instance;
        }

        IBuilding IBuildingPrefab.BuildAt(IWorld world, CubicHexCoord position, float angele, BuildingInfo info)
        {
            return BuildAt(world, position, angele, info);
        }
    }
}
