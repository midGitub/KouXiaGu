using JiongXiaGu.Grids;
using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图数据;
    /// </summary>
    [ProtoContract(SkipConstructor = true)]
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
            Data.Add(item.Point, item.Node);
        }

        public IEnumerator<NodeItem> GetEnumerator()
        {
            foreach (var item in Data)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct NodeItem
        {
            [XmlElement("Point")]
            public RectCoord Point { get; set; }

            [XmlElement("Node")]
            public MapNode Node { get; set; }

            public NodeItem(RectCoord point, MapNode node)
            {
                Point = point;
                Node = node;
            }

            public static implicit operator KeyValuePair<RectCoord, MapNode>(NodeItem item)
            {
                return new KeyValuePair<RectCoord, MapNode>(item.Point, item.Node);
            }

            public static implicit operator NodeItem(KeyValuePair<RectCoord, MapNode> pair)
            {
                return new NodeItem(pair.Key, pair.Value);
            }
        }
    }
}
