using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图节点;
    /// </summary>
    [ProtoContract]
    [XmlRoot("Node")]
    public struct MapNode
    {
        [ProtoMember(1)]
        [XmlElement("Landform")]
        public NodeLandformInfo Landform { get; set; }

        [ProtoMember(10)]
        [XmlElement("Building")]
        public NodeBuildingInfo Building { get; set; }

        [ProtoMember(20)]
        [XmlElement("Road")]
        public NodeRoadInfo Road { get; set; }
    }
}
