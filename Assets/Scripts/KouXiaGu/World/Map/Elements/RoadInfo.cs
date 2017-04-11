using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using KouXiaGu.Terrain3D;
using KouXiaGu.Collections;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 道路信息;
    /// </summary>
    [XmlType("Road")]
    public struct RoadInfo
    {
        [XmlAttribute("id")]
        public int ID;

        [XmlAttribute("name")]
        public string Name;

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
    public class RoadInfoXmlSerializer : DataReader<Dictionary<int, RoadInfo>, RoadInfo[]>
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(RoadInfo[]));

        public RoadInfoXmlSerializer()
        {
            file = new RoadInfoFilePath(FileExtension);
        }

        RoadInfoFilePath file;

        public override string FileExtension
        {
            get { return ".xml"; }
        }

        public override CustomFilePath File
        {
            get { return file; }
        }

        public override Dictionary<int, RoadInfo> Read(IEnumerable<string> filePaths)
        {
            Dictionary<int, RoadInfo> dictionary = new Dictionary<int, RoadInfo>();

            foreach (var filePath in filePaths)
            {
                var infos = Read(filePath);
                AddOrUpdate(dictionary, infos);
            }

            return dictionary;
        }

        RoadInfo[] Read(string filePath)
        {
            var item = (RoadInfo[])serializer.DeserializeXiaGu(filePath);
            return item;
        }

        void AddOrUpdate(Dictionary<int, RoadInfo> dictionary, IEnumerable<RoadInfo> infos)
        {
            foreach (var info in infos)
            {
                dictionary.AddOrUpdate(info.ID, info);
            }
        }

        public override void Write(RoadInfo[] infos, string filePath)
        {
            serializer.SerializeXiaGu(filePath, infos);
        }
    }

}
