using System;
using System.Collections.Generic;
using System.Linq;
using JiongXiaGu.Unity.RectTerrain.Resources;
using JiongXiaGu.Grids;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 简单地图随机生成器;
    /// </summary>
    public class SimpleMapGenerator
    {
        public SimpleMapGenerator(RectTerrainResources rectTerrainResources)
        {
            if (rectTerrainResources == null)
                throw new ArgumentNullException("rectTerrainResources");

            random = new Random();
            landformTypes = rectTerrainResources.Landform.Values.ToArray(item => item.ID);
        }

        Random random;
        int[] landformTypes;

        public MapData Create(int radius)
        {
            MapData data = new MapData();
            RandomMap(data.Data, radius);
            return data;
        }

        Dictionary<RectCoord, MapNode> RandomMap(Dictionary<RectCoord, MapNode> map, int radius)
        {
            foreach (var point in RectCoord.Spiral_in(RectCoord.Self, radius))
            {
                MapNode node = RandomMapNode();
                map.Add(point, node);
            }
            return map;
        }

        MapNode RandomMapNode()
        {
            MapNode node = new MapNode()
            {
                Landform = new NodeLandformInfo()
                {
                    TypeID = random.Get(landformTypes),
                    Angle = random.Angle(),
                },
            };
            return node;
        }
    }
}
