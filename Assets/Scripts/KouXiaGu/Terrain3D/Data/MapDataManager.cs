using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Initialization;
using System.IO;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地图数据管理;
    /// </summary>
    public static class MapDataManager
    {
        /// <summary>
        /// 预制地图数据文件;
        /// </summary>
        const string DATA_FILE_NAME = "Terrain";

        /// <summary>
        /// 当前游戏的地形数据;
        /// </summary>
        public static MapData ActiveData { get; private set; }

        /// <summary>
        /// 加载地形数据;
        /// </summary>
        public static void Load()
        {
            try
            {
                string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
                ActiveData = MapData.Create(filePath);
            }
            catch (FileNotFoundException e)
            {
                Debug.LogWarning("未找到地形地图数据文件或地图损坏,从新的地图加载游戏" + e);
                ActiveData = MapData.Create();
            }
        }

        /// <summary>
        /// 从存档加载地形数据;
        /// </summary>
        public static void Load(ArchiveFile archive)
        {
            string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
            string archivefilePath = archive.CombineToTerrain(DATA_FILE_NAME);
            ActiveData = MapData.Create(filePath, archivefilePath);
        }

        /// <summary>
        /// 保存地图数据到文件;
        /// </summary>
        public static void Save()
        {
            string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
            MapData.Write(filePath, ActiveData);
        }

        /// <summary>
        /// 保存地图记录到存档;
        /// </summary>
        public static void Save(ArchiveFile archive)
        {
            string archivefilePath = archive.CombineToTerrain(DATA_FILE_NAME);
            MapData.WriteArchive(archivefilePath, ActiveData);
        }

        /// <summary>
        /// 卸载已读取的地形数据;
        /// </summary>
        public static void Unload()
        {
            ActiveData = null;
        }

    }

}
