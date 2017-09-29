using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图节点;
    /// </summary>
    [ProtoContract]
    public struct MapNode
    {
        [ProtoMember(1)]
        public NodeLandformInfo Landform { get; set; }

        [ProtoMember(10)]
        public NodeBuildingInfo Building { get; set; }

        [ProtoMember(20)]
        public NodeRoadInfo Road { get; set; }
    }
}
