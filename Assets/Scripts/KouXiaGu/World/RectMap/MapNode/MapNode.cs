using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu.World.RectMap
{

    /// <summary>
    /// 地图节点;
    /// </summary>
    [ProtoContract]
    public struct MapNode
    {
        [ProtoMember(1)]
        public NodeLandformInfo Landform { get; set; }

        [ProtoMember(2)]
        public NodeBuildingInfo Building { get; set; }

        [ProtoMember(3)]
        public NodeRoadInfo Road { get; set; }
    }
}
