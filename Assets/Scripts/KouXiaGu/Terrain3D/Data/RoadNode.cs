using System;
using ProtoBuf;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路节点信息;
    /// </summary>
    [ProtoContract]
    public struct RoadNode
    {
        /// <summary>
        /// 节点不存在道路时放置的标志;
        /// </summary>
        internal const uint EmptyMark = 0;

        /// <summary>
        /// 道路的唯一编号;
        /// </summary>
        [ProtoMember(1)]
        public uint ID;


        /// <summary>
        /// 是否存在道路;
        /// </summary>
        public bool ExistRoad()
        {
            return ID != EmptyMark;
        }

        public override string ToString()
        {
            return "[ID:" + ID + "]";
        }

    }

}
