using System;
using System.Collections.Generic;
using System.Linq;
using JiongXiaGu.Unity.RectTerrain.Resources;
using JiongXiaGu.Grids;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 简单的地图随机生成器;
    /// </summary>
    public class SimpleMapDataGenerator : MapDataGenerator
    {
        int[] landformTypes;

        public SimpleMapDataGenerator(RectTerrainResources rectTerrainResources) : base()
        {
            if (rectTerrainResources == null)
                throw new ArgumentNullException("rectTerrainResources");

            landformTypes = rectTerrainResources.Landform.Values.ToArray(item => item.ID);
        }

        public override void Generate(IDictionary<RectCoord, MapNode> map, IEnumerable<RectCoord> points) 
        {
            foreach (var point in points)
            {
                MapNode node = new MapNode()
                {
                    Landform = new NodeLandformInfo()
                    {
                        TypeID = random.Get(landformTypes),
                        Angle = Angle(),
                    },
                };
                map.Add(point, node);
            }
        }
    }
}
