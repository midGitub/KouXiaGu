using System;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 对地形地图的描述;
    /// </summary>
    [Serializable, XmlType("TerrainMap")]
    public struct MapDescr
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

        [XmlArray("Landforms")]
        public LandformRecord[] landformRecord;

        static readonly XmlSerializer serializer = new XmlSerializer(typeof(MapDescr));

        public static XmlSerializer Serializer
        {
            get { return serializer; }
        }

        [Obsolete]
        public static void Serialize(string filePath, MapDescr data)
        {
            serializer.SerializeXiaGu(filePath, data);
        }

        [Obsolete]
        public static MapDescr Deserialize(string filePath)
        {
            MapDescr data = (MapDescr)serializer.DeserializeXiaGu(filePath);
            return data;
        }

    }

}
