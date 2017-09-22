using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.RectTerrain;
using JiongXiaGu.RectTerrain.Resources;
using JiongXiaGu.Grids;

namespace JiongXiaGu.World.RectMap
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
            MapData map = new MapData()
            {
                Data = RandomMap(radius),
            };
            return map;
        }

        Dictionary<RectCoord, MapNode> RandomMap(int radius)
        {
            var map = new Dictionary<RectCoord, MapNode>();
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
