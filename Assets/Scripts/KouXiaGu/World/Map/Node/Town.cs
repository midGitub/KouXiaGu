using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 节点城镇信息;
    /// </summary>
    [ProtoContract]
    public struct TownNode : IEquatable<TownNode>
    {
        /// <summary>
        /// 城镇编号,若不存在则为0;
        /// </summary>
        [ProtoMember(1)]
        public int TownID { get; set; }

        /// <summary>
        /// 返回是否存在城镇;
        /// </summary>
        public bool Exist()
        {
            return TownID != 0;
        }

        /// <summary>
        /// 销毁该点城镇信息;
        /// </summary>
        public RoadNode Destroy()
        {
            return default(RoadNode);
        }

        public bool Equals(TownNode other)
        {
            return TownID == other.TownID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TownNode))
                return false;
            return Equals((TownNode)obj);
        }

        public override int GetHashCode()
        {
            return TownID;
        }

        public static bool operator ==(TownNode a, TownNode b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(TownNode a, TownNode b)
        {
            return !a.Equals(b);
        }
    }
}
