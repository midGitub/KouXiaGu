using System.IO;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using System.Collections;
using UnityEngine;
using System;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形地图保存和提供;
    /// </summary>
    public class TerrainMap
    {

        /// <summary>
        /// 是否为允许编辑状态?若为 true 则在保存游戏时,对地图进行保存;
        /// </summary>
        public static bool AllowEdit { get; private set; }

        static TerrainMap()
        {
            AllowEdit = true;
        }


        #region 地图资源管理

        /// <summary>
        /// 保存所有地图的文件路径;
        /// </summary>
        const string MAPS_DIRECTORY_NAME = "TerrainMaps";

        /// <summary>
        /// 预制地图描述文件名;
        /// </summary>
        const string MAP_DESCRIPTION_FILE_NAME = "TerrainMap.xml";

        /// <summary>
        /// 预制地图数据文件;
        /// </summary>
        const string MAP_DATA_FILE_NAME = "Map.MAPP";

        /// <summary>
        /// 预定义的的地图存放路径;
        /// </summary>
        public static string PredefinedDirectory
        {
            get { return Path.Combine(ResourcePath.ConfigurationDirectoryPath, MAPS_DIRECTORY_NAME); }
        }

        /// <summary>
        /// 获取到预定义目录下的所有地图;
        /// </summary>
        public static IEnumerable<TerrainMap> GetMaps()
        {
            return GetMaps(PredefinedDirectory);
        }

        /// <summary>
        /// 获取到目录下一级的所有地图;
        /// </summary>
        public static IEnumerable<TerrainMap> GetMaps(string directoryPath)
        {
            var paths = Directory.GetDirectories(directoryPath);
            TerrainMap data;

            foreach (var mapPath in paths)
            {
                try
                {
                    data = GetMap(mapPath);
                }
                catch (FileNotFoundException)
                {
                    Debug.LogWarning("无法获取到描述文件 ;地图目录 :" + mapPath + " ; 跳过此文件夹;");
                    continue;
                }
                yield return data;
            }
        }

        /// <summary>
        /// 获取到目录下所保存的地图;
        /// </summary>
        public static TerrainMap GetMap(string directoryPath)
        {
            string filePath = GetDescriptionFilePath(directoryPath);
            var data = TerrainMapDescr.Deserialize(filePath);
            TerrainMap map = new TerrainMap(data, directoryPath);
            return map;
        }

        /// <summary>
        /// 寻找这个地图并且返回,若不存在则返回异常 InvalidOperationException;
        /// </summary>
        public static TerrainMap FindMap(int id)
        {
            return FindMap(id, PredefinedDirectory);
        }

        /// <summary>
        /// 寻找这个地图并且返回,若不存在则返回异常 InvalidOperationException;
        /// </summary>
        public static TerrainMap FindMap(int id, string directoryPaths)
        {
            TerrainMap map = GetMaps(directoryPaths).First(tmap => tmap.Description.id == id);
            return map;
        }

        /// <summary>
        /// 获取到目录下的描述文件路径;
        /// </summary>
        static string GetDescriptionFilePath(string directoryPath)
        {
            string filePath = Path.Combine(directoryPath, MAP_DESCRIPTION_FILE_NAME);
            return filePath;
        }

        #endregion


        #region 实例部分;

        TerrainMapDescr description;

        ObservableDictionary<CubicHexCoord, TerrainNode> map;

        LandformObserver landformObserver;

        /// <summary>
        /// 完整的地图存放路径;
        /// </summary>
        public string DirectoryPath { get; private set; }

        public TerrainMapDescr Description
        {
            get { return description; }
            private set { description = value; }
        }

        /// <summary>
        /// 地形地图;
        /// </summary>
        public ObservableDictionary<CubicHexCoord, TerrainNode> Map
        {
            get { return map; }
        }

        /// <summary>
        /// 根据ID创建一个新地图到预定义的目录下,若已经存在则返回异常;;
        /// </summary>
        public TerrainMap(TerrainMapDescr description)
        {
            this.DirectoryPath = Path.Combine(PredefinedDirectory, description.id.ToString());

            if (Directory.Exists(this.DirectoryPath))
                throw new IOException("为此ID的地图已经存在;");

            this.description = description;
            UpdateDescription();
        }

        /// <summary>
        /// 初始化地图信息;
        /// </summary>
        TerrainMap(TerrainMapDescr description, string directoryPath)
        {
            this.description = description;
            this.DirectoryPath = directoryPath;

            if (description.landformRecord != null)
                landformObserver = new LandformObserver(description.landformRecord);
            else
                landformObserver = new LandformObserver();
        }

        /// <summary>
        /// 更新描述文件;
        /// </summary>
        public void UpdateDescription()
        {
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);

            string filePath = GetDescriptionFilePath(DirectoryPath);

            if (landformObserver != null)
                description.landformRecord = landformObserver.ToLandformRecord().ToArray();

            TerrainMapDescr.Serialize(filePath, description);
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

        /// <summary>
        /// 读取地图数据到内存;
        /// </summary>
        public void Load()
        {
            string prefabFilePath = Path.Combine(DirectoryPath, MAP_DATA_FILE_NAME);
            map = LoadMap(prefabFilePath);

            landformObserver.Subscribe(map);
        }

        /// <summary>
        /// 保存地图数据;
        /// </summary>
        public void Save()
        {
            string prefabFilePath = Path.Combine(DirectoryPath, MAP_DATA_FILE_NAME);
            SaveMap(map, prefabFilePath);

            UpdateDescription();
        }

        /// <summary>
        /// 卸载读取的地图;
        /// </summary>
        public void Unload()
        {
            if (map != null)
            {
                map.Clear();
                map.EndTransmission();
                map = null;
            }
        }

        ObservableDictionary<CubicHexCoord, TerrainNode> LoadMap(string filePath)
        {
            return SerializeHelper.DeserializeProtoBuf<ObservableDictionary<CubicHexCoord, TerrainNode>>(filePath);
        }

        void SaveMap(ObservableDictionary<CubicHexCoord, TerrainNode> map, string filePath)
        {
            SerializeHelper.SerializeProtoBuf(filePath, map);
        }

        #endregion

        #region Test

        //[ShowOnlyProperty]
        //public int Count
        //{
        //    get { return Map.Count; }
        //}

        //[ShowOnlyProperty]
        //public int ChunkCount
        //{
        //    get { return (map.BlockedMap as IMap<RectCoord, Dictionary<CubicHexCoord, TerrainNode>>).Count; }
        //}

        //[ContextMenu("烘焙测试")]
        //void Test_Baking()
        //{
        //    TerrainCreater.Create(RectCoord.Self);
        //    TerrainCreater.Create(RectCoord.West);
        //    TerrainCreater.Create(RectCoord.East);
        //    TerrainCreater.Create(RectCoord.North);
        //    TerrainCreater.Create(RectCoord.South);
        //    TerrainCreater.Create(RectCoord.South + RectCoord.West);
        //    TerrainCreater.Create(RectCoord.South + RectCoord.East);
        //    TerrainCreater.Create(RectCoord.North + RectCoord.West);
        //    TerrainCreater.Create(RectCoord.North + RectCoord.East);
        //}

        //[ContextMenu("保存修改的地图")]
        //void SaveMap()
        //{
        //    Save(mapDirectory);
        //}

        //[ContextMenu("保存地图")]
        //void SaveAllMap()
        //{
        //    SaveAll(mapDirectory);
        //}

        //[ContextMenu("读取地图")]
        //void LoadMap()
        //{
        //    Load(mapDirectory);
        //}

        //[ContextMenu("输出所有地图文件")]
        //void ShowAllMapFile()
        //{
        //    var paths = BlockProtoBufExtensions.GetFilePaths(mapDirectory);
        //    Debug.Log(paths.ToLog());
        //}

        ///// <summary>
        ///// 返回一个随机地图;
        ///// </summary>
        //Map<CubicHexCoord, TerrainNode> RandomMap()
        //{
        //    Map<CubicHexCoord, TerrainNode> terrainMap = new Map<CubicHexCoord, TerrainNode>();
        //    int[] aa = new int[] { 10, 20, 30, 20 };

        //    foreach (var item in CubicHexCoord.GetHexRange(CubicHexCoord.Self, 50))
        //    {
        //        try
        //        {
        //            terrainMap.Add(item, new TerrainNode(aa[UnityEngine.Random.Range(0, aa.Length)], UnityEngine.Random.Range(0, 360)));
        //        }
        //        catch (ArgumentException)
        //        {

        //        }
        //    }
        //    return terrainMap;
        //}

        //void Awake()
        //{
        //    if (ConfirmDirectory(mapDirectory))
        //    {
        //        LoadMap();
        //    }
        //    else
        //    {
        //        Debug.Log("随机地图");
        //        terrainMap.Add(RandomMap());
        //    }
        //    Debug.Log("地图准备完毕");
        //}

        #endregion


    }

}
