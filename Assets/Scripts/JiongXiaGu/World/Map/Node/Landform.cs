using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.World.Map
{

    [ProtoContract]
    public struct NodeLandformInfo : IEquatable<NodeLandformInfo>
    {
        public NodeLandformInfo(int type, float angle)
        {
            LandformType = type;
            Angle = angle;
        }

        /// <summary>
        /// 地形类型;
        /// </summary>
        [ProtoMember(1)]
        public int LandformType { get; set; }

        /// <summary>
        /// 地形旋转角度;
        /// </summary>
        [ProtoMember(2)]
        public float Angle { get; set; }

        public bool Exist()
        {
            return LandformType != 0;
        }

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

        public LandformNode Update(MapData data, NodeLandformInfo info)
        {
            return Update(data, info.LandformType, info.Angle);
        }

        public LandformNode Update(MapData data, int landformType, float angle)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            return Update(data.Landform, landformType, angle);
        }

        public LandformNode Update(IdentifierGenerator landformInfo, int landformType, float angle)
        {
            if (landformInfo == null)
                throw new ArgumentNullException("landformInfo");

            if (!Exist())
            {
                ID = landformInfo.GetNewEffectiveID();
            }
            LandformType = landformType;
            Angle = angle;
            return this;
        }

        /// <summary>
        /// 是否存在地形?
        /// </summary>
        public bool Exist()
        {
            return info.Exist();
        }

        /// <summary>
        /// 清除地形信息;
        /// </summary>
        public LandformNode Destroy()
        {
            ID = default(uint);
            LandformType = default(int);
            Angle = default(int);
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
