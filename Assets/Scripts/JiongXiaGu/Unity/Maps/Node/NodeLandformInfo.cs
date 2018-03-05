using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Maps
{

    /// <summary>
    /// 节点地貌信息;
    /// </summary>
    [ProtoContract]
    public struct NodeLandformInfo : IEquatable<NodeLandformInfo>
    {
        /// <summary>
        /// 地形类型;
        /// </summary>
        [ProtoMember(1)]
        [XmlAttribute("id")]
        public string TypeID { get; set; }

        /// <summary>
        /// 地形旋转角度;
        /// </summary>
        [ProtoMember(2)]
        [XmlAttribute("angle")]
        public float Angle { get; set; }

        /// <summary>
        /// 是否定义了地形数据?
        /// </summary>
        public bool Exist()
        {
            return !string.IsNullOrWhiteSpace(TypeID);
        }

        public bool Equals(NodeLandformInfo other)
        {
            return 
                TypeID == other.TypeID 
                && Angle == other.Angle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NodeLandformInfo))
                return false;
            return Equals((NodeLandformInfo)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = -654319184;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TypeID);
            hashCode = hashCode * -1521134295 + Angle.GetHashCode();
            return hashCode;
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
}
