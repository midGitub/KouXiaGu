using JiongXiaGu.Collections;
using JiongXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图数据;
    /// </summary>
    [XmlRoot(MapXmlReader.MapRootName)]
    public sealed class Map
    {
        MapDescription description;

        /// <summary>
        /// 地图数据;
        /// </summary>
        [XmlElement(MapXmlReader.MapNodeElementName)]
        public MapData MapData { get; set; }

        Map()
        {
            MapData = new MapData();
        }

        public Map(MapDescription description) : this()
        {
            this.description = description;
        }

        public Map(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// 地图描述;
        /// </summary>
        [XmlIgnore]
        public MapDescription Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 地图名;
        /// </summary>
        [XmlAttribute(MapXmlReader.MapNameAttributeName)]
        public string Name
        {
            get { return description.Name; }
            set { description.Name = value; }
        }

        /// <summary>
        /// 地图版本;
        /// </summary>
        [XmlAttribute(MapXmlReader.MapVersionAttributeName)]
        public int Version
        {
            get { return description.Version; }
            set { description.Version = value; }
        }

        /// <summary>
        /// 是否为存档?
        /// </summary>
        [XmlAttribute(MapXmlReader.MapIsArchivedAttributeName)]
        public bool IsArchived
        {
            get { return description.IsArchived; }
            set { description.IsArchived = value; }
        }

        /// <summary>
        /// 地图数据;
        /// </summary>
        public IDictionary<RectCoord, MapNode> Data
        {
            get { return MapData.Data; }
        }

        /// <summary>
        /// 添加存档数据;
        /// </summary>
        public void AddArchive(Map archiveMap)
        {
            if (archiveMap == null)
                throw new ArgumentNullException("archiveData");
            if (archiveMap.Name != Name)
                throw new InvalidOperationException("不允许合并不同的地图");
            if (!archiveMap.IsArchived)
                throw new InvalidOperationException("传入参数不为存档;");

            Data.AddOrUpdate(archiveMap.Data);
        }
    }
}
