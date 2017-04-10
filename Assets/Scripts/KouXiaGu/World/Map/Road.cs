using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using ProtoBuf;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 节点道路信息;
    /// </summary>
    [ProtoContract]
    public struct RoadNode
    {
        /// <summary>
        /// 道路的唯一编号;
        /// </summary>
        [ProtoMember(1)]
        public uint ID;

        /// <summary>
        /// 道路类型;
        /// </summary>
        [ProtoMember(2)]
        public int Type;
    }

    [ProtoContract]
    public class MapRoad
    {
        /// <summary>
        /// 节点不存在道路时放置的标志;
        /// </summary>
        internal const uint EmptyRoadID = 0;

        public MapRoad() : this(0)
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
        public uint GetEffectiveID()
        {
            return EffectiveID++;
        }

    }

    public static class RoadExtensions
    {

        public static bool ExistRoad(this MapNode node)
        {
            return node.Road.ExistRoad();
        }

        public static bool ExistRoad(this RoadNode node)
        {
            return node.ID != MapRoad.EmptyRoadID;
        }


        public static MapNode CreateRoad(this MapNode node, Map map, int roadType)
        {
            node.Road = CreateRoad(node.Road, map.Road, roadType);
            return node;
        }

        public static RoadNode CreateRoad(this RoadNode node, MapRoad road, int roadType)
        {
            if (!node.ExistRoad())
            {
                node.ID = road.GetEffectiveID();
            }
            node.Type = roadType;
            return node;
        }

    }

}
