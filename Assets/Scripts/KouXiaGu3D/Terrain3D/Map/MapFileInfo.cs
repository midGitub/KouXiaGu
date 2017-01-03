using System;
using System.Collections.Generic;
using System.IO;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形地图文件;每个路径只存在一个实例;
    /// </summary>
    public class MapFileInfo
    {

        /// <summary>
        /// 预制地图描述文件名;
        /// </summary>
        const string MAP_DESCRIPTION_FILE_NAME = "TerrainMap.xml";

        /// <summary>
        /// 预制地图数据文件;
        /// </summary>
        const string MAP_DATA_FILE_NAME = "Map.MAPP";

        /// <summary>
        /// 保存获取到的地图;
        /// </summary>
        static readonly Dictionary<string, MapFileInfo> activated = new Dictionary<string, MapFileInfo>();


        public static string DescriptionFile(string directoryPath)
        {
            string description = Path.Combine(directoryPath, MAP_DESCRIPTION_FILE_NAME);
            return description;
        }

        public static string MapFile(string directoryPath)
        {
            string map = Path.Combine(directoryPath, MAP_DATA_FILE_NAME);
            return map;
        }


        static void AddActivated(MapFileInfo mapFile)
        {
            activated.Add(mapFile.DirectoryPath, mapFile);
        }


        /// <summary>
        /// 从文件夹读取到;
        /// </summary>
        public static MapFileInfo Load(string directoryPath)
        {
            MapFileInfo mapfile;
            bool exists = Exists(directoryPath);

            if (Exists(directoryPath))
            {
                if (!activated.TryGetValue(directoryPath, out mapfile))
                {
                    string filePath = DescriptionFile(directoryPath);
                    var descr = (MapDescr)MapDescr.Serializer.DeserializeXiaGu(filePath);
                    mapfile = new MapFileInfo(descr, directoryPath);
                    AddActivated(mapfile);
                }
                return mapfile;
            }
            else
            {
                throw new FileNotFoundException("地图文件损坏;");
            }
        }

        /// <summary>
        /// 创建一个新的地图,若已经存在则返回异常;
        /// </summary>
        public static MapFileInfo Create(MapDescr description, string directoryPath)
        {
            if (activated.ContainsKey(directoryPath) || Exists(directoryPath))
                throw new ArgumentException("已经存在地图;");

            Directory.CreateDirectory(directoryPath);

            var mapfile = new MapFileInfo(description, directoryPath);
            mapfile.SaveDescription();
            mapfile.CreateEmptyMap();

            AddActivated(mapfile);

            return mapfile;
        }

        /// <summary>
        /// 该路径是否存在地图;
        /// </summary>
        public static bool Exists(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                return ExistsMap(directoryPath) && ExistsDescription(directoryPath);
            }
            return false;
        }

        public static bool ExistsMap(string directoryPath)
        {
            string map = DescriptionFile(directoryPath);
            return File.Exists(map);
        }

        public static bool ExistsDescription(string directoryPath)
        {
            string description = DescriptionFile(directoryPath);
            return File.Exists(description);
        }

        /// <summary>
        /// 删除地图;
        /// </summary>
        public static void Delete(string directoryPath)
        {
            string description = DescriptionFile(directoryPath);
            string map = MapFile(directoryPath);

            File.Delete(description);
            File.Delete(map);

            if (Exists(directoryPath))
                Debug.LogError("地图删除失败!");

            activated.Remove(directoryPath);
        }



        /// <summary>
        /// 根据ID创建一个新地图到预定义的目录下,若已经存在则返回异常;;
        /// </summary>
        MapFileInfo(MapDescr description, string directoryPath)
        {
            this.description = description;
            this.directoryPath = directoryPath;
        }

        /// <summary>
        /// 地貌记录;
        /// </summary>
        LandformObserver landformObserver;

        /// <summary>
        /// 地图描述信息;
        /// </summary>
        MapDescr description;

        /// <summary>
        /// 地图存放的路径;
        /// </summary>
        readonly string directoryPath;

        /// <summary>
        /// 地图存放的路径;
        /// </summary>
        public string DirectoryPath
        {
            get { return directoryPath; }
        }

        /// <summary>
        /// 地图描述信息;
        /// </summary>
        public MapDescr Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 地图是否已经读取?
        /// </summary>
        public bool IsLoaded
        {
            get { return Map != null; }
        }

        /// <summary>
        /// 地形地图;
        /// </summary>
        public ObservableDictionary<CubicHexCoord, TerrainNode> Map { get; private set; }


        static readonly ObservableDictionary<CubicHexCoord, TerrainNode> emptyMap =
            new ObservableDictionary<CubicHexCoord, TerrainNode>();

        /// <summary>
        /// 创建一个空的地图文件到;
        /// </summary>
        void CreateEmptyMap()
        {
            SaveMap(emptyMap);
        }

        /// <summary>
        /// 读取地图数据到内存;
        /// </summary>
        public void Load()
        {
            if (IsLoaded)
            {
                LoadMap();
                LoadLandformObserver();
            }
            else
            {
                throw new ArgumentException("重复读取地图!");
            }
        }

        void LoadMap()
        {
            string prefabFilePath = Path.Combine(DirectoryPath, MAP_DATA_FILE_NAME);
            Map = LoadMap(prefabFilePath);
        }

        ObservableDictionary<CubicHexCoord, TerrainNode> LoadMap(string filePath)
        {
            return SerializeExtensions.DeserializeProtoBuf<ObservableDictionary<CubicHexCoord, TerrainNode>>(filePath);
        }

        void LoadLandformObserver()
        {
            if (landformObserver == null)
                landformObserver = new LandformObserver();

            landformObserver.Reset(description.landformRecord);
            landformObserver.Subscribe(Map);
        }

        /// <summary>
        /// 重新读取地图数据到内存;
        /// </summary>
        public void Reload()
        {
            Unload();
            Load();
        }

        /// <summary>
        /// 保存地图数据;
        /// </summary>
        public void Save()
        {
            SaveDescription();
            SaveMap();
        }

        public void SaveDescription()
        {
            description.landformRecord = landformObserver.ToLandformRecord().ToArray();

            string filePath = DescriptionFile(DirectoryPath);
            MapDescr.Serializer.SerializeXiaGu(filePath, Description);
        }

        void SaveMap()
        {
            SaveMap(Map);
        }

        void SaveMap(ObservableDictionary<CubicHexCoord, TerrainNode> map)
        {
            string prefabFilePath = MapFile(DirectoryPath);
            SaveMap(map, prefabFilePath);
        }

        void SaveMap(ObservableDictionary<CubicHexCoord, TerrainNode> map, string filePath)
        {
            SerializeExtensions.SerializeProtoBuf(filePath, map);
        }

        /// <summary>
        /// 卸载读取的地图;
        /// </summary>
        public void Unload()
        {
            if (Map != null)
            {
                Map.Clear();
                Map.EndTransmission();
                Map = null;

                landformObserver.Clear();
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TerrainMap))
                return false;
            return ((TerrainMap)obj).DirectoryPath == this.DirectoryPath;
        }

        public override int GetHashCode()
        {
            return this.DirectoryPath.GetHashCode();
        }

        public override string ToString()
        {
            return "[Loaded" + IsLoaded + ";Path:" + directoryPath + "]";
        }


    }

}
