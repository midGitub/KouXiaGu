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

        public CubicHexCoord Position { get; private set; }
        public Landform Landform { get; private set; }
        public IWorldData WorldData { get; private set; }

        protected GameObject Prefab
        {
            get { return gameObject; }
        }

        void Update()
        {
            Vector3 position = transform.position;
            position.y = Landform.GetHeight(transform.position);
            transform.position = position;
        }

        ILandformBuilding ILandformBuilding.BuildAt(CubicHexCoord position, MapNode node, Landform landform, IWorldData data)
        {
            Position = position;
            Landform = landform;
            WorldData = data;

            BuildingNode buildingNode = node.Building;
            Vector3 pixelPosition = position.GetTerrainPixel();
            Quaternion angle = Quaternion.Euler(0, buildingNode.Angle, 0);
            GameObject instance = Instantiate(Prefab, pixelPosition, angle);
            LandformBuilding item = instance.GetComponent<LandformBuilding>();
            return item;
        }

        void ILandformBuilding.Destroy()
        {
            Destroy(gameObject);
        }
    }

}
