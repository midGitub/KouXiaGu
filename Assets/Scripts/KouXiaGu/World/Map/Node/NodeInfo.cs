using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 节点信息;
    /// </summary>
    [ProtoContract]
    public struct NodeInfo
    {
        [ProtoMember(1)]
        public NodeLandformInfo Landform { get; set; }

        [ProtoMember(2)]
        public NodeRoadInfo Road { get; set; }

        [ProtoMember(3)]
        public NodeBuildingInfo Building { get; set; }
    }
}
