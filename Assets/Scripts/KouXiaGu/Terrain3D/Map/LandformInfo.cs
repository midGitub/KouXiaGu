using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地貌节点信息
    /// </summary>
    [ProtoContract]
    public class LandformInfo : IEquatable<LandformInfo>
    {

        /// <summary>
        /// 节点不存在道路时放置的标志;
        /// </summary>
        internal const uint EMPTY_LANDFORM_MARK = 0;

        /// <summary>
        /// 代表的地形ID;
        /// </summary>
        [ProtoMember(1)]
        public int ID;

        /// <summary>
        /// 地形旋转角度;
        /// </summary>
        [ProtoMember(2)]
        public float Angle;


        /// <summary>
        /// 是否设定了地貌?;
        /// </summary>
        public bool IsHaveLandform()
        {
            return ID != EMPTY_LANDFORM_MARK;
        }

        public bool Equals(LandformInfo other)
        {
            return other.IsHaveLandform() == IsHaveLandform();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LandformInfo))
                return false;
            return this.Equals((LandformInfo)obj);
        }

        public override int GetHashCode()
        {
            return IsHaveLandform().GetHashCode();
        }

        public override string ToString()
        {
            return "[ID:" + ID + ",Angle:" + Angle + "]";
        }

    }

}
