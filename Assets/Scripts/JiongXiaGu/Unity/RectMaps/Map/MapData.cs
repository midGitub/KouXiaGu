using JiongXiaGu.Collections;
using JiongXiaGu.Grids;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Collections;
using System.Linq;

namespace JiongXiaGu.Unity.RectMaps
{


    [XmlRoot(MapXmlReader.MapRootName)]
    public class Map
    {
        /// <summary>
        /// 地图名;
        /// </summary>
        [XmlAttribute(MapXmlReader.MapNameAttributeName)]
        public string Name { get; set; }

        /// <summary>
        /// 地图版本;
        /// </summary>
        [XmlAttribute(MapXmlReader.MapVersionAttributeName)]
        public int Version { get; set; }

        /// <summary>
        /// 地图数据;
        /// </summary>
        [XmlElement(MapXmlReader.MapNodeElementName)]
        public MapData Data { get; set; }

        public Map()
        {
            Data = new MapData();
        }

        public Map(string name, int version) : this()
        {
            Name = name;
            Version = version;
        }
    }

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

        /// <summary>
        /// 添加存档数据;
        /// </summary>
        public void AddArchive(ArchiveData archiveData)
        {
            if (archiveData == null)
                throw new ArgumentNullException("archiveData");

            Data.AddOrUpdate(archiveData.Data);
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
