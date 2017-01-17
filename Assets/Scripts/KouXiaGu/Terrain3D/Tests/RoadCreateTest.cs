using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{


    public class RoadCreateTest : MonoBehaviour
    {

        [SerializeField]
        KeyCode createKey = KeyCode.K;

        [SerializeField]
        RoadMeshCreate creater;

        void Update()
        {
            if (Input.GetKeyDown(createKey))
            {
                CubicHexCoord coord = TerrainTrigger.MouseRayPointOrDefault().GetTerrainCubic();
                creater.RoadInfo = MapDataManager.ActiveData.Road;
                creater.Create(coord);
            }
        }

    }

}
