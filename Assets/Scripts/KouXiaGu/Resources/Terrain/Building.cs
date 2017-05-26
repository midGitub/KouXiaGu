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
        public NavigationBuildingInfo Navigation { get; set; }
    }

    class BuildingFile : FilePath
    {
        public override string FileName
        {
            get { return "World/Building.xml"; }
        }
    }

    class BuildingXmlSerializer : ElementXmlSerializer<BuildingInfo>
    {
        public BuildingXmlSerializer() : base(new BuildingFile())
        {
        }

        public BuildingXmlSerializer(IFilePath file) : base(file)
        {
        }
    }
}
