using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using System.Collections;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形地图保存和提供;
    /// </summary>
    [XmlType(TypeName = "TerrainMap")]
    public sealed class TerrainMap
    {

        #region 实例部分;

        /// <summary>
        /// 地图分块大小;
        /// </summary>
        const short MapBlockSize = 600;

        [XmlAttribute("name")]
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
        /// 地图路径;
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
            return map.SaveAsync(DirectoryPath, FileMode.Create);
        }

        /// <summary>
        /// 保存所有地图;
        /// </summary>
        public IEnumerator SaveAllAsync()
        {
            return map.SaveAllAsync(DirectoryPath, FileMode.Create);
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
        const string MAPS_DIRECTORY_NAME = "Maps";

        /// <summary>
        /// 描述文件名;
        /// </summary>
        public const string MAP_DESCRIPTION_FILE_NAME = "Description.xml";

        static readonly XmlSerializer terrainMapInfoSerializer = new XmlSerializer(typeof(TerrainMap));

        public static XmlSerializer TerrainMapInfoSerializer
        {
            get { return terrainMapInfoSerializer; }
        }

        /// <summary>
        /// 获取到目录下的所有地图文件;
        /// </summary>
        public static IEnumerable<TerrainMap> GetMaps(string directoryPath)
        {
            var paths = Directory.GetDirectories(directoryPath);
            TerrainMap data;

            foreach (var mapPath in paths)
            {
                string filePath = GetDescriptionFilePath(mapPath);
                if (TryDeserialize(filePath, out data))
                {
                    data.DirectoryPath = mapPath;
                    yield return data;
                }
            }
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

        static bool TryDeserialize(string filePath, out TerrainMap data)
        {
            try
            {
                data = Deserialize(filePath);
                return true;
            }
            catch
            {
                data = default(TerrainMap);
                return false;
            }
        }

        #endregion


        #region 游戏地图

        /// <summary>
        /// 
        /// </summary>
        public static TerrainMap Current;

        /// <summary>
        /// 当前游戏使用的地图;
        /// </summary>
        public static IMap<CubicHexCoord, TerrainNode> ActivatedMap
        {
            get { return Current.map; }
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
