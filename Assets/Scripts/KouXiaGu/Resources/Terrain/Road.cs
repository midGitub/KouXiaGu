using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using KouXiaGu.Terrain3D;
using KouXiaGu.Collections;
using KouXiaGu.Navigation;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// 道路信息;
    /// </summary>
    [XmlType("Road")]
    public struct RoadInfo : IElement
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlElement("Terrain")]
        public TerrainRoadInfo Terrain;
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

    class RoadXmlSerializer : ElementsXmlSerializer<RoadInfo>
    {
        public RoadXmlSerializer() : base(new RoadFile())
        {
        }

        public RoadXmlSerializer(IFilePath file) : base(file)
        {
        }
    }
}
