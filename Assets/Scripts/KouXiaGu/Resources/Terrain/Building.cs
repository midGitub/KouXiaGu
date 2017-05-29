//using System.Xml.Serialization;
//using KouXiaGu.Terrain3D;
//using KouXiaGu.Navigation;

//namespace KouXiaGu.Resources
//{

//    /// <summary>
//    /// 地形信息;
//    /// </summary>
//    [XmlType("Building")]
//    public struct BuildingInfo : IElement
//    {
//        [XmlAttribute("id")]
//        public int ID { get; set; }

//        [XmlAttribute("tag")]
//        public string Tags { get; set; }

//        [XmlElement("Terrain")]
//        public TerrainBuildingInfo TerrainInfo { get; set; }

//        [XmlIgnore]
//        public int TagsMask { get; set; }

//        [XmlIgnore]
//        public BuildingResource Terrain { get; set; }
//    }

//    class BuildingFile : MultipleFilePath
//    {
//        [CustomFilePath("建筑资源描述文件;", true)]
//        public const string fileName = "World/Terrain/Building.xml";

//        public override string FileName
//        {
//            get { return fileName; }
//        }
//    }

//    class BuildingXmlSerializer : ElementsXmlSerializer<BuildingInfo>
//    {
//        public BuildingXmlSerializer() : base(new BuildingFile())
//        {
//        }

//        public BuildingXmlSerializer(IFilePath file) : base(file)
//        {
//        }
//    }
//}
