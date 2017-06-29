using KouXiaGu.Grids;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    [ProtoContract]
    public struct NodeRoadInfo : IEquatable<NodeRoadInfo>
    {
        /// <summary>
        /// 道路类型;
        /// </summary>
        [ProtoMember(1)]
        public int RoadType { get; internal set; }

        public bool Equals(NodeRoadInfo other)
        {
            return RoadType == other.RoadType;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NodeRoadInfo))
                return false;
            return Equals((NodeRoadInfo)obj);
        }

        public override int GetHashCode()
        {
            return RoadType;
        }

        public static bool operator ==(NodeRoadInfo a, NodeRoadInfo b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(NodeRoadInfo a, NodeRoadInfo b)
        {
            return !a.Equals(b);
        }
    }

    /// <summary>
    /// 节点道路信息;
    /// </summary>
    [ProtoContract]
    public struct RoadNode : IEquatable<RoadNode>
    {
        /// <summary>
        /// 节点不存在道路时放置的标志;
        /// </summary>
        public const int EmptyMark = 0;

        NodeRoadInfo info;

        public NodeRoadInfo Info
        {
            get { return info; }
            internal set { info = value; }
        }

        /// <summary>
        /// 道路的唯一编号;
        /// </summary>
        [ProtoMember(1)]
        public uint ID { get; internal set; }

        /// <summary>
        /// 道路类型;
        /// </summary>
        [ProtoMember(2)]
        public int RoadType
        {
            get { return info.RoadType; }
            internal set { info.RoadType = value; }
        }

        /// <summary>
        /// 返回是否存在道路;
        /// </summary>
        public bool Exist()
        {
            return ID != EmptyMark;
        }

        /// <summary>
        /// 销毁该点道路信息;
        /// </summary>
        public RoadNode Destroy()
        {
            return default(RoadNode);
        }

        /// <summary>
        /// 更新道路信息;
        /// </summary>
        public RoadNode Update(MapData data, int roadType)
        {
            return Update(data.Road, roadType);
        }

        /// <summary>
        /// 更新道路信息;
        /// </summary>
        public RoadNode Update(IdentifierGenerator roadInfo, int roadType)
        {
            if (!Exist())
            {
                ID = roadInfo.GetNewEffectiveID();
            }
            RoadType = roadType;
            return this;
        }

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
}
