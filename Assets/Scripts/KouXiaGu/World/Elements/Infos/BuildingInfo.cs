using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KouXiaGu.Collections;

namespace KouXiaGu.World
{

    /// <summary>
    /// 地形信息;
    /// </summary>
    [XmlType("Building")]
    public struct BuildingInfo : IMarked
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("name")]
        public string Name;
    }

    /// <summary>
    /// 地形信息文件路径;
    /// </summary>
    public class BuildingInfosFilePath : CustomFilePath
    {
        public BuildingInfosFilePath(string fileExtension) : base(fileExtension)
        {
        }

        public override string FileName
        {
            get { return "World/Building"; }
        }
    }

    /// <summary>
    /// 地形信息读取;
    /// </summary>
    public class BuildingInfosXmlSerializer : DataDictionaryXmlReader<BuildingInfo>
    {
        public BuildingInfosXmlSerializer()
        {
            file = new BuildingInfosFilePath(FileExtension);
        }

        BuildingInfosFilePath file;

        public override CustomFilePath File
        {
            get { return file; }
        }
    }

}
