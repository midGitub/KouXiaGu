using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KouXiaGu.Collections;
using KouXiaGu.Terrain3D;
using KouXiaGu.Navigation;

namespace KouXiaGu.World
{

    /// <summary>
    /// 地形信息;
    /// </summary>
    [XmlType("Building")]
    public class BuildingInfo : ElementInfo
    {
        [XmlElement("Terrain")]
        public TerrainBuildingInfo Terrain { get; set; }

        [XmlElement("Navigation")]
        public NavigationBuildingInfo Navigation;
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
