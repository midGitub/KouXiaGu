using System;
using System.Collections.Generic;
using KouXiaGu.Grids;
using System.Xml.Serialization;
using System.Linq;
using ProtoBuf;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路节点信息;
    /// </summary>
    [ProtoContract]
    public struct RoadInfo
    {

        /// <summary>
        /// 道路的唯一编号;
        /// </summary>
        [ProtoMember(1)]
        public uint ID;

    }


    /// <summary>
    /// 与地图匹配的保存文件;
    /// </summary>
    [XmlType("RoadInfo")]
    public struct RoadDescription
    {
        /// <summary>
        /// 记录到的ID;
        /// </summary>
        [XmlElement("EffectiveID")]
        public uint EffectiveID;
    }


    /// <summary>
    /// 道路编辑类;
    /// </summary>
    public static class RoadEdit
    {

        /// <summary>
        /// 节点不存在道路时放置的标志;
        /// </summary>
        const uint EMPTY_ROAD_MARK = 0;

        /// <summary>
        /// 起始的ID;
        /// </summary>
        const uint INITATING_EFFECTIVE_ID = 5;


        /// <summary>
        /// 当前有效的ID;
        /// </summary>
        static uint effectiveID;

        /// <summary>
        /// 进行编辑的地图;
        /// </summary>
        public static IDictionary<CubicHexCoord, TerrainNode> Map { get; set; }


        public static void Initialize(IDictionary<CubicHexCoord, TerrainNode> map)
        {
            Map = map;
            effectiveID = INITATING_EFFECTIVE_ID;
        }

        public static void Initialize(IDictionary<CubicHexCoord, TerrainNode> map, RoadDescription roadDescr)
        {
            Map = map;
            effectiveID = roadDescr.EffectiveID;
        }


        /// <summary>
        /// 向这个坐标添加道路标记;
        /// </summary>
        public static void CreateRoad(CubicHexCoord coord)
        {
            TerrainNode node = Map[coord];

            if (!IsHaveRoad(node))
            {
                node.RoadInfo.ID = GetNewID();
                Map[coord] = node;
            }
        }

        /// <summary>
        /// 获取到一个新的ID;
        /// </summary>
        static uint GetNewID()
        {
            return effectiveID++;
        }

        /// <summary>
        /// 清除这个坐标的道路标记;
        /// </summary>
        public static void DestroyRoad(CubicHexCoord coord)
        {
            TerrainNode node = Map[coord];

            if (IsHaveRoad(node))
            {
                node.RoadInfo.ID = EMPTY_ROAD_MARK;
                Map[coord] = node;
            }
        }


        /// <summary>
        /// 是否存在道路;
        /// </summary>
        public static bool IsHaveRoad(TerrainNode node)
        {
            return IsHaveRoad(node.RoadInfo);
        }

        /// <summary>
        /// 是否存在道路;
        /// </summary>
        public static bool IsHaveRoad(RoadInfo road)
        {
            return road.ID != 0;
        }

        public static void Clear()
        {
            Map = null;
        }


        /// <summary>
        /// 获取到这个点通向周围的路径;
        /// </summary>
        public static IEnumerable<CubicHexCoord[]> FindPaths(CubicHexCoord target)
        {
            RoadInfo targetRoadInfo = Map[target].RoadInfo;

            if(IsHaveRoad(targetRoadInfo))
            {
                foreach (var neighbour in Map.GetNeighbours<CubicHexCoord, HexDirections, TerrainNode>(target))
                {
                    RoadInfo neighbourRoadInfo = neighbour.Item.RoadInfo;

                    if (IsHaveRoad(neighbourRoadInfo) && neighbourRoadInfo.ID > targetRoadInfo.ID)
                    {
                        CubicHexCoord[] path = new CubicHexCoord[4];

                        path[0] = MinNeighbour(target, neighbour.Point);
                        path[1] = target;
                        path[2] = neighbour.Point;
                        path[4] = MinNeighbour(neighbour.Point, target);

                        yield return path;
                    }
                }
            }
        }

        /// <summary>
        /// 获取到ID值最小的邻居节点,若无法找到则返回 target;
        /// </summary>
        static CubicHexCoord MinNeighbour(
            CubicHexCoord target,
            CubicHexCoord eliminate)
        {
            bool isFind = false;
            uint minID = uint.MaxValue;
            CubicHexCoord min = default(CubicHexCoord);

            foreach (var neighbour in Map.GetNeighbours<CubicHexCoord, HexDirections, TerrainNode>(target))
            {
                RoadInfo neighbourRoadInfo = neighbour.Item.RoadInfo;

                if (neighbour.Point != eliminate &&
                    IsHaveRoad(neighbourRoadInfo) &&
                    neighbourRoadInfo.ID < minID)
                {
                    isFind = true;
                    minID = neighbourRoadInfo.ID;
                    min = neighbour.Point;
                }
            }

            if (isFind)
                return min;
            else
                return target;
        }



    }

}
