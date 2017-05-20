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
    public class LandformBuilding : MonoBehaviour, IBuildingPrefab, IBuilding
    {
        protected LandformBuilding()
        {
        }

        public CubicHexCoord Position { get; private set; }
        public BuildingBuilder Builder { get; private set; }

        Landform landform
        {
            get { return Builder.Landform; }
        }

        IBuilding IBuildingPrefab.BuildAt(CubicHexCoord position, MapNode node, BuildingBuilder builder)
        {
            BuildingNode buildingNode = node.Building;
            Vector3 pixelPosition = position.GetTerrainPixel();
            Quaternion angle = Quaternion.Euler(0, buildingNode.Angle, 0);
            GameObject instance = Instantiate(gameObject, pixelPosition, angle);
            LandformBuilding item = instance.GetComponent<LandformBuilding>();
            item.Position = position;
            item.Builder = builder;
            item.Rebuild();
            return item;
        }

        void IBuilding.Destroy()
        {
            Destroy(gameObject);
        }

        public void Rebuild()
        {
            Vector3 position = transform.position;
            position.y = landform.GetHeight(transform.position);
            transform.position = position;
        }
    }

}
