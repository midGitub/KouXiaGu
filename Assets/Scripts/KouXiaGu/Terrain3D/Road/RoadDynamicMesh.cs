using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Terrain3D.Wall;
using KouXiaGu.Grids;
using KouXiaGu.World;
using KouXiaGu.World.Map;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent]
    public class RoadDynamicMesh : MonoBehaviour, ILandformBuilding
    {
        RoadDynamicMesh()
        {
        }

        [SerializeField]
        DynamicMeshScript prefab;
        CubicHexCoord position;
        IWorldData worldData;

        public ILandformBuilding BuildAt(CubicHexCoord target, MapNode node, Landform landform, IWorldData worldData)
        {
            Vector3 pixelPosition = target.GetTerrainPixel(0.8f);
            Quaternion angle = Quaternion.identity;
            GameObject instance = Instantiate(gameObject, pixelPosition, angle);
            RoadDynamicMesh parent = instance.GetComponent<RoadDynamicMesh>();
            parent.position = target;
            parent.worldData = worldData;
            Rebuild();
            return parent;
        }

        public void Rebuild()
        {
            foreach (Transform child in transform.OfType<Transform>().ToArray())
            {
                Destroy(child.gameObject);
            }

            IEnumerable<CubicHexCoord[]> routes = GetPeripheralRoutes(position, worldData.MapData.ReadOnlyMap);
            foreach (var route in routes)
            {
                CatmullRomSpline spline = CreateSpline(position, route);
                DynamicMeshScript child = Instantiate(prefab, transform);
                child.Transformation(spline);
            }
        }

        public CatmullRomSpline CreateSpline(CubicHexCoord target, CubicHexCoord[] route)
        {
            Vector3[] points = new Vector3[route.Length];
            for (int i = 0; i < route.Length; i++)
            {
                CubicHexCoord localPosition = route[i] - target;
                points[i] = localPosition.GetTerrainPixel();
            }
            CatmullRomSpline spline = new CatmullRomSpline(points[0], points[1], points[2], points[3]);
            return spline;
        }

        /// <summary>
        /// 迭代获取到这个点通向周围的路径点,若不存在节点则不进行迭代;
        /// </summary>
        public IEnumerable<CubicHexCoord[]> GetPeripheralRoutes(CubicHexCoord target, IReadOnlyDictionary<CubicHexCoord, MapNode> map)
        {
            PeripheralRoute.TryGetPeripheralValue tryGetValue = delegate (CubicHexCoord position, out uint value)
            {
                MapNode node;
                if (map.TryGetValue(position, out node))
                {
                    if (node.Road.Exist())
                    {
                        value = node.Road.ID;
                        return true;
                    }
                }
                value = default(uint);
                return false;
            };
            return PeripheralRoute.GetRoadRoutes(target, tryGetValue);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }

}
