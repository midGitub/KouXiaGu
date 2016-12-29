using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{


    /// <summary>
    /// 道路节点;分别检测点周围是否存在道路,若存在道路则标记;
    /// </summary>
    public class RoadNode
    {

        /// <summary>
        /// 道路使用的贴图;
        /// </summary>
        public RoadRes Road { get; private set; }

        /// <summary>
        /// 存在道路的节点方向;
        /// </summary>
        public IEnumerable<float> Angles { get; private set; }

        public RoadNode(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            this.Road = GetRoadRes(map, coord);
            this.Angles = GetRoadAngles(map, coord);
        }

        /// <summary>
        /// 获取到这个点的道路资源;
        /// </summary>
        RoadRes GetRoadRes(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            var node = map[coord];
            int roadId = node.Road;

            if (node.ExistRoad)
            {
                return GetRoadRes(roadId);
            }
            else
            {
                throw new KeyNotFoundException("该节点不存在道路;");
            }
        }

        /// <summary>
        /// 获取到道路资源信息;
        /// </summary>
        RoadRes GetRoadRes(int id)
        {
            return RoadRes.initializedInstances[id];
        }

        /// <summary>
        /// 获取到这个点周围存在道路的方向角度;
        /// </summary>
        List<float> GetRoadAngles(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            List<float> angles = new List<float>();
            TerrainNode node;
            foreach (var dir in CubicHexCoord.Directions)
            {
                CubicHexCoord dirCoord = coord.GetDirection(dir);
                if (map.TryGetValue(dirCoord, out node))
                {
                    if (node.ExistRoad)
                    {
                        float angle = GridConvert.GetAngle(dir);
                        angles.Add(angle);
                    }
                }
            }
            return angles;
        }


        ///// <summary>
        ///// 获取到这个地图节点应该放置的道路贴图;
        ///// </summary>
        ///// <param name="map">使用的地图</param>
        ///// <param name="coord">所在的位置;</param>
        ///// <returns></returns>
        //public static RoadNode GetRoadTexs(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        //{
        //    TerrainNode node;
        //    if (map.TryGetValue(coord, out node))
        //    {
        //        List<RoadNode> list = new List<RoadNode>();

        //        foreach (var dir in CubicHexCoord.Directions)
        //        {
        //            RoadNode road;
        //            if (TryGetRoadNode(map, coord, dir, out road))
        //            {
        //                list.Add(road);
        //            }
        //        }

        //        return list;
        //    }
        //    else
        //    {
        //        throw new KeyNotFoundException();
        //    }
        //}

        ///// <summary>
        ///// 确认这个方向是否存在道路,若存在则返回标记;
        ///// </summary>
        //static bool TryGetRoadNode(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord, HexDirections direction, out RoadNode road)
        //{
        //    TerrainNode node;
        //    if (map.TryGetValue(coord.GetDirection(direction), out node))
        //    {
        //        if (node.Road != 0)
        //        {
        //            road = new RoadNode()
        //            {
        //                Road = GetRoadRes(node.Road),
        //                RotationY = GridConvert.GetAngle(direction),
        //            };
        //            return true;
        //        }
        //    }
        //    road = default(RoadNode);
        //    return false;
        //}

    }

}
