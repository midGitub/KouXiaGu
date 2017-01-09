using System.Xml.Serialization;
using KouXiaGu.Initialization;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 归档描述;
    /// </summary>
    [XmlType("Terrain")]
    public struct ArchiveDescription
    {
        /// <summary>
        /// 保存为的文件名;
        /// </summary>
        const string ARCHIVED_FILE_NAME = "Terrain.xml";

        static readonly XmlSerializer serializer = new XmlSerializer(typeof(ArchiveDescription));


        public static XmlSerializer Serializer
        {
            get { return serializer; }
        }

        /// <summary>
        /// 保存当前游戏状态的信息到存档文件;
        /// </summary>
        public static void Write(Archive archive, ArchiveDescription data)
        {
            string filePath = archive.CombineToTerrain(ARCHIVED_FILE_NAME);
            Serializer.SerializeXiaGu(filePath, data);
        }

        /// <summary>
        /// 从存档文件获取到对应信息;
        /// </summary>
        public static ArchiveDescription Read(Archive archive)
        {
            string filePath = archive.CombineToTerrain(ARCHIVED_FILE_NAME);
            ArchiveDescription data = (ArchiveDescription)ArchiveDescription.Serializer.DeserializeXiaGu(filePath);
            return data;
        }


        /// <summary>
        /// 使用的地形地图ID;
        /// </summary>
        [XmlElement("UseMapID")]
        public string UseMapID { get; set; }

    }

}
