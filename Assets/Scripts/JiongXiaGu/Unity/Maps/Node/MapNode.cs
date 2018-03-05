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
    public struct MapNode : IEquatable<MapNode>
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
            MapNodeChangeContents changeContents = 0;

            if (originalValue.Landform != newValue.Landform)
            {
                changeContents |= MapNodeChangeContents.Landform;
            }
            if (originalValue.Building != newValue.Building)
            {
                changeContents |= MapNodeChangeContents.Building;
            }


            return changeContents;
        }

        public override bool Equals(object obj)
        {
            return obj is MapNode && Equals((MapNode)obj);
        }

        public bool Equals(MapNode other)
        {
            return Landform.Equals(other.Landform) &&
                   Building.Equals(other.Building) &&
                   Road.Equals(other.Road);
        }

        public override int GetHashCode()
        {
            var hashCode = -801783822;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<NodeLandformInfo>.Default.GetHashCode(Landform);
            hashCode = hashCode * -1521134295 + EqualityComparer<NodeBuildingInfo>.Default.GetHashCode(Building);
            hashCode = hashCode * -1521134295 + EqualityComparer<NodeRoadInfo>.Default.GetHashCode(Road);
            return hashCode;
        }

        public static bool operator ==(MapNode node1, MapNode node2)
        {
            return node1.Equals(node2);
        }

        public static bool operator !=(MapNode node1, MapNode node2)
        {
            return !(node1 == node2);
        }
    }
}
