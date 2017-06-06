using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu.Terrain3D.DynamicMeshs
{

    [DisallowMultipleComponent]
    public class WallMeshPrefab : MonoBehaviour, IBuildingPrefab
    {
        public IBuilding BuildAt(IWorld world, CubicHexCoord position, float angele, BuildingInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
