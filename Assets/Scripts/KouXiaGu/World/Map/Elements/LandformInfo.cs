using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using KouXiaGu.Terrain3D;
using KouXiaGu.Collections;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 地形信息;
    /// </summary>
    [XmlType("Landform")]
    public struct LandformInfo
    {
        [XmlAttribute("id")]
        public int ID;

        [XmlAttribute("name")]
        public string Name;

        [XmlElement("Terrain")]
        public TerrainLandformInfo Terrain;
    }

    /// <summary>
    /// 地形信息文件路径;
    /// </summary>
    class LandformInfosFilePath : CustomFilePath
    {
        const string fileName = "World/Landform";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    /// <summary>
    /// 地形信息读取;
    /// </summary>
    public class LandformInfoXmlSerializer : IReaderWriter<Dictionary<int, LandformInfo>, LandformInfo[]>
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(LandformInfo[]));
        static readonly LandformInfosFilePath file = new LandformInfosFilePath();

        protected string FileExtension
        {
            get { return ".xml"; }
        }

        public Dictionary<int, LandformInfo> Read()
        {
            Dictionary<int, LandformInfo> dictionary = new Dictionary<int, LandformInfo>();
            var filePaths = GetFilePaths();

            foreach (var filePath in filePaths)
            {
                var infos = Read(filePath);
                AddOrUpdate(dictionary, infos);
            }

            return dictionary;
        }

        void AddOrUpdate(Dictionary<int, LandformInfo> dictionary, IEnumerable<LandformInfo> infos)
        {
            foreach (var info in infos)
            {
                dictionary.AddOrUpdate(info.ID, info);
            }
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

        public LandformInfo[] Read(string filePath)
        {
            var item = (LandformInfo[])serializer.DeserializeXiaGu(filePath);
            return item;
        }

        /// <summary>
        /// 输出/保存到 文件夹下;
        /// </summary>
        /// <param name="infos">内容</param>
        /// <param name="dirPath">输出的文件夹</param>
        public void Write(LandformInfo[] infos, string dirPath)
        {
            string filePath = file.Combine(dirPath);
            filePath = Path.ChangeExtension(filePath, FileExtension);
            serializer.SerializeXiaGu(filePath, infos);
        }
    }

}
