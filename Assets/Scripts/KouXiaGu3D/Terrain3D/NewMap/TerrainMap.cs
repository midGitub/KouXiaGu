using System.Collections.Generic;
using System.IO;
using KouXiaGu.Collections;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形地图;
    /// </summary>
    public class TerrainMap
    {

        /// <summary>
        /// 预制地图描述文件名;
        /// </summary>
        const string DESCRIPTION_FILE_NAME = "TerrainMap.xml";

        /// <summary>
        /// 预制地图数据文件;
        /// </summary>
        const string DATA_FILE_NAME = "Map.MAPP";

        static readonly ObservableDictionary<CubicHexCoord, TerrainNode> emptyMap = 
            new ObservableDictionary<CubicHexCoord, TerrainNode>();


        /// <summary>
        /// 创建一个地图内容到目录下;
        /// </summary>
        public static TerrainMap Create(string directory, MapDescription description)
        {
            Directory.CreateDirectory(directory);
            TerrainMap map = new TerrainMap(directory, description);

            WriteDescription(directory, description);
            WriteEmptyMap(directory);

            return map;
        }

        /// <summary>
        /// 创建描述文件到目录下;
        /// </summary>
        static void WriteDescription(string directory, MapDescription description)
        {
            Directory.CreateDirectory(directory);
            string descriptionFilePath = GetDescriptionFilePath(directory);
            MapDescription.Serializer.SerializeXiaGu(descriptionFilePath, description);
        }

        static void WriteEmptyMap(string directory)
        {
            WriteMap(directory, emptyMap);
        }

        static void WriteMap(string directory, ObservableDictionary<CubicHexCoord, TerrainNode> map)
        {
            string mapFilePath = GetMapDataFilePath(directory);
            ProtoBufExtensions.SerializeProtoBuf(mapFilePath, map);
        }


        /// <summary>
        /// 获取到目录下的描述文件;
        /// </summary>
        static string GetDescriptionFilePath(string directory)
        {
            return Path.Combine(directory, DESCRIPTION_FILE_NAME);
        }

        /// <summary>
        /// 获取到目录下地图数据文件;
        /// </summary>
        static string GetMapDataFilePath(string directory)
        {
            return Path.Combine(directory, DATA_FILE_NAME);
        }


        /// <summary>
        /// 获取到这些目录中为描述文件;
        /// </summary>
        public static IEnumerable<TerrainMap> Find(IEnumerable<string> directorys)
        {
            TerrainMap item;
            foreach (var directory in directorys)
            {
                if (TryLoad(directory, out item))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// 尝试读取此路径下的描述文件;
        /// </summary>
        public static bool TryLoad(string directory, out TerrainMap item)
        {
            if (Exists(directory))
            {
                item = Load(directory);
                return true;
            }

            item = default(TerrainMap);
            return false;
        }

        /// <summary>
        /// 确认此目录是否为存在描述文件;
        /// </summary>
        public static bool Exists(string directory)
        {
            string descriptionFilePath = GetDescriptionFilePath(directory);
            string dataFilePath = GetMapDataFilePath(directory);

            return File.Exists(descriptionFilePath) && File.Exists(dataFilePath);
        }


        public static TerrainMap Load(string directory)
        {
            MapDescription description = ReadDescription(directory);
            return new TerrainMap(directory, description);
        }

        /// <summary>
        /// 从目录下读取描述文件;
        /// </summary>
        static MapDescription ReadDescription(string directory)
        {
            string descriptionFilePath = GetDescriptionFilePath(directory);
            MapDescription description = (MapDescription)MapDescription.Serializer.DeserializeXiaGu(descriptionFilePath);
            return description;
        }

        /// <summary>
        /// 从目录下读取地图文件;
        /// </summary>
        static ObservableDictionary<CubicHexCoord, TerrainNode> ReadMap(string directory)
        {
            string mapFilePath = GetMapDataFilePath(directory);
            return ProtoBufExtensions.DeserializeProtoBuf<ObservableDictionary<CubicHexCoord, TerrainNode>>(mapFilePath);
        }



        TerrainMap(string directoryPath, MapDescription description)
        {
            this.DirectoryPath = directoryPath;
            this.Description = description;
        }

        public string DirectoryPath { get; private set; }

        public MapDescription Description { get; private set; }

        public ObservableDictionary<CubicHexCoord, TerrainNode> Map { get; private set; }

        /// <summary>
        /// 地图是否加载?
        /// </summary>
        public bool IsMapLoaded
        {
            get { return Map != null; }
        }

        /// <summary>
        /// 更新描述文件;
        /// </summary>
        public void UpdateDescription(MapDescription description)
        {
            WriteDescription(DirectoryPath, description);
            this.Description = description;
        }

        /// <summary>
        /// 读取地图文件;
        /// </summary>
        public void ReadMap()
        {
            Map = ReadMap(DirectoryPath);
        }

        /// <summary>
        /// 输出地图文件;
        /// </summary>
        public void WriteMap()
        {
            WriteMap(DirectoryPath, Map);
        }

        /// <summary>
        /// 清除地图数据;
        /// </summary>
        public void CreateMap()
        {
            Map.Clear();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TerrainMap))
                return false;
            return ((TerrainMap)obj).DirectoryPath == DirectoryPath;
        }

        public override int GetHashCode()
        {
            return DirectoryPath.GetHashCode();
        }

    }

}
