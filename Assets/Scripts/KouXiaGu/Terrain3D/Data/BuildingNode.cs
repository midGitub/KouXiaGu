using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.Terrain3D
{


    [ProtoContract]
    public struct BuildingNode : IEquatable<BuildingNode>
    {

        /// <summary>
        /// 节点不存在道路时放置的标志;
        /// </summary>
        internal const int EMPTY_MARK = 0;

        /// <summary>
        /// 建筑物类型编号;
        /// </summary>
        [ProtoMember(1)]
        public int ID;

        /// <summary>
        /// 建筑物旋转的角度;
        /// </summary>
        [ProtoMember(2)]
        public float Angle;


        /// <summary>
        /// 是否存在建筑物?;
        /// </summary>
        public bool Exist()
        {
            return ID != EMPTY_MARK;
        }

        public bool Equals(BuildingNode other)
        {
            return
               other.ID == ID &&
               other.Angle == Angle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BuildingNode))
                return false;
            return this.Equals((BuildingNode)obj);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override string ToString()
        {
            return "[ID:" + ID + ",Angle:" + Angle + "]";
        }

    }

}
