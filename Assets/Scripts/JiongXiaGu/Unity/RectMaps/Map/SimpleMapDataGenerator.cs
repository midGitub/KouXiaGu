using System;
using System.Collections.Generic;
using System.Linq;
using JiongXiaGu.Unity.RectTerrain.Resources;
using JiongXiaGu.Grids;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.RectMaps
{

    public class MapDataGenerator
    {
        protected readonly Random random;

        public MapDataGenerator()
        {
            random = new Random();
        }

        /// <summary>
        /// 生成随机地图节点;
        /// </summary>
        /// <param name="map"></param>
        /// <param name="radius"></param>
        public virtual void Generate(IDictionary<RectCoord, MapNode> map, int radius)
        {
            Generate(map, RectCoord.Spiral_in(RectCoord.Self, radius));
        }

        public virtual void Generate(IDictionary<RectCoord, MapNode> map, IEnumerable<RectCoord> points)
        {
            foreach (var point in points)
            {
                MapNode node = new MapNode()
                {
                    Landform = new NodeLandformInfo()
                    {
                        TypeID = random.Next(),
                        Angle = Angle(),
                    },

                    Building = new NodeBuildingInfo()
                    {
                        TypeID = random.Next(),
                        Angle = Angle(),
                    },

                    Road = new NodeRoadInfo()
                    {
                        TypeID = random.Next(),
                    },
                };
                map.AddOrUpdate(point, node);
            }
        }

        /// <summary>
        /// 返回一个随机的0~360的角度;
        /// </summary>
        public float Angle()
        {
            return random.Next(0, 360);
        }
    }

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
