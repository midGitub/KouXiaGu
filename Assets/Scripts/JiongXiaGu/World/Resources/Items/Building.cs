using JiongXiaGu.Resources;
using JiongXiaGu.Terrain3D;
using JiongXiaGu.World.Commerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace JiongXiaGu.World.Resources
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

        [XmlElement("Commerce")]
        public BuildingCommerceInfo Commerce { get; set; }

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
        public const string fileName = "World/Resources/Building.xml";

        public override string FileName
        {
            get { return fileName; }
        }
    }
}
