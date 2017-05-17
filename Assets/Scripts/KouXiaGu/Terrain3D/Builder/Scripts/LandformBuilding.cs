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

        public void Destroy()
        {
            Destroy(gameObject);
        }

        ILandformBuilding ILandformBuilding.BuildAt(CubicHexCoord coord, MapNode node, LandformManager landform, IWorldData data)
        {
            BuildingNode buildingNode = node.Building;

            Vector3 position = coord.GetTerrainPixel();
            position.y = landform.Builder.GetHeight(position);

            Quaternion angle = Quaternion.Euler(0, buildingNode.Angle, 0);

            GameObject instance = Instantiate(Prefab, position, angle);
            return instance.GetComponent<ILandformBuilding>();
        }

    }

}
