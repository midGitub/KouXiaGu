using System;
using System.Xml.Serialization;
using KouXiaGu.Initialization;
using System.IO;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 对地形具体信息进行归档;
    /// </summary>
    public static class TerrainArchiver
    {

        /// <summary>
        /// 保存为的文件名;
        /// </summary>
        const string ARCHIVED_FILE_NAME = "Terrain.xml";

        /// <summary>
        /// 输出一个模板到存档;
        /// </summary>
        public static void TempletOutput(string archiveDirectory)
        {
            string filePath = Archiver.CreateDirectory(archiveDirectory, ARCHIVED_FILE_NAME);
            ArchiveDescr data = new ArchiveDescr()
            {
                UseMapID = 0,
            };
            ArchiveDescr.Serializer.SerializeXiaGu(filePath, data);
        }


        /// <summary>
        /// 保存当前游戏状态的信息到存档文件;
        /// </summary>
        public static void Save(string archiveDirectory)
        {
            string filePath = Archiver.CreateDirectory(archiveDirectory, ARCHIVED_FILE_NAME);
            ArchiveDescr data = Export();
            ArchiveDescr.Serializer.SerializeXiaGu(filePath, data);
        }

        /// <summary>
        /// 从存档文件获取到对应信息,并且设置到游戏;
        /// </summary>
        public static ArchiveDescr Load(string archiveDirectory)
        {
            string filePath = Archiver.CombineDirectory(archiveDirectory, ARCHIVED_FILE_NAME);
            ArchiveDescr data = (ArchiveDescr)ArchiveDescr.Serializer.DeserializeXiaGu(filePath);
            Import(data);
            return data;
        }

        /// <summary>
        /// 从当前游戏状态返回一个存档格式;
        /// </summary>
        public static ArchiveDescr Export()
        {
            ArchiveDescr archive = new ArchiveDescr()
            {
                UseMapID = GetTerrainMapID(),
            };
            return archive;
        }

        /// <summary>
        /// 从存档 设置游戏当前的状态;
        /// </summary>
        public static void Import(ArchiveDescr archive)
        {
            SetTerrainMapID(archive);
        }


        static int GetTerrainMapID()
        {
            return TerrainInitializer.CurrentMap.Description.id;
        }

        static void SetTerrainMapID(ArchiveDescr archive)
        {
            TerrainMapO map = TerrainMapO.FindMap(archive.UseMapID);
            TerrainInitializer.CurrentMap = map;
        }

    }

}
