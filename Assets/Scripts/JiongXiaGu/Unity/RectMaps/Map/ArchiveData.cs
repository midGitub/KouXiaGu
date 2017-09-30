using JiongXiaGu.Grids;
using ProtoBuf;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.RectMaps
{
    /// <summary>
    /// 地图存档数据;
    /// </summary>
    [ProtoContract]
    public class ArchiveData
    {
        /// <summary>
        /// 地图节点字典;
        /// </summary>
        [ProtoMember(1)]
        public Dictionary<RectCoord, MapNode> Data { get; internal set; }
    }
}
