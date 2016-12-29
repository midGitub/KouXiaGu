using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{


    public struct RoadNode
    {

        /// <summary>
        /// 道路使用的贴图;
        /// </summary>
        public RoadRes Road { get; private set; }

        /// <summary>
        /// 贴图旋转角度;
        /// </summary>
        public float RotationY { get; private set; }


        /// <summary>
        /// 获取到这个地图节点应该放置的道路贴图;
        /// </summary>
        /// <param name="map">使用的地图</param>
        /// <param name="coord">所在的位置;</param>
        /// <returns></returns>
        public static List<RoadNode> GetRoadTexs(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            TerrainNode node;
            if (map.TryGetValue(coord, out node))
            {
                List<RoadNode> list = new List<RoadNode>();

                foreach (var dir in CubicHexCoord.Directions)
                {
                    RoadNode road;
                    if (TryGetRoadNode(node, dir, out road))
                    {
                        list.Add(road);
                    }
                }

                return list;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// 尝试获取到这个节点的道路节点信息;
        /// </summary>
        static bool TryGetRoadNode(TerrainNode node, HexDirections direction, out RoadNode road)
        {
            if (node.Road != 0)
            {
                road = new RoadNode()
                {
                    Road = GetRoadRes(node.Road),
                    RotationY = GridConvert.GetAngle(direction),
                };
                return true;
            }
            road = default(RoadNode);
            return false;
        }

        /// <summary>
        /// 获取到道路资源信息;
        /// </summary>
        static RoadRes GetRoadRes(int id)
        {
            return RoadRes.initializedInstances[id];
        }

    }

}
