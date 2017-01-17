﻿using System;
using ProtoBuf;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路节点信息;
    /// </summary>
    [ProtoContract]
    public struct RoadInfo : IEquatable<RoadInfo>
    {

        /// <summary>
        /// 道路的唯一编号;
        /// </summary>
        [ProtoMember(1)]
        public uint ID;


        public bool Equals(RoadInfo other)
        {
            return other.ID == ID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RoadInfo))
                return false;
            return ((RoadInfo)obj).ID == ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override string ToString()
        {
            return "[ID:" + ID + "]";
        }

    }

}
