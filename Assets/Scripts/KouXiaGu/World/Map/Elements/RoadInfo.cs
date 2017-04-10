using System;
using System.Collections.Generic;
using System.Linq;
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
        [XmlAttribute("id")]
        public int ID;

        [XmlAttribute("name")]
        public string Name;

        [XmlElement("Terrain")]
        public TerrainRoadInfo Terrain;
    }

    class RoadInfoFile : CustomFile
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
    public class RoadInfoXmlReader : IReader<List<RoadInfo>>, IReader<Dictionary<int, RoadInfo>>
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(RoadInfo[]));
        static readonly RoadInfoFile file = new RoadInfoFile();

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
            List<RoadInfo> item = new List<RoadInfo>();

            foreach (var path in file.GetFilePaths())
            {
                string newPath = Path.ChangeExtension(path, FileExtension);
                if (File.Exists(newPath))
                {
                    var array = Read(newPath);
                    item.AddRange(array);
                }
            }

            return item;
        }

        protected RoadInfo[] Read(string filePath)
        {
            var item = (RoadInfo[])serializer.DeserializeXiaGu(filePath);
            return item;
        }

    }

}
