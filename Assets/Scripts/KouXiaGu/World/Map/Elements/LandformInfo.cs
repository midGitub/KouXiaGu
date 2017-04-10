using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using KouXiaGu.Terrain3D;

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
    public class LandformInfoXmlSerializer : IReader<List<LandformInfo>>, IReader<Dictionary<int, LandformInfo>>
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(LandformInfo[]));
        static readonly LandformInfosFilePath file = new LandformInfosFilePath();

        protected string FileExtension
        {
            get { return ".xml"; }
        }

        Dictionary<int, LandformInfo> IReader<Dictionary<int, LandformInfo>>.Read()
        {
            var infoArray = Read();
            var infoDictionary = infoArray.ToDictionary(item => item.ID);
            return infoDictionary;
        }

        public List<LandformInfo> Read()
        {
            List<LandformInfo> item = new List<LandformInfo>();
            foreach (var filePath in file.GetFilePaths())
            {
                string newPath = Path.ChangeExtension(filePath, FileExtension);
                if (File.Exists(newPath))
                {
                    var array = Read(newPath);
                    item.AddRange(array);
                }
            }
            return item;
        }

        public LandformInfo[] Read(string filePath)
        {
            var item = (LandformInfo[])serializer.DeserializeXiaGu(filePath);
            return item;
        }

    }


}
