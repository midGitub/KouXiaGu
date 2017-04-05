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
    public class RoadInfo
    {
        /// <summary>
        /// 节点不存在道路时放置的标志;
        /// </summary>
        internal const uint EmptyRoadMark = 0;

        public RoadInfo() : this(0)
        {
        }

        public RoadInfo(uint effectiveID)
        {
            EffectiveID = effectiveID;
        }

        /// <summary>
        /// 当前有效的ID;
        /// </summary>
        [ProtoMember(1)]
        public uint EffectiveID { get; internal set; }

    }

    public static class RoadExtensions
    {

        /// <summary>
        /// 该节点是否存在道路;
        /// </summary>
        public static bool ExistRoad(this RoadNode node)
        {
            return node.ID == RoadInfo.EmptyRoadMark;
        }

        /// <summary>
        /// 设置道路信息到此位置;
        /// </summary>
        public static bool SetRoadAt(this Map map, CubicHexCoord pos, int roadType)
        {
            MapNode node;
            if (map.Data.TryGetValue(pos, out node))
            {
                if (!node.Road.ExistRoad())
                {
                    map.Road.EffectiveID++;
                    node.Road.ID = map.Road.EffectiveID;
                }
                node.Road.Type = roadType;
                map.Data[pos] = node;
                return true;
            }
            return false;
        }

    }


}
