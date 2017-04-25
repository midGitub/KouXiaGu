//using System;
//using System.Collections.Generic;
//using System.Linq;
//using KouXiaGu.Initialization;
//using System.IO;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 地图数据管理;
//    /// </summary>
//    public static class MapDataManager
//    {

//        static MapDataManager()
//        {
//            Filer = new ProtoMapFiler();
//        }

//        public static MapFiler Filer { get; private set; }

//        /// <summary>
//        /// 预制地图数据文件;
//        /// </summary>
//        const string DATA_FILE_NAME = "Terrain";

//        /// <summary>
//        /// 当前游戏的地形数据;
//        /// </summary>
//        public static MapData Data { get; private set; }

//        /// <summary>
//        /// 加载地形数据;
//        /// </summary>
//        public static void Load()
//        {
//            try
//            {
//                string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
//                Data = Filer.Read(filePath);
//            }
//            catch (FileNotFoundException e)
//            {
//                Debug.LogWarning("未找到地形地图数据文件或地图损坏,从新的地图加载游戏" + e);
//                //ActiveData = Filer.Read();
//            }
//        }

//        /// <summary>
//        /// 从存档加载地形数据;
//        /// </summary>
//        public static void Load(ArchiveFile archive)
//        {
//            string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
//            string archivefilePath = archive.CombineToTerrain(DATA_FILE_NAME);
//            //Data = MapData.Create(filePath, archivefilePath);
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// 保存地图数据到文件;
//        /// </summary>
//        public static void Save()
//        {
//            string filePath = TerrainFiler.Combine(DATA_FILE_NAME);
//            Filer.Write(filePath, Data);
//        }

//        /// <summary>
//        /// 保存地图记录到存档;
//        /// </summary>
//        public static void Save(ArchiveFile archive)
//        {
//            string archivefilePath = archive.CombineToTerrain(DATA_FILE_NAME);
//            //MapData.WriteArchive(archivefilePath, Data);
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// 卸载已读取的地形数据;
//        /// </summary>
//        public static void Unload()
//        {
//            Data = null;
//        }

//    }

//}
