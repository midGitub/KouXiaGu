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
        static readonly List<TerrainMap> maps = new List<TerrainMap>();

        /// <summary>
        /// 当地图内容更新时调用;
        /// </summary>
        public static event Action OnMapUpdate;

        /// <summary>
        /// 是否初始化完毕?
        /// </summary>
        public static bool IsInitialized { get; private set; }

        public static List<TerrainMap> ReadOnlyMaps
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
        public static void AddMaps(IEnumerable<TerrainMap> maps)
        {
            MapFiler.maps.AddRange(maps);

            if (OnMapUpdate != null)
                OnMapUpdate();
        }

        /// <summary>
        /// 加入新的地图;
        /// </summary>
        public static void AddMap(TerrainMap map)
        {
            MapFiler.maps.Add(map);

            if (OnMapUpdate != null)
                OnMapUpdate();
        }


        /// <summary>
        /// 创建一个新地图;
        /// </summary>
        public static TerrainMap CreateNewMap(MapDescription description)
        {
            string directory = RandomMapDirectory();
            TerrainMap map = TerrainMap.Create(directory, description);
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
        public static IEnumerable<TerrainMap> FindAll()
        {
            string[] paths = Directory.GetDirectories(MapDirectory);
            return TerrainMap.Find(paths);
        }

        /// <summary>
        /// 获取到Id相同的地图,若不存在则返回异常;
        /// </summary>
        public static TerrainMap Find(string id)
        {
            return FindAll().First(item => item.Description.Id == id);
        }

    }

}
