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

    }

}
