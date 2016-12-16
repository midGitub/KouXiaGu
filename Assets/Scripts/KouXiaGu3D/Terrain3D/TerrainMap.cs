using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形地图保存和提供;
    /// </summary>
    [XmlType(TypeName = "TerrainMap")]
    public sealed class TerrainMap
    {

        /// <summary>
        /// 是否为允许编辑状态?若为 true 则在保存游戏时,对地图进行保存;
        /// </summary>
        public static bool AllowEdit { get; private set; }

        static TerrainMap()
        {
            AllowEdit = false;
        }

        #region 实例部分;

        /// <summary>
        /// 地图分块大小;
        /// </summary>
        const short MapBlockSize = 600;

        [XmlAttribute("id")]
        public uint ID;

        [XmlElement("Name")]
        public string Name;

        [XmlElement("Time")]
        public long Time;

        [XmlElement("Version")]
        public int Version;

        [XmlElement("Description")]
        public string Description;

        /// <summary>
        /// 地形地图结构;
        /// </summary>
        [XmlIgnore]
        BlockMapRecord<TerrainNode> map;

        /// <summary>
        /// 是否已经读取过了?
        /// </summary>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// 地形地图;
        /// </summary>
        public IMap<CubicHexCoord, TerrainNode> Map
        {
            get { return map; }
        }

        /// <summary>
        /// 完整的地图存放路径;
        /// </summary>
        [XmlIgnore]
        public string DirectoryPath { get; private set; }

        TerrainMap()
        {
            IsLoaded = false;
        }

        /// <summary>
        /// 创建一个新的地形到这个目录之下;
        /// </summary>
        public TerrainMap(string directoryPath) : this()
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string filePath = GetDescriptionFilePath(directoryPath);
            Serialize(filePath, this);

            this.DirectoryPath = directoryPath;
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

        public void Clear()
        {
            map.Clear();
            IsLoaded = false;
        }

        /// <summary>
        /// 若地图为空,则创建一个地图实例;
        /// </summary>
        BlockMapRecord<TerrainNode> CreateMapOrReturn()
        {
            return  map ?? 
                (map = new BlockMapRecord<TerrainNode>((MapBlockSize & 1) == 1 ? MapBlockSize : (short)(MapBlockSize + 1)));
        }

        /// <summary>
        /// 保存上次保存之后修改过的地图;
        /// </summary>
        public IEnumerator SaveAsync()
        {
            if (AllowEdit)
            {
                return map.SaveAsync(DirectoryPath, FileMode.Create);
            }
            else
            {
                return DontSaveRetrun();
            }
        }

        /// <summary>
        /// 保存所有地图;
        /// </summary>
        public IEnumerator SaveAllAsync()
        {
            if (AllowEdit)
            {
                return map.SaveAllAsync(DirectoryPath, FileMode.Create);
            }
            else
            {
                return DontSaveRetrun();
            }
        }

        /// <summary>
        /// 不保存地图并且返回一个空的迭代结构;
        /// </summary>
        static IEnumerator DontSaveRetrun()
        {
            Debug.Log("跳过地形地图保存;");
            return EmptyEnumerator.GetInstance;
        }

        /// <summary>
        /// 读取地图 或 若已经读取过了,则为重新加载地图;
        /// </summary>
        public IEnumerator LoadAsync()
        {
            var map = CreateMapOrReturn();
            return map.LoadAsync(DirectoryPath);
        }

        #endregion


        #region 地图资源管理

        /// <summary>
        /// 保存所有地图的文件路径;
        /// </summary>
        const string MAPS_DIRECTORY_NAME = "TerrainMaps";

        /// <summary>
        /// 描述文件名;
        /// </summary>
        public const string MAP_DESCRIPTION_FILE_NAME = "TerrainMap.xml";

        static readonly XmlSerializer terrainMapInfoSerializer = new XmlSerializer(typeof(TerrainMap));

        public static XmlSerializer TerrainMapInfoSerializer
        {
            get { return terrainMapInfoSerializer; }
        }

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
            TerrainMap data = Deserialize(filePath);
            data.DirectoryPath = directoryPath;
            return data;
        }

        /// <summary>
        /// 寻找这个地图并且返回,若不存在则返回异常 InvalidOperationException;
        /// </summary>
        public static TerrainMap FindMap(uint id)
        {
            return FindMap(id, PredefinedDirectory);
        }

        /// <summary>
        /// 寻找这个地图并且返回,若不存在则返回异常 InvalidOperationException;
        /// </summary>
        public static TerrainMap FindMap(uint id, string directoryPaths)
        {
            TerrainMap map = GetMaps(directoryPaths).First(tmap => tmap.ID == id);
            return map;
        }

        /// <summary>
        /// 确认这个文件夹下是否存在地图文件;
        /// </summary>
        public static bool ConfirmDirectory(string directoryPath)
        {
            var paths = BlockProtoBufExtensions.GetFilePaths(directoryPath);
            string path = paths.FirstOrDefault();
            return path != null;
        }

        /// <summary>
        /// 获取到目录下的描述文件路径;
        /// </summary>
        static string GetDescriptionFilePath(string directoryPath)
        {
            string filePath = Path.Combine(directoryPath, MAP_DESCRIPTION_FILE_NAME);
            return filePath;
        }

        static void Serialize(string filePath, TerrainMap data)
        {
            TerrainMapInfoSerializer.Serialize(filePath, data);
        }

        static TerrainMap Deserialize(string filePath)
        {
            TerrainMap data = (TerrainMap)TerrainMapInfoSerializer.Deserialize(filePath);
            return data;
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
