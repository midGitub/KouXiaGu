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

    class RoadInfoFilePath : CustomFilePath
    {
        const string fileName = "World/Road";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    /// <summary>
    /// 道路信息读取;
    /// </summary>
    public class RoadInfoXmlReader : IReader<Dictionary<int, RoadInfo>>, IWriter<RoadInfo[]>
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(RoadInfo[]));
        static readonly RoadInfoFilePath file = new RoadInfoFilePath();

        protected string FileExtension
        {
            get { return ".xml"; }
        }

        public Dictionary<int, RoadInfo> Read()
        {
            Dictionary<int, RoadInfo> dictionary = new Dictionary<int, RoadInfo>();
            var filePaths = GetFilePaths();

            foreach (var filePath in filePaths)
            {
                var infos = Read(filePath);
                AddOrUpdate(dictionary, infos);
            }

            return dictionary;
        }

        IEnumerable<string> GetFilePaths()
        {
            foreach (var path in file.GetFilePaths())
            {
                string newPath = Path.ChangeExtension(path, FileExtension);

                if (File.Exists(newPath))
                {
                    yield return newPath;
                }
            }
        }

        void AddOrUpdate(Dictionary<int, RoadInfo> dictionary, IEnumerable<RoadInfo> infos)
        {
            foreach (var info in infos)
            {
                dictionary.AddOrUpdate(info.ID, info);
            }
        }

        protected RoadInfo[] Read(string filePath)
        {
            var item = (RoadInfo[])serializer.DeserializeXiaGu(filePath);
            return item;
        }


        /// <summary>
        /// 输出覆盖保存到主要文件上;
        /// </summary>
        public void Write(RoadInfo[] infos)
        {
            Write(infos, file.MainFilePath);
        }

        public void Write(RoadInfo[] infos, string filePath)
        {
            serializer.SerializeXiaGu(filePath, infos);
        }
    }

}
