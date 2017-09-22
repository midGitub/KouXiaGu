using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace JiongXiaGu.World.Map
{

    /// <summary>
    /// 地图节点;
    /// </summary>
    [ProtoContract]
    public struct MapNode
    {
        [ProtoMember(1)]
        public LandformNode Landform { get; set; }

        [ProtoMember(2)]
        public RoadNode Road { get; set; }

        [ProtoMember(3)]
        public BuildingNode Building { get; set; }

        [ProtoMember(4)]
        public TownNode Town { get; set; }

        public static implicit operator NodeInfo(MapNode node)
        {
            return new NodeInfo()
            {
                Building = new NodeBuildingInfo()
                {
                    BuildingType = node.Building.BuildingType,
                    Angle = node.Building.Angle,
                },

                Landform = new NodeLandformInfo()
                {
                    LandformType = node.Landform.LandformType,
                    Angle = node.Landform.Angle,
                },

                Road = new NodeRoadInfo()
                {
                    RoadType = node.Road.RoadType,
                },
            };
        }
    }
}
