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
        const string fileName = "World/Road";

        public override string FileName
        {
            get { return fileName; }
        }

        public RoadInfoFilePath(string fileExtension) : base(fileExtension)
        {
        }
    }

    /// <summary>
    /// 道路信息读取;
    /// </summary>
    public class RoadInfoXmlSerializer : IReaderWriter<Dictionary<int, RoadInfo>, RoadInfo[]>
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(RoadInfo[]));

        public RoadInfoXmlSerializer()
        {
            File = new RoadInfoFilePath(FileExtension);
        }

        public RoadInfoFilePath File { get; private set; }

        public string FileExtension
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
            foreach (var path in File.GetFilePaths())
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
        /// 输出/保存到 文件夹下;
        /// </summary>
        /// <param name="infos">内容</param>
        /// <param name="dirPath">输出的文件夹</param>
        public void Write(RoadInfo[] infos, string dirPath)
        {
            string filePath = File.Combine(dirPath);
            filePath = Path.ChangeExtension(filePath, FileExtension);
            serializer.SerializeXiaGu(filePath, infos);
        }
    }

}
