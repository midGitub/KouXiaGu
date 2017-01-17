using ProtoBuf;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路节点信息;
    /// </summary>
    [ProtoContract]
    public struct RoadInfo
    {
        /// <summary>
        /// 道路的唯一编号;
        /// </summary>
        [ProtoMember(1)]
        public uint ID;
    }

}
