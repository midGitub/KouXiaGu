using System;
using System.Collections.Generic;
using System.Linq;
using JiongXiaGu.Grids;
using JiongXiaGu.Collections;
using JiongXiaGu.Unity.RectTerrain;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 简单的地图随机生成器;
    /// </summary>
    public class SimpleMapGenerator : RandomMapGenerator
    {
        string[] landformTypes;

        public SimpleMapGenerator(MapDescription description, RectTerrainResource resources, IEnumerable<RectCoord> points) : base(description, points)
        {
            if (resources == null)
                throw new ArgumentNullException(nameof(resources));

            landformTypes = resources.LandformResCreater.Descriptions.Values.ToArray(item => item.ID);
        }

        public override void GenerateData(IDictionary<RectCoord, MapNode> map)
        {
            foreach (var point in Points)
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
