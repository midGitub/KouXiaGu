﻿using System;
using ProtoBuf;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路节点信息;
    /// </summary>
    [ProtoContract]
    public struct RoadNode : IEquatable<RoadNode>
    {
        /// <summary>
        /// 节点不存在道路时放置的标志;
        /// </summary>
        internal const uint EMPTY_MARK = 0;

        /// <summary>
        /// 道路的唯一编号;
        /// </summary>
        [ProtoMember(1)]
        public uint ID;


        /// <summary>
        /// 是否存在道路;
        /// </summary>
        public bool Exist()
        {
            return ID != EMPTY_MARK;
        }

        public bool Equals(RoadNode other)
        {
            return other.Exist() == Exist();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RoadNode))
                return false;
            return this.Equals((RoadNode)obj);
        }

        public override int GetHashCode()
        {
            return Exist().GetHashCode();
        }

        public override string ToString()
        {
            return "[ID:" + ID + "]";
        }

    }

}