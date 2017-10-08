using JiongXiaGu.Collections;
using JiongXiaGu.Grids;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Collections;
using System.Xml.Schema;

namespace JiongXiaGu.Unity.RectMaps
{


    [XmlRoot(MapXmlReader.MapRootName)]
    public class Map
    {
        /// <summary>
        /// 地图描述;
        /// </summary>
        [XmlElement(MapXmlReader.MapDescriptionElementName)]
        public MapDescription Description { get; set; }

        /// <summary>
        /// 地图数据;
        /// </summary>
        [XmlElement(MapXmlReader.MapDataElementName)]
        public MapData Data { get; set; }
    }

    /// <summary>
    /// 地图信息定义;
    /// </summary>
    public struct MapDescription : IXmlSerializable
    {
        const string NameElementName = "Name";
        const string VersionElementName = "Version";

        /// <summary>
        /// 地图名;
        /// </summary>
        [XmlElement(NameElementName)]
        public string Name { get; set; }

        /// <summary>
        /// 地图版本;
        /// </summary>
        [XmlElement(VersionElementName)]
        public int Version { get; set; }

        ///// <summary>
        ///// 兼容的地图版本;
        ///// </summary>
        //[XmlArray("CompatibleVersions")]
        //[XmlArrayItem("Version")]
        //public int[] CompatibleVersions { get; set; }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// 读取到内容;
        /// </summary>
        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将内容输出;
        /// </summary>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(NameElementName, Name);
            writer.WriteElementString(VersionElementName, Version.ToString());
        }
    }

    /// <summary>
    /// 地图数据;
    /// </summary>
    [ProtoContract(SkipConstructor = true)]
    public class MapData : IEnumerable<KeyValuePair<RectCoord, MapNode>>
    {
        /// <summary>
        /// 地图节点字典;
        /// </summary>
        public Dictionary<RectCoord, MapNode> Data { get; private set; }

        public MapData()
        {
            Data = new Dictionary<RectCoord, MapNode>();
        }

        public void Add(KeyValuePair<RectCoord, MapNode> item)
        {
            Data.Add(item.Key, item.Value);
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

        public IEnumerator<KeyValuePair<RectCoord, MapNode>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
