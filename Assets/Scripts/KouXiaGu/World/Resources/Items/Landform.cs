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
    /// 地形信息;
    /// </summary>
    [XmlType("Landform")]
    public class LandformInfo : IElement
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("tags")]
        public string Tags { get; set; }

        [XmlElement("Terrain")]
        public TerrainLandformInfo TerrainInfo { get; set; }

        [XmlIgnore]
        public int TagsMask { get; internal set; }

        [XmlIgnore]
        public LandformResource Terrain { get; internal set; }

        public override string ToString()
        {
            return "[ID:" + ID + ",Tags:" + Tags + "]";
        }
    }

    class LandformFile : MultipleFilePath
    {
        [CustomFilePath("地形资源描述文件;", true)]
        public const string fileName = "World/Resources/Landform.xml";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    class LandformInfoXmlSerializer : XmlElementsReaderWriter<LandformInfo>
    {
        public LandformInfoXmlSerializer() : base(new LandformFile())
        {
        }
    }
}
