using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using KouXiaGu.Terrain3D;
using UnityEngine;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 道路信息;
    /// </summary>
    [XmlType("RoadInfo")]
    public struct RoadInfo
    {
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
    public class XmlRoadInfoReader : IReader<RoadInfo[]>
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(RoadInfo[]));
        internal const string InfosFileName = "World/Road";

        protected string FileExtension
        {
            get { return ".xml"; }
        }

        public virtual RoadInfo[] Read()
        {
            string filePath = GetFilePath(InfosFileName);
            return ReadOrDefault(filePath);
        }

        /// <summary>
        /// 若不存在此文件,则返回空的数组;
        /// </summary>
        protected RoadInfo[] ReadOrDefault(string filePath)
        {
            if (!File.Exists(filePath))
                return new RoadInfo[0];

            var item = (RoadInfo[])serializer.DeserializeXiaGu(filePath);
            return item;
        }

        protected string GetFilePath(string fileName)
        {
            string path = ResourcePath.CombineConfiguration(fileName);
            path = Path.ChangeExtension(path, FileExtension);
            return path;
        }
    }

}
