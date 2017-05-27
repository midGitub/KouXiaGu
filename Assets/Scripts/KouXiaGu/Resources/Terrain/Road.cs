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
    public class RoadInfo : ElementInfo
    {
        [XmlElement("Terrain")]
        public TerrainRoadInfo Terrain;
    }

    public class RoadInfoFilePath : CustomFilePath
    {
        public RoadInfoFilePath(string fileExtension) : base(fileExtension)
        {
        }

        public override string FileName
        {
            get { return "World/Road"; }
        }
    }

    /// <summary>
    /// 道路信息读取;
    /// </summary>
    public class RoadInfoXmlSerializer : DataDictionaryXmlReader<RoadInfo>
    {
        public RoadInfoXmlSerializer()
        {
            file = new RoadInfoFilePath(FileExtension);
        }

        RoadInfoFilePath file;

        public override CustomFilePath File
        {
            get { return file; }
        }
    }

}
