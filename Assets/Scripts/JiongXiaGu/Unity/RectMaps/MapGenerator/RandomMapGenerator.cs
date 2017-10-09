using System;
using System.Collections.Generic;
using JiongXiaGu.Grids;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图随机生成器;
    /// </summary>
    public class RandomMapGenerator : MapGenerator
    {
        protected readonly Random random;

        /// <summary>
        /// 需要生成地图节点的坐标;
        /// </summary>
        IEnumerable<RectCoord> points;

        public RandomMapGenerator(MapDescription description, IEnumerable<RectCoord> points) : base(description)
        {
            if(points == null)
                throw new ArgumentNullException(nameof(points));

            random = new Random();
            this.points = points;
        }

        /// <summary>
        /// 需要生成地图节点的坐标;
        /// </summary>
        public IEnumerable<RectCoord> Points
        {
            get { return points; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException(nameof(value));
                points = value;
            }
        }

        public override void GenerateData(IDictionary<RectCoord, MapNode> map)
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
        protected float Angle()
        {
            return random.Next(0, 360);
        }
    }
}
