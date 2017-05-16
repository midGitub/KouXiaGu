using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World;
using UnityEngine;
using KouXiaGu.World.Map;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent]
    public class LandformBuilding : MonoBehaviour, ILandformBuilding
    {
        protected LandformBuilding()
        {
        }

        protected GameObject Prefab
        {
            get { return gameObject; }
        }

        GameObject ILandformBuilding.Build(CubicHexCoord coord, LandformBuilder landform, IWorldData data)
        {
            MapNode node = data.Map.Data[coord];
            BuildingNode buildingNode = node.Building;

            Vector3 position = coord.GetTerrainPixel();
            position.y = landform.GetHeight(position);

            Quaternion angle = Quaternion.Euler(0, buildingNode.Angle, 0);

            GameObject instance = Instantiate(Prefab, position, angle);
            return instance;
        }

    }

}
