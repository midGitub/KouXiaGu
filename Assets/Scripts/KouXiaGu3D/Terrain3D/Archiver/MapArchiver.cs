using System;
using System.IO;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 负责对地形地图归档,并且记录地图信息;
    /// </summary>
    public static class MapArchiver
    {

        /// <summary>
        /// 存档的地图数据文件;
        /// </summary>
        const string MAP_ARCHIVED_FILE_NAME = "TerrainMap.MAPP";

        /// <summary>
        /// 记录当前地图发生变化的内容;
        /// </summary>
        public static DictionaryArchiver<CubicHexCoord, TerrainNode> ArchiveMap { get; private set; }

        /// <summary>
        /// 对地图进行存档,若存档地图为空,则返回异常;
        /// </summary>
        public static void SaveMap(string archiveDirectory)
        {
            if (ArchiveMap == null)
                throw new ArgumentNullException("存档地图未初始化");

            string filePath = Archiver.CreateDirectory(archiveDirectory, MAP_ARCHIVED_FILE_NAME);

            SerializeHelper.SerializeProtoBuf(filePath, ArchiveMap);
        }

        /// <summary>
        /// 读取地图;
        /// 若地图则创建一个空地图;
        /// </summary>
        public static void LoadMap(string archiveDirectory)
        {
            string filePath = Archiver.CombineDirectory(archiveDirectory, MAP_ARCHIVED_FILE_NAME);

            if (File.Exists(filePath))
            {
                MapArchiver.ArchiveMap = SerializeHelper.DeserializeProtoBuf<DictionaryArchiver<CubicHexCoord, TerrainNode>>(filePath);
            }
            else
            {
                MapArchiver.ArchiveMap = new DictionaryArchiver<CubicHexCoord, TerrainNode>();
            }
        }

        /// <summary>
        /// 监视到地形地图;
        /// </summary>
        public static void Subscribe(TerrainMap terrainMap)
        {
            if (MapArchiver.ArchiveMap == null)
                throw new NullReferenceException("地形地图还未初始化;");

            terrainMap.Map.AddOrUpdate(MapArchiver.ArchiveMap);
            MapArchiver.ArchiveMap.Subscribe(terrainMap.Map);
        }

        /// <summary>
        /// 清空数据;
        /// </summary>
        public static void UnLoad()
        {
            if (ArchiveMap != null)
            {
                ArchiveMap.Unsubscribe();
                ArchiveMap.Clear();
                ArchiveMap = null;
            }
        }

    }

}
