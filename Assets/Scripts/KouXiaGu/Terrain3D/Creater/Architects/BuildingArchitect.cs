using System;
using System.Collections.Generic;
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
            this.data = data;

            foreach (var coord in overlayes)
            {
                try
                {
                    Build(coord);
                }
                catch (KeyNotFoundException e)
                {
                    Debug.LogWarning(e);
                }
            }

            return buildings;
        }

        void Build(CubicHexCoord coord)
        {
            BuildingNode node;
            if (data.TryGetValue(coord, out node))
            {
                GameObject prefab = GetBuildRes(node.ID).Prefab;
                Vector3 position = coord.GetTerrainPixel();
                float angle = node.Angle;

                var gameObject = GameObject.Instantiate(prefab, position, Quaternion.Euler(0, angle, 0));
                buildings.Add(gameObject);
            }
            return;
        }

        BuildingRes GetBuildRes(int id)
        {
            try
            {
                return BuildingRes.initializedInstances[id];
            }
            catch (KeyNotFoundException ex)  
            {
                throw new KeyNotFoundException("缺少建筑资源 :" + id, ex);
            }
        }

    }

}
