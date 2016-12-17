using System;
using System.Xml.Serialization;
using KouXiaGu.Initialization;
using System.IO;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形地图存档;
    /// </summary>
    [XmlType("Terrain")]
    public struct TerrainArchive
    {

        /// <summary>
        /// 使用的地形地图ID;
        /// </summary>
        [XmlElement("UseMapID")]
        public uint UseMapID { get; set; }

        /// <summary>
        /// 保存为的文件名;
        /// </summary>
        const string ARCHIVED_FILE_NAME = "Terrain.xml";

        static readonly XmlSerializer terrainArchiveSerializer = new XmlSerializer(typeof(TerrainArchive));

        public static XmlSerializer TerrainArchiveSerializer
        {
            get { return terrainArchiveSerializer; }
        }

        /// <summary>
        /// 输出一个模板到存档;
        /// </summary>
        public static void TempletOutput(ArchiveDirectory archive)
        {
            string filePath = GetFilePath(archive);
            TerrainArchive data = new TerrainArchive()
            {
                UseMapID = 000,
            };
            terrainArchiveSerializer.Serialize(filePath, data);
        }

        /// <summary>
        /// 保存当前游戏状态的信息到存档文件;
        /// </summary>
        public static void Save(ArchiveDirectory archive)
        {
            string filePath = GetFilePath(archive);
            TerrainArchive data = Export();
            terrainArchiveSerializer.Serialize(filePath, data);
        }

        /// <summary>
        /// 从存档文件获取到对应信息,并且设置到游戏;
        /// </summary>
        public static TerrainArchive Load(ArchiveDirectory archive)
        {
            string filePath = GetFilePath(archive);
            TerrainArchive data = (TerrainArchive)terrainArchiveSerializer.Deserialize(filePath);
            Import(data);
            return data;
        }

        public static string GetFilePath(ArchiveDirectory archive)
        {
            string filePath = Path.Combine(archive.DirectoryPath, ARCHIVED_FILE_NAME);
            return filePath;
        }


        /// <summary>
        /// 从当前游戏状态返回一个存档格式;
        /// </summary>
        public static TerrainArchive Export()
        {
            TerrainArchive archive = new TerrainArchive()
            {
                UseMapID = GetTerrainMapID(),
            };
            return archive;
        }

        /// <summary>
        /// 从存档 设置游戏当前的状态;
        /// </summary>
        public static void Import(TerrainArchive archive)
        {
            SetTerrainMapID(archive);
        }


        static uint GetTerrainMapID()
        {
            return TerrainController.CurrentMap.ID;
        }

        static void SetTerrainMapID(TerrainArchive archive)
        {
            TerrainMap map = TerrainMap.FindMap(archive.UseMapID);
            TerrainController.CurrentMap = map;
        }

    }

}
