using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Maps
{

    [ProtoContract]
    public struct NodeRoadInfo : IEquatable<NodeRoadInfo>
    {
        /// <summary>
        /// 道路类型,0代表不存在;
        /// </summary>
        [ProtoMember(1)]
        [XmlAttribute("ID")]
        public int TypeID { get; set; }

        public bool Exist()
        {
            return TypeID != 0;
        }

        public bool Equals(NodeRoadInfo other)
        {
            return TypeID == other.TypeID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NodeRoadInfo))
                return false;
            return Equals((NodeRoadInfo)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = -173284040;
            hashCode = hashCode * -1521134295 + TypeID.GetHashCode();
            return hashCode;
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

}
