using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地图管理;
    /// </summary>
    public static class MapFiler
    {

        static MapFiler()
        {
            IsInitialized = false;
        }

        /// <summary>
        /// 保存所有地图的文件路径;
        /// </summary>
        const string MAPS_DIRECTORY_NAME = "Maps";

        /// <summary>
        /// 预定义的的地图存放路径;
        /// </summary>
        public static string MapDirectory
        {
            get { return TerrainResPath.Combine(MAPS_DIRECTORY_NAME); }
        }

        /// <summary>
        /// 地形地图合集;
        /// </summary>
        static readonly List<TerrainMapFile> maps = new List<TerrainMapFile>();

        /// <summary>
        /// 当地图内容更新时调用;
        /// </summary>
        public static event Action OnMapUpdate;

        /// <summary>
        /// 是否初始化完毕?
        /// </summary>
        public static bool IsInitialized { get; private set; }

        public static List<TerrainMapFile> ReadOnlyMaps
        {
            get { return maps; }
        }

        /// <summary>
        /// 初始化;
        /// </summary>
        public static void Initialize()
        {
            maps.AddRange(FindAll());

            IsInitialized = true;
        }

        /// <summary>
        /// 加入新的地图;
        /// </summary>
        public static void AddMaps(IEnumerable<TerrainMapFile> maps)
        {
            MapFiler.maps.AddRange(maps);

            if (OnMapUpdate != null)
                OnMapUpdate();
        }

        /// <summary>
        /// 加入新的地图;
        /// </summary>
        public static void AddMap(TerrainMapFile map)
        {
            MapFiler.maps.Add(map);

            if (OnMapUpdate != null)
                OnMapUpdate();
        }


        /// <summary>
        /// 创建一个新地图;
        /// </summary>
        public static TerrainMapFile CreateNewMap(MapDescription description)
        {
            string directory = RandomMapDirectory();
            TerrainMapFile map = TerrainMapFile.Create(directory, description);
            AddMap(map);
            return map;
        }

        static string RandomMapDirectory()
        {
            string directory = Path.Combine(MapDirectory, Path.GetRandomFileName());
            return directory;
        }

        /// <summary>
        /// 获取到所有地图;
        /// </summary>
        public static IEnumerable<TerrainMapFile> FindAll()
        {
            string[] paths = Directory.GetDirectories(MapDirectory);
            return Find(paths);
        }

        /// <summary>
        /// 获取到这些目录中为描述文件;
        /// </summary>
        public static IEnumerable<TerrainMapFile> Find(IEnumerable<string> directorys)
        {
            TerrainMapFile item;
            foreach (var directory in directorys)
            {
                if (TerrainMapFile.TryRead(directory, out item))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// 获取到Id相同的地图,若不存在则返回异常;
        /// </summary>
        public static TerrainMapFile Find(string id)
        {
            return FindAll().First(item => item.Description.Id == id);
        }

    }

}
