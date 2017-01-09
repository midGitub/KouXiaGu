using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{

    [XmlType("TerrainMap")]
    public struct MapDescription
    {

        static readonly XmlSerializer serializer = new XmlSerializer(typeof(MapDescription));

        public static XmlSerializer Serializer
        {
            get { return serializer; }
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
        public string Description;



    }

}
