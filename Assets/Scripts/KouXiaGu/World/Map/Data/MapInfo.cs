using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.World.Map
{
    
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
