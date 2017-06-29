using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 建筑节点信息;
    /// </summary>
    [ProtoContract]
    public struct NodeBuildingInfo : IEquatable<NodeBuildingInfo>
    {
        /// <summary>
        /// 建筑类型;
        /// </summary>
        [ProtoMember(1)]
        public int BuildingType { get; set; }

        /// <summary>
        /// 建筑旋转角度;
        /// </summary>
        [ProtoMember(2)]
        public float Angle { get; set; }

        public bool Equals(NodeBuildingInfo other)
        {
            return BuildingType == other.BuildingType && Angle == other.Angle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NodeBuildingInfo))
                return false;
            return Equals((NodeBuildingInfo)obj);
        }

        public override int GetHashCode()
        {
            return BuildingType.GetHashCode();
        }

        public static bool operator ==(NodeBuildingInfo a, NodeBuildingInfo b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(NodeBuildingInfo a, NodeBuildingInfo b)
        {
            return !a.Equals(b);
        }
    }

    /// <summary>
    /// 建筑节点;
    /// </summary>
    [ProtoContract]
    public struct BuildingNode : IEquatable<BuildingNode>
    {
        /// <summary>
        /// 不存在建筑时放置的标志;
        /// </summary>
        public const int EmptyMark = 0;

        NodeBuildingInfo info;

        public NodeBuildingInfo Info
        {
            get { return info; }
            internal set{ info = value; }
        }

        /// <summary>
        /// 编号,不存在建筑则为0;
        /// </summary>
        [ProtoMember(1)]
        public uint ID { get; internal set; }

        [ProtoMember(2)]
        public int BuildingType
        {
            get { return info.BuildingType; }
            internal set { info.BuildingType = value; }
        }

        [ProtoMember(3)]
        public float Angle
        {
            get { return info.Angle; }
            internal set { info.Angle = value; }
        }

        /// <summary>
        /// 添加建筑物;
        /// </summary>
        public BuildingNode Update(MapData data, int buildingType, float angle)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            return Update(data.Building, buildingType, angle);
        }

        /// <summary>
        /// 添加建筑物;
        /// </summary>
        internal BuildingNode Update(IdentifierGenerator buildingInfo, int buildingType, float angle)
        {
            if (buildingInfo == null)
                throw new ArgumentNullException("buildingInfo");

            if (!Exist())
            {
                ID = buildingInfo.GetNewEffectiveID();
            }
            BuildingType = buildingType;
            Angle = angle;
            return this;
        }

        /// <summary>
        /// 是否存在该建筑物?
        /// </summary>
        public bool Exist(int buildingType)
        {
            return BuildingType == buildingType;
        }

        /// <summary>
        /// 是否存在建筑物?
        /// </summary>
        public bool Exist()
        {
            return ID != EmptyMark;
        }

        /// <summary>
        /// 清除建筑信息;
        /// </summary>
        public BuildingNode Destroy()
        {
            ID = 0;
            BuildingType = default(int);
            Angle = default(float);
            return default(BuildingNode);
        }

        public bool Equals(BuildingNode other)
        {
            return ID == other.ID 
                && BuildingType == other.BuildingType
                && Angle == other.Angle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BuildingNode))
                return false;
            return Equals((BuildingNode)obj);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public static bool operator ==(BuildingNode a, BuildingNode b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(BuildingNode a, BuildingNode b)
        {
            return !a.Equals(b);
        }
    }
}
