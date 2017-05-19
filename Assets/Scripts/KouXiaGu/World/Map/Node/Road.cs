using KouXiaGu.Grids;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 节点道路信息;
    /// </summary>
    [ProtoContract]
    public struct RoadNode : IEquatable<RoadNode>
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
        public int RoadType;

        public bool Equals(RoadNode other)
        {
            return ID == other.ID
                && RoadType == other.RoadType;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RoadNode))
                return false;
            return Equals((RoadNode)obj);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public static bool operator ==(RoadNode a, RoadNode b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(RoadNode a, RoadNode b)
        {
            return !a.Equals(b);
        }
    }

    public static class MapRoadExtensions
    {
        /// <summary>
        /// 节点不存在道路时放置的标志;
        /// </summary>
        public const int EmptyMark = 0;

        /// <summary>
        /// 返回是否存在道路;
        /// </summary>
        public static bool Exist(this RoadNode node)
        {
            return node.ID != EmptyMark;
        }

        /// <summary>
        /// 销毁该点道路信息;
        /// </summary>
        public static RoadNode Destroy(this RoadNode node)
        {
            return default(RoadNode);
        }

        /// <summary>
        /// 更新道路信息;
        /// </summary>
        public static RoadNode Update(this RoadNode node, MapData data, int roadType)
        {
            return Update(node, data.Road, roadType);
        }

        /// <summary>
        /// 更新道路信息;
        /// </summary>
        public static RoadNode Update(this RoadNode node, IdentifierGenerator roadInfo, int roadType)
        {
            if (!node.Exist())
            {
                node.ID = roadInfo.GetNewEffectiveID();
            }
            node.RoadType = roadType;
            return node;
        }


        /// <summary>
        /// 迭代获取到这个点通向周围的路径点,若不存在节点则不进行迭代;
        /// </summary>
        public static IEnumerable<CubicHexCoord[]> GetPeripheralRoutes(this IReadOnlyDictionary<CubicHexCoord, MapNode> map, CubicHexCoord target)
        {
            PeripheralRoute.TryGetPeripheralValue tryGetValue = delegate (CubicHexCoord position, out uint value)
            {
                MapNode node;
                if (map.TryGetValue(position, out node))
                {
                    if (node.Road.Exist())
                    {
                        value = node.Road.ID;
                        return true;
                    }
                }
                value = default(uint);
                return false;
            };
            return PeripheralRoute.GetRoutes(target, tryGetValue);
        }
    }
}
