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

        public CubicHexCoord Position { get; private set; }

        /// <summary>
        /// 道路使用的贴图;
        /// </summary>
        public RoadRes Road { get; private set; }

        /// <summary>
        /// 存在道路的节点方向;
        /// </summary>
        public IEnumerable<float> RoadAngles { get; private set; }

        /// <summary>
        /// 存在道路的方向;
        /// </summary>
        public HexDirections RoadDirections { get; private set; }

        /// <summary>
        /// 存在道路?
        /// </summary>
        public bool ExistRoad
        {
            get { return Road != null; }
        }

        /// <summary>
        /// 初始化该节点道路信息,若不存在道路信息则返回异常;;
        /// </summary>
        public RoadNode(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            var node = map[coord];

            if (node.ExistRoad)
            {
                this.Road = GetRoadRes(node.Road);
                this.RoadDirections = GetRoadDirections(map, coord);
                this.RoadAngles = GetRoadAngles(this.RoadDirections);
            }
            else
            {
                throw new ObjectNotExistedException("该节点不存在道路;");
            }

            this.Position = coord;
        }

        /// <summary>
        /// 获取到道路资源信息;
        /// </summary>
        RoadRes GetRoadRes(int id)
        {
            return RoadRes.initializedInstances[id];
        }

        /// <summary>
        /// 获取到存在道路的方向;
        /// </summary>
        HexDirections GetRoadDirections(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            HexDirections roadDirections = 0;
            TerrainNode node;
            foreach (var dir in CubicHexCoord.Directions)
            {
                CubicHexCoord dirCoord = coord.GetDirection(dir);
                if (map.TryGetValue(dirCoord, out node))
                {
                    if (node.ExistRoad)
                    {
                        roadDirections |= dir;
                    }
                }
            }
            return roadDirections;
        }

        ///// <summary>
        ///// 获取到这个点周围存在道路的方向角度;
        ///// </summary>
        //List<float> GetRoadAngles(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        //{
        //    List<float> angles = new List<float>();
        //    TerrainNode node;
        //    foreach (var dir in CubicHexCoord.Directions)
        //    {
        //        CubicHexCoord dirCoord = coord.GetDirection(dir);
        //        if (map.TryGetValue(dirCoord, out node))
        //        {
        //            if (node.ExistRoad)
        //            {
        //                float angle = GridConvert.GetAngle(dir);
        //                angles.Add(angle);
        //            }
        //        }
        //    }
        //    return angles;
        //}

        /// <summary>
        /// 获取到方向对应的角度;
        /// </summary>
        List<float> GetRoadAngles(HexDirections roadDirections)
        {
            List<float> angles = new List<float>();
            foreach (var dir in CubicHexCoord.GetDirections(roadDirections))
            {
                float angle = GridConvert.GetAngle(dir);
                angles.Add(angle);
            }
            return angles;
        }


    }

}
