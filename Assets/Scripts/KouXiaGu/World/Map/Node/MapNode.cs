using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 地图节点;
    /// </summary>
    [ProtoContract]
    public struct MapNode
    {
        [ProtoMember(1)]
        public LandformNode Landform;

        [ProtoMember(2)]
        public RoadNode Road;

        [ProtoMember(3)]
        public BuildingNode Building;


    }
   
}
