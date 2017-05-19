using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using ProtoBuf;

namespace KouXiaGu.World.Map
{

    [Obsolete]
    [ProtoContract]
    public class MapRoad
    {
        /// <summary>
        /// 节点不存在道路时放置的标志;
        /// </summary>
        internal const uint EmptyRoadID = 0;

        /// <summary>
        /// 起始的有效ID;
        /// </summary>
        internal const uint InitialEmptyRoadID = uint.MinValue;

        public MapRoad() : this(InitialEmptyRoadID)
        {
        }

        public MapRoad(uint effectiveID)
        {
            EffectiveID = effectiveID;
        }

        /// <summary>
        /// 当前有效的ID;
        /// </summary>
        [ProtoMember(1)]
        public uint EffectiveID { get; private set; }

        /// <summary>
        /// 获取到一个唯一的有效ID;
        /// </summary>
        public uint GetNewEffectiveID()
        {
            return EffectiveID++;
        }

        /// <summary>
        /// 重置记录信息;
        /// </summary>
        internal void Reset()
        {
            EffectiveID = InitialEmptyRoadID;
        }
    }

    //[Obsolete]
    //public static class OMapRoadExtensions
    //{

    //    /// <summary>
    //    /// 返回是否存在道路;
    //    /// </summary>
    //    public static bool ExistRoad(this MapNode node)
    //    {
    //        return node.Road.ExistRoad();
    //    }

    //    /// <summary>
    //    /// 返回是否存在道路;
    //    /// </summary>
    //    public static bool ExistRoad(this RoadNode node)
    //    {
    //        return node.ID != MapRoad.EmptyRoadID;
    //    }


    //    /// <summary>
    //    /// 创建道路到改点;
    //    /// </summary>
    //    public static MapNode CreateRoad(this MapNode node, PredefinedMap map, int roadType)
    //    {
    //        node.Road = CreateRoad(node.Road, map.Road, roadType);
    //        return node;
    //    }

    //    /// <summary>
    //    /// 创建道路到改点;
    //    /// </summary>
    //    public static RoadNode CreateRoad(this RoadNode node, MapRoad road, int roadType)
    //    {
    //        if (!node.ExistRoad())
    //        {
    //            node.ID = road.GetNewEffectiveID();
    //        }
    //        node.RoadType = roadType;
    //        return node;
    //    }


    //    /// <summary>
    //    /// 销毁该点道路信息;
    //    /// </summary>
    //    public static MapNode DestroyRoad(this MapNode node)
    //    {
    //        node.Road = DestroyRoad(node.Road);
    //        return node;
    //    }

    //    /// <summary>
    //    /// 销毁该点道路信息;
    //    /// </summary>
    //    public static RoadNode DestroyRoad(this RoadNode node)
    //    {
    //        node.ID = MapRoad.EmptyRoadID;
    //        return node;
    //    }


    //    /// <summary>
    //    /// 迭代获取到这个点通向周围的路径点,若不存在节点则不返回;
    //    /// </summary>
    //    public static IEnumerable<CubicHexCoord[]> FindPaths(
    //        this IDictionary<CubicHexCoord, MapNode> map
    //        ,CubicHexCoord target)
    //    {
    //        MapNode node;
    //        if (map.TryGetValue(target, out node))
    //        {
    //            RoadNode targetRoadInfo = node.Road;

    //            if (targetRoadInfo.ExistRoad())
    //            {
    //                foreach (var neighbour in map.GetNeighbours<CubicHexCoord, HexDirections, MapNode>(target))
    //                {
    //                    RoadNode neighbourRoadInfo = neighbour.Item.Road;

    //                    if (neighbourRoadInfo.ExistRoad() && neighbourRoadInfo.ID > targetRoadInfo.ID)
    //                    {
    //                        CubicHexCoord[] path = new CubicHexCoord[4];

    //                        path[0] = RoadMinNeighbour(map, target, neighbour.Point);
    //                        path[1] = target;
    //                        path[2] = neighbour.Point;
    //                        path[3] = RoadMinNeighbour(map, neighbour.Point, target);

    //                        yield return path;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 获取到ID值最小的邻居节点,若无法找到则返回 target;
    //    /// </summary>
    //    /// <param name="map">数据;</param>
    //    /// <param name="target">目标点;</param>
    //    /// <param name="eliminate">排除的点;</param>
    //    /// <returns></returns>
    //    internal static CubicHexCoord RoadMinNeighbour(
    //        this IDictionary<CubicHexCoord, MapNode> map
    //        ,CubicHexCoord target
    //        ,CubicHexCoord eliminate)
    //    {
    //        bool isFind = false;
    //        uint minID = uint.MaxValue;
    //        CubicHexCoord min = default(CubicHexCoord);

    //        foreach (var neighbour in map.GetNeighbours<CubicHexCoord, HexDirections, MapNode>(target))
    //        {
    //            RoadNode neighbourRoadInfo = neighbour.Item.Road;

    //            if (neighbour.Point != eliminate &&
    //                neighbourRoadInfo.ExistRoad() &&
    //                neighbourRoadInfo.ID < minID)
    //            {
    //                isFind = true;
    //                minID = neighbourRoadInfo.ID;
    //                min = neighbour.Point;
    //            }
    //        }

    //        if (isFind)
    //            return min;
    //        else
    //            return target;
    //    }

    //}

}
