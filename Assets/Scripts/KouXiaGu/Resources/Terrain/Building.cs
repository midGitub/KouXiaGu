using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KouXiaGu.Collections;
using KouXiaGu.Terrain3D;
using KouXiaGu.Navigation;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// 地形信息;
    /// </summary>
    [XmlType("Building")]
    public struct BuildingInfo : IElement
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlElement("Terrain")]
        public TerrainBuildingInfo Terrain { get; set; }

        [XmlElement("Navigation")]
        public NavBuildingInfo Navigation { get; set; }
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

    class BuildingXmlSerializer : ElementsXmlSerializer<BuildingInfo>
    {
        public BuildingXmlSerializer() : base(new BuildingFile())
        {
        }

        public BuildingXmlSerializer(IFilePath file) : base(file)
        {
        }
    }
}
