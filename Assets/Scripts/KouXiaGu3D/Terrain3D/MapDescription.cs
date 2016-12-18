using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{


    [Serializable, XmlType("TerrainMap")]
    public struct MapDescription
    {

        [XmlElement("id")]
        public int id;

        [XmlElement("Name")]
        public string name;

        [XmlElement("Time")]
        public long time;

        [XmlElement("Version")]
        public int version;

        [XmlElement("Description")]
        public string description;

        static readonly XmlSerializer TerrainMapInfoSerializer = new XmlSerializer(typeof(MapDescription));

        public static void Serialize(string filePath, MapDescription data)
        {
            TerrainMapInfoSerializer.SerializeFile(filePath, data);
        }

        public static MapDescription Deserialize(string filePath)
        {
            MapDescription data = (MapDescription)TerrainMapInfoSerializer.DeserializeFile(filePath);
            return data;
        }

    }


}
