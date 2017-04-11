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
    public class LandformInfoXmlSerializer : DataReader<Dictionary<int, LandformInfo>, LandformInfo[]>, 
        IReaderWriter<Dictionary<int, LandformInfo>, LandformInfo[]>
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(LandformInfo[]));

        public LandformInfoXmlSerializer()
        {
            file = new LandformInfosFilePath(FileExtension);
        }

        LandformInfosFilePath file;

        public override string FileExtension
        {
            get { return ".xml"; }
        }

        public override CustomFilePath File
        {
            get { return file; }
        }

        public override Dictionary<int, LandformInfo> Read(IEnumerable<string> filePaths)
        {
            Dictionary<int, LandformInfo> dictionary = new Dictionary<int, LandformInfo>();

            foreach (var filePath in filePaths)
            {
                var infos = Read(filePath);
                AddOrUpdate(dictionary, infos);
            }

            return dictionary;
        }

        LandformInfo[] Read(string filePath)
        {
            var item = (LandformInfo[])serializer.DeserializeXiaGu(filePath);
            return item;
        }

        void AddOrUpdate(Dictionary<int, LandformInfo> dictionary, IEnumerable<LandformInfo> infos)
        {
            foreach (var info in infos)
            {
                dictionary.AddOrUpdate(info.ID, info);
            }
        }

        public override void Write(LandformInfo[] item, string filePath)
        {
            serializer.SerializeXiaGu(filePath, item);
        }

        ///// <summary>
        ///// 输出/保存到 文件夹下;
        ///// </summary>
        ///// <param name="infos">内容</param>
        ///// <param name="dirPath">输出的文件夹</param>
        //public void Write(LandformInfo[] infos, string dirPath)
        //{
        //    string filePath = file.Combine(dirPath);
        //    filePath = Path.ChangeExtension(filePath, FileExtension);
        //    serializer.SerializeXiaGu(filePath, infos);
        //}
    }

}
