using System;
using System.IO;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using KouXiaGu.Initialization;
using System.Collections.Generic;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 负责对地形地图归档,并且记录地图信息;
    /// </summary>
    public static class MapArchiver
    {

        static MapArchiver()
        {
            IsSubscribe = false;
        }


        /// <summary>
        /// 存档的地图数据文件;
        /// </summary>
        const string MAP_ARCHIVED_FILE_NAME = "TerrainMap.mapp";

        /// <summary>
        /// 记录当前地图发生变化的内容;
        /// </summary>
        public static DictionaryArchiver<CubicHexCoord, TerrainNode> ArchiveMap { get; private set; }

        public static bool IsSubscribe { get; private set; }


        /// <summary>
        /// 开始检视地图变化;
        /// </summary>
        public static void Initialize(Archive archive, TerrainMap map)
        {
            if (map.IsMapLoaded)
                throw new ArgumentNullException("地图还未初始化");

            ObservableDictionary<CubicHexCoord, TerrainNode> observableMap = map.Map;

            ReadArchiveMap(archive);
            Combine(observableMap, ArchiveMap);
            ArchiveMap.Subscribe(observableMap);

            IsSubscribe = true;
        }

        /// <summary>
        /// 停止检视;
        /// </summary>
        public static void Unsubscribe()
        {
            if (IsSubscribe)
            {
                ArchiveMap.Unsubscribe();
                ArchiveMap.Clear();
                ArchiveMap = null;

                IsSubscribe = false;
            }
        }

        /// <summary>
        /// 清除所有记录;
        /// </summary>
        public static void Clear()
        {
            ArchiveMap.Clear();
        }


        /// <summary>
        /// 读取到存档内的地图;
        /// </summary>
        static void ReadArchiveMap(Archive archive)
        {
            string filePath = archive.CombineToTerrain(MAP_ARCHIVED_FILE_NAME);

            if (File.Exists(filePath))
            {
                ArchiveMap = ReadArchiveMap(filePath);
            }
            else
            {
                ArchiveMap = new DictionaryArchiver<CubicHexCoord, TerrainNode>();
            }
        }

        static DictionaryArchiver<CubicHexCoord, TerrainNode> ReadArchiveMap(string filePath)
        {
            return ProtoBufExtensions.DeserializeProtoBuf<DictionaryArchiver<CubicHexCoord, TerrainNode>>(filePath);
        }

        static void Combine(IDictionary<CubicHexCoord, TerrainNode> map, DictionaryArchiver<CubicHexCoord, TerrainNode> archiveMap)
        {
            map.AddOrUpdate(archiveMap);
        }


        /// <summary>
        /// 将存档输出;
        /// </summary>
        public static void Write(Archive archive)
        {
            string filePath = archive.CombineToTerrain(MAP_ARCHIVED_FILE_NAME);
            WriteArchiveMap(filePath, ArchiveMap);
        }

        static void WriteArchiveMap(string filePath, DictionaryArchiver<CubicHexCoord, TerrainNode> map)
        {
            ProtoBufExtensions.SerializeProtoBuf(filePath, map);
        }

    }

}
