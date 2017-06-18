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
    /// 建筑信息;
    /// </summary>
    [XmlType("Building")]
    public class BuildingInfo : IElement
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("tags")]
        public string Tags { get; set; }

        [XmlElement("Terrain")]
        public TerrainBuildingInfo TerrainInfo { get; set; }

        [XmlIgnore]
        public int TagsMask { get; internal set; }

        [XmlIgnore]
        public BuildingResource Terrain { get; internal set; }

        public override string ToString()
        {
            return "[ID:" + ID + ",Tags:" + Tags + "]";
        }
    }

    class BuildingInfoXmlSerializer : XmlElementsReaderWriter<BuildingInfo>
    {
        public BuildingInfoXmlSerializer() : base(new BuildingFile())
        {
        }
    }

    class BuildingFile : MultipleFilePath
    {
        [CustomFilePath("建筑资源描述文件;", true)]
        public const string fileName = "World/Terrain/Building.xml";

        public override string FileName
        {
            get { return fileName; }
        }
    }
}
