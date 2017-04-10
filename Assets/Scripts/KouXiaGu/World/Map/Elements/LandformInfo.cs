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
        internal const string ArrayFileName = "World/Landform";

        [XmlAttribute("id")]
        public int ID;

        [XmlAttribute("name")]
        public string Name;

        [XmlElement("Terrain")]
        public TerrainLandformInfo Terrain;
    }


    /// <summary>
    /// 地形信息读取;
    /// </summary>
    public class LandformInfoXmlSerializer : IReader<List<LandformInfo>>, IReader<Dictionary<int, LandformInfo>>
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(LandformInfo[]));

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

        public virtual List<LandformInfo> Read()
        {
            string filePath = GetDefaultFilePath();

            if (!File.Exists(filePath))
                return new List<LandformInfo>();
            else
                return Read(filePath).ToList();
        }

        protected string GetDefaultFilePath()
        {
            string path = ResourcePath.CombineConfiguration(LandformInfo.ArrayFileName);
            path = Path.ChangeExtension(path, FileExtension);
            return path;
        }

        public LandformInfo[] Read(string filePath)
        {
            var item = (LandformInfo[])serializer.DeserializeXiaGu(filePath);
            return item;
        }

    }


}
