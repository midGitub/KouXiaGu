using System.IO;
using KouXiaGu.Collections;
using KouXiaGu.Initialization;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地图文件管理;
    /// </summary>
    public static class MapDataFiler
    {

        /// <summary>
        /// 预制地图数据文件;
        /// </summary>
        const string DATA_FILE_NAME = "Terrain";

        /// <summary>
        /// 地形地图;
        /// </summary>
        public static MapData Map { get; private set; }

        /// <summary>
        /// 记录当前地图发生变化的内容;
        /// </summary>
        public static ArchiveMapData ArchiveMap { get; private set; }


        /// <summary>
        /// 读取地图;
        /// </summary>
        public static void Read()
        {
            ReadMap();
            CreateArchiveMap();
        }

        static void ReadMap()
        {
            string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
            Map = MapData.Read(filePath);
        }

        static void CreateArchiveMap()
        {
            ArchiveMap = new ArchiveMapData();
            ArchiveMap.Subscribe(Map);
        }

        /// <summary>
        /// 从存档读取到地图;
        /// </summary>
        public static void Read(ArchiveFile archive)
        {
            ReadMap();

            try
            {
                string archiveMapFile = archive.CombineToTerrain(DATA_FILE_NAME);
                ArchiveMap = ArchiveMapData.Read(archiveMapFile);
                Map.AddOrUpdate(ArchiveMap);
                ArchiveMap.Subscribe(Map);
            }
            catch (FileNotFoundException)
            {
                CreateArchiveMap();
            }
        }


        /// <summary>
        /// 保存地图,并且重置存档地图;
        /// </summary>
        public static void WriteAndReset()
        {
            Write();
            ArchiveMap.Clear();
        }

        /// <summary>
        /// 保存地图,不重置存档地图;
        /// </summary>
        public static void Write()
        {
            string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
            MapData.Write(filePath, Map);
        }

        /// <summary>
        /// 将变化的地图输出到存档;
        /// </summary>
        public static void Write(ArchiveFile archive)
        {
            string filePath = archive.CombineToTerrain(DATA_FILE_NAME);
            ArchiveMapData.Write(filePath, ArchiveMap);
        }


        public static void Clear()
        {
            if (Map != null)
            {
                Map.EndTransmission();
                Map = null;
            }

            ArchiveMap = null;
        }

    }

}
