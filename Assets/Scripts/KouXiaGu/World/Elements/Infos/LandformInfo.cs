using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using KouXiaGu.Terrain3D;
using KouXiaGu.Collections;

namespace KouXiaGu.World
{

    /// <summary>
    /// 地形信息;
    /// </summary>
    [XmlType("Landform")]
    public struct LandformInfo : IMarked
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("name")]
        public string Name;

        [XmlElement("Terrain")]
        public TerrainLandformInfo Terrain;
    }

    /// <summary>
    /// 地形信息文件路径;
    /// </summary>
    public class LandformInfosFilePath : CustomFilePath
    {
        public LandformInfosFilePath(string fileExtension) : base(fileExtension)
        {
        }

        public override string FileName
        {
            get { return "World/Landform"; }
        }
    }

    /// <summary>
    /// 地形信息读取;
    /// </summary>
    public class LandformInfoXmlSerializer : DataXmlSerializer<LandformInfo>
    {
        public LandformInfoXmlSerializer()
        {
            file = new LandformInfosFilePath(FileExtension);
        }

        LandformInfosFilePath file;

        public override CustomFilePath File
        {
            get { return file; }
        }

    }

}
