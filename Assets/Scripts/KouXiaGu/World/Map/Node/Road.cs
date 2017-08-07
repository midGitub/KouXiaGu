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
        public NodeRoadInfo(int type)
        {
            RoadType = type;
        }

        /// <summary>
        /// 道路类型;
        /// </summary>
        [ProtoMember(1)]
        public int RoadType { get; internal set; }

        public bool Exist()
        {
            return RoadType != 0;
        }

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

        public RoadNode Update(MapData data, NodeRoadInfo info)
        {
            return Update(data, info.RoadType);
        }

        /// <summary>
        /// 更新道路信息;
        /// </summary>
        public RoadNode Update(MapData data, int roadType)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            return Update(data.Road, roadType);
        }

        /// <summary>
        /// 更新道路信息;
        /// </summary>
        public RoadNode Update(IdentifierGenerator roadInfo, int roadType)
        {
            if (roadInfo == null)
                throw new ArgumentNullException("roadInfo");

            if (!Exist())
            {
                ID = roadInfo.GetNewEffectiveID();
            }
            RoadType = roadType;
            return this;
        }

        /// <summary>
        /// 返回是否存在道路;
        /// </summary>
        public bool Exist()
        {
            return info.Exist();
        }

        /// <summary>
        /// 销毁该点道路信息;
        /// </summary>
        public RoadNode Destroy()
        {
            ID = default(uint);
            RoadType = default(int);
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
