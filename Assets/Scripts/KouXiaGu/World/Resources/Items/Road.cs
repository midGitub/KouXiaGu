using KouXiaGu.Resources;
using KouXiaGu.Terrain3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.World.Resources
{

    /// <summary>
    /// 道路信息;
    /// </summary>
    [XmlType("Road")]
    public class RoadInfo : IElement
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlElement("Terrain")]
        public TerrainRoadInfo TerrainInfo { get; set; }

        [XmlIgnore]
        public RoadResource Terrain { get; internal set; }
    }

    class RoadFile : MultipleFilePath
    {
        [CustomFilePath("道路资源描述文件;", true)]
        public const string fileName = "World/Terrain/Road.xml";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    class RoadInfoXmlSerializer : XmlElementsReaderWriter<RoadInfo>
    {
        public RoadInfoXmlSerializer() : base(new RoadFile())
        {
        }
    }
}
