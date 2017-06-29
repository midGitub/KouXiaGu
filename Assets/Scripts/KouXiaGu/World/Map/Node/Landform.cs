using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    [ProtoContract]
    public struct NodeLandformInfo : IEquatable<NodeLandformInfo>
    {
        /// <summary>
        /// 地形类型;
        /// </summary>
        [ProtoMember(1)]
        public int LandformType { get; internal set; }

        /// <summary>
        /// 地形旋转角度;
        /// </summary>
        [ProtoMember(2)]
        public float Angle { get; internal set; }

        public bool Equals(NodeLandformInfo other)
        {
            return LandformType == other.LandformType && Angle == other.Angle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NodeLandformInfo))
                return false;
            return Equals((NodeLandformInfo)obj);
        }

        public override int GetHashCode()
        {
            return LandformType;
        }

        public static bool operator ==(NodeLandformInfo a, NodeLandformInfo b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(NodeLandformInfo a, NodeLandformInfo b)
        {
            return !a.Equals(b);
        }
    }

    /// <summary>
    /// 节点地貌信息;
    /// </summary>
    [ProtoContract]
    public struct LandformNode : IEquatable<LandformNode>
    {
        /// <summary>
        /// 不存在地形时放置的标志;
        /// </summary>
        public const int EmptyMark = 0;

        NodeLandformInfo info;

        public NodeLandformInfo Info
        {
            get { return info; }
        }

        /// <summary>
        /// 编号,不存在则为0;
        /// </summary>
        [ProtoMember(1)]
        public uint ID { get; internal set; }

        /// <summary>
        /// 地形类型;
        /// </summary>
        [ProtoMember(2)]
        public int LandformType
        {
            get { return info.LandformType; }
            internal set { info.LandformType = value; }
        }

        /// <summary>
        /// 地形旋转角度;
        /// </summary>
        [ProtoMember(3)]
        public float Angle
        {
            get { return info.Angle; }
            internal set { info.Angle = value; }
        }

        /// <summary>
        /// 是否存在地形?
        /// </summary>
        public bool Exist()
        {
            return ID != EmptyMark;
        }

        /// <summary>
        /// 清除地形信息;
        /// </summary>
        public LandformNode Destroy()
        {
            return default(LandformNode);
        }

        /// <summary>
        /// 更新建筑信息;
        /// </summary>
        public LandformNode Update(MapData data, int landformType, float angle)
        {
            return Update(data.Landform, landformType, angle);
        }

        /// <summary>
        /// 更新建筑信息;
        /// </summary>
        public LandformNode Update(IdentifierGenerator landformInfo, int landformType, float angle)
        {
            if (!Exist())
            {
                ID = landformInfo.GetNewEffectiveID();
            }
            LandformType = landformType;
            Angle = angle;
            return this;
        }

        public bool Equals(LandformNode other)
        {
            return ID == other.ID &&
                LandformType == other.LandformType &&
                Angle == other.Angle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LandformNode))
                return false;
            return Equals((LandformNode)obj);
        }

        public override int GetHashCode()
        {
            return LandformType.GetHashCode();
        }

        public static bool operator ==(LandformNode a, LandformNode b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(LandformNode a, LandformNode b)
        {
            return !a.Equals(b);
        }
    }
}
