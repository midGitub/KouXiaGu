using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 对地形地图的描述;
    /// </summary>
    [Serializable, XmlType("TerrainMap")]
    public struct TerrainMapDescr
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

        static readonly XmlSerializer TerrainMapInfoSerializer = new XmlSerializer(typeof(TerrainMapDescr));

        public static void Serialize(string filePath, TerrainMapDescr data)
        {
            TerrainMapInfoSerializer.SerializeFile(filePath, data);
        }

        public static TerrainMapDescr Deserialize(string filePath)
        {
            TerrainMapDescr data = (TerrainMapDescr)TerrainMapInfoSerializer.DeserializeFile(filePath);
            return data;
        }

    }


}
