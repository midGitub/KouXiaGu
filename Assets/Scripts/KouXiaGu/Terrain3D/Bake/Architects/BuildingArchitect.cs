using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [Serializable]
    class BuildingArchitect
    {
        BuildingArchitect()
        {
        }

        BuildingData data;
        List<GameObject> buildings;

        public List<GameObject> Build(BuildingData data, IEnumerable<CubicHexCoord> overlayes)
        {
            buildings = new List<GameObject>();

            foreach (var coord in overlayes)
            {
                Build(coord);
            }

            return buildings;
        }

        void Build(CubicHexCoord coord)
        {
            BuildingNode node;
            if (data.TryGetValue(coord, out node))
            {
                GameObject prefab = GetBuildRes(node.ID).Prefab;
                Vector3 position = GetTerrainPixel(coord);
                float angle = node.Angle;

                var gameObject = GameObject.Instantiate(prefab, position, Quaternion.Euler(0, angle, 0));
                buildings.Add(gameObject);
            }
            return;
        }

        Vector3 GetTerrainPixel(CubicHexCoord coord)
        {
            var pixel = coord.GetTerrainPixel();
            pixel.y = TerrainData.GetHeight(pixel);
            return pixel;
        }

        BuildingRes GetBuildRes(int id)
        {
            try
            {
                var res = BuildingRes.initializedInstances[id];
                return res;
            }
            catch (KeyNotFoundException ex)  
            {
                throw new KeyNotFoundException("缺少建筑资源 :" + id, ex);
            }
        }

    }

}
