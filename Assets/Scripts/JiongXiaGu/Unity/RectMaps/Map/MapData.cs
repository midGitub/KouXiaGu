using JiongXiaGu.Grids;
using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using JiongXiaGu.Collections;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图数据;可通过 Proto 或 XML 进行序列化;
    /// </summary>
    [ProtoContract(SkipConstructor = false)]
    public class MapData : IEnumerable<MapData.NodeItem>
    {
        /// <summary>
        /// 地图节点字典;
        /// </summary>
        public Dictionary<RectCoord, MapNode> Data { get; private set; }

        public MapData()
        {
            Data = new Dictionary<RectCoord, MapNode>();
        }

        public void Add(NodeItem item)
        {
            Data.AddOrUpdate(item.Point, item.Node);
        }

        public IEnumerator<NodeItem> GetEnumerator()
        {
            foreach (var item in Data)
            {
                NodeItem node = new NodeItem(item.Key, item.Value);
                yield return node;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 提供序列化使用;
        /// </summary>\
        [ProtoContract]
        public struct NodeItem
        {
            [ProtoMember(0)]
            [XmlElement("Point")]
            public RectCoord Point { get; set; }

            [ProtoMember(1)]
            [XmlElement("Node")]
            public MapNode Node { get; set; }

            public NodeItem(RectCoord point, MapNode node)
            {
                Point = point;
                Node = node;
            }
        }
    }
}
