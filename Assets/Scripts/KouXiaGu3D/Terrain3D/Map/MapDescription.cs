using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{

    [XmlType("TerrainMap")]
    public struct MapDescription
    {
        const string FILE_EXTENSION = ".xml";

        static readonly XmlSerializer serializer = new XmlSerializer(typeof(MapDescription));

        public static XmlSerializer Serializer
        {
            get { return serializer; }
        }

        /// <summary>
        /// 输出到文件;
        /// </summary>
        public static void Write(string filePath, MapDescription description)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            Serializer.SerializeXiaGu(filePath, description);
        }

        /// <summary>
        /// 从文件读取到;
        /// </summary>
        public static MapDescription Read(string filePath)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            MapDescription description = (MapDescription)Serializer.DeserializeXiaGu(filePath);
            return description;
        }



        [XmlAttribute("id")]
        public string Id;

        [XmlAttribute("Version")]
        public string Version;


        [XmlElement("Name")]
        public string Name;

        [XmlElement("SaveTime")]
        public long SaveTime;

        [XmlElement("Description")]
        public string Summary;

    }

}
