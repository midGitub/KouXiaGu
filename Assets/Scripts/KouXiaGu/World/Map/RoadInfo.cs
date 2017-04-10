using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 道路信息;
    /// </summary>
    [XmlType("Road")]
    public struct RoadInfo
    {
        internal const string ArrayFileName = "World/Road";

        [XmlAttribute("id")]
        public int ID;

        [XmlAttribute("name")]
        public string Name;

        [XmlElement("Terrain")]
        public TerrainRoadInfo Terrain;
    }


    /// <summary>
    /// 道路信息读取;
    /// </summary>
    public class RoadInfoXmlReader : IReader<List<RoadInfo>>, IReader<Dictionary<int, RoadInfo>>
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(RoadInfo[]));

        protected string FileExtension
        {
            get { return ".xml"; }
        }

        Dictionary<int, RoadInfo> IReader<Dictionary<int, RoadInfo>>.Read()
        {
            var infoArray = Read();
            var infoDictionary = infoArray.ToDictionary(item => item.ID);
            return infoDictionary;
        }

        public virtual List<RoadInfo> Read()
        {
            string filePath = GetDefaultFilePath();

            if (!File.Exists(filePath))
                return new List<RoadInfo>();
            else
                return Read(filePath).ToList();
        }

        protected string GetDefaultFilePath()
        {
            string path = ResourcePath.CombineConfiguration(RoadInfo.ArrayFileName);
            path = Path.ChangeExtension(path, FileExtension);
            return path;
        }

        protected RoadInfo[] Read(string filePath)
        {
            var item = (RoadInfo[])serializer.DeserializeXiaGu(filePath);
            return item;
        }

    }

}
