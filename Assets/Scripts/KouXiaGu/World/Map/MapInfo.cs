using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.World.Map
{

    public class MapInfoReader
    {
        public MapInfoReader()
        {
            serializer = new XmlSerializer(typeof(MapInfo));
        }

        XmlSerializer serializer;

        public string FileExtension
        {
            get { return ".xml"; }
        }

        public MapInfo Read(string filePath)
        {
            return (MapInfo)serializer.DeserializeXiaGu(filePath);
        }

        public void Write(string filePath, MapInfo data)
        {
            serializer.SerializeXiaGu(filePath, data);
        }
    }

    [XmlType("MapInfo")]
    public struct MapInfo
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }
    }

}
