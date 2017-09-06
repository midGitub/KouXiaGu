using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu.World.RectMap
{

    [ProtoContract]
    public struct NodeRoadInfo : IEquatable<NodeRoadInfo>
    {
        /// <summary>
        /// 道路类型;
        /// </summary>
        [ProtoMember(1)]
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
            return TypeID;
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
