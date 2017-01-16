using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using ProtoBuf;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路节点信息;
    /// </summary>
    [ProtoContract]
    public struct Road : IEquatable<Road>
    {

        /// <summary>
        /// 道路表示的编号;
        /// </summary>
        [ProtoMember(1)]
        public int ID;

        /// <summary>
        /// 下一个节点方向;
        /// </summary>
        [ProtoMember(2)]
        public HexDirections Next;

        /// <summary>
        /// 上一个节点方向;
        /// </summary>
        [ProtoMember(3)]
        public HexDirections Previous;


        public bool Equals(Road other)
        {
            return
                ID == other.ID &&
                Next == other.Next &&
                Previous == other.Previous;
        }

    }

}
