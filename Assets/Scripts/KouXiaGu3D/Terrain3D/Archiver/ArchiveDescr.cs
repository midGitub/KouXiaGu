using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 归档描述;
    /// </summary>
    [XmlType("Terrain")]
    public struct ArchiveDescr
    {
        /// <summary>
        /// 使用的地形地图ID;
        /// </summary>
        [XmlElement("UseMapID")]
        public int UseMapID { get; set; }




        static readonly XmlSerializer serializer = new XmlSerializer(typeof(ArchiveDescr));

        public static XmlSerializer Serializer
        {
            get { return serializer; }
        }

    }

}
