using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 节点建筑信息;
    /// </summary>
    [ProtoContract]
    public struct NodeBuildingInfo : IEquatable<NodeBuildingInfo>
    {
        /// <summary>
        /// 建筑类型,0代表不存在;
        /// </summary>
        [ProtoMember(1)]
        [XmlAttribute("id")]
        public int TypeID { get; set; }

        /// <summary>
        /// 建筑旋转角度;
        /// </summary>
        [ProtoMember(2)]
        [XmlAttribute("angle")]
        public float Angle { get; set; }

        /// <summary>
        /// 是否定义了建筑数据?
        /// </summary>
        public bool Exist()
        {
            return TypeID != 0;
        }

        public bool Equals(NodeBuildingInfo other)
        {
            return TypeID == other.TypeID && Angle == other.Angle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NodeBuildingInfo))
                return false;
            return Equals((NodeBuildingInfo)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = 969184184;
            hashCode = hashCode * -1521134295 + TypeID.GetHashCode();
            hashCode = hashCode * -1521134295 + Angle.GetHashCode();
            return hashCode;
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
}
