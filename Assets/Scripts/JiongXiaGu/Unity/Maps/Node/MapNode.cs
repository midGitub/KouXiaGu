using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Maps
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

        /// <summary>
        /// 获取到地图节点变化内容;
        /// </summary>
        public static MapNodeChangeContents GetNodeChangeElements(MapNode originalValue, MapNode newValue)
        {
            throw new NotImplementedException();
        }
    }
}
