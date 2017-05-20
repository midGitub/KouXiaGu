using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 节点建筑信息;
    /// </summary>
    [ProtoContract]
    public struct BuildingNode : IEquatable<BuildingNode>
    {
        /// <summary>
        /// 不存在建筑时放置的标志;
        /// </summary>
        public const int EmptyMark = 0;

        /// <summary>
        /// 编号,不存在建筑则为0;
        /// </summary>
        [ProtoMember(0)]
        public uint ID;

        /// <summary>
        /// 建筑物类型编号;
        /// </summary>
        [ProtoMember(1)]
        public int BuildingType;

        /// <summary>
        /// 建筑物旋转的角度;
        /// </summary>
        [ProtoMember(2)]
        public float Angle;

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
            return default(BuildingNode);
        }

        /// <summary>
        /// 更新建筑信息;
        /// </summary>
        public BuildingNode Update(MapData data, int buildingType, float angle)
        {
            return Update(data.Building, buildingType, angle);
        }

        /// <summary>
        /// 更新建筑信息;
        /// </summary>
        public BuildingNode Update(IdentifierGenerator buildingInfo, int buildingType, float angle)
        {
            if (!Exist())
            {
                ID = buildingInfo.GetNewEffectiveID();
            }
            BuildingType = buildingType;
            Angle = angle;
            return this;
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
