using System;
using System.IO;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形地图;
    /// </summary>
    public class TerrainMapFile
    {

        /// <summary>
        /// 预制地图描述文件名;
        /// </summary>
        const string DESCRIPTION_FILE_NAME = "Description";

        /// <summary>
        /// 预制地图数据文件;
        /// </summary>
        const string DATA_FILE_NAME = "TerrainMap";


        /// <summary>
        /// 创建一个地图内容到目录下;
        /// </summary>
        public static TerrainMapFile Create(string directory, MapDescription description)
        {
            Directory.CreateDirectory(directory);
            TerrainMapFile map = new TerrainMapFile(directory, description);

            Directory.CreateDirectory(directory);
            WriteDescription(directory, description);
            WriteEmptyMap(directory);

            return map;
        }

        /// <summary>
        /// 创建描述文件到目录下;
        /// </summary>
        static void WriteDescription(string directory, MapDescription description)
        {
            string descriptionFilePath = GetDescriptionFilePath(directory);
            MapDescription.Write(descriptionFilePath, description);
        }

        static void WriteEmptyMap(string directory)
        {
            string filePath = GetMapDataFilePath(directory);
            TerrainMap.WriteEmpty(filePath);
        }

        static void WriteMap(string directory, TerrainMap map)
        {
            string mapFilePath = GetMapDataFilePath(directory);
            TerrainMap.Write(mapFilePath, map);
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
        /// 尝试读取此路径下的描述文件;
        /// </summary>
        public static bool TryRead(string directory, out TerrainMapFile item)
        {
            try
            {
                item = Read(directory);
                return true;
            }
            catch (FileNotFoundException)
            {
            }
            catch (InvalidOperationException)
            {
            }

            item = default(TerrainMapFile);
            return false;
        }

        /// <summary>
        /// 读取此路径下的描述文件;
        /// </summary>
        public static TerrainMapFile Read(string directory)
        {
            MapDescription description = ReadDescription(directory);

            string dataFilePath = GetMapDataFilePath(directory);
            if (!File.Exists(dataFilePath))
                throw new FileNotFoundException();

            return new TerrainMapFile(directory, description);
        }

        /// <summary>
        /// 从目录下读取描述文件;
        /// </summary>
        static MapDescription ReadDescription(string directory)
        {
            string descriptionFilePath = GetDescriptionFilePath(directory);
            MapDescription description = MapDescription.Read(descriptionFilePath);
            return description;
        }

        /// <summary>
        /// 从目录下读取地图文件;
        /// </summary>
        static TerrainMap ReadMap(string directory)
        {
            string mapFilePath = GetMapDataFilePath(directory);
            return TerrainMap.Read(mapFilePath);
        }

        /// <summary>
        /// 确认此目录是否为存在文件;
        /// </summary>
        public static bool ExistsFile(string directory)
        {
            string descriptionFilePath = GetDescriptionFilePath(directory);
            string dataFilePath = GetMapDataFilePath(directory);

            return File.Exists(descriptionFilePath) && File.Exists(dataFilePath);
        }




        TerrainMapFile(string directoryPath, MapDescription description)
        {
            this.DirectoryPath = directoryPath;
            this.Description = description;
        }

        public string DirectoryPath { get; private set; }

        public MapDescription Description { get; private set; }

        public TerrainMap Map { get; private set; }

        /// <summary>
        /// 地图是否加载?
        /// </summary>
        public bool IsLoaded
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

            if (Map == null)
                Map = new TerrainMap();
        }

        /// <summary>
        /// 输出地图文件;
        /// </summary>
        public void WriteMap()
        {
            if (!IsLoaded)
                throw new ArgumentException("地图还未加载;");

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
            if (!(obj is TerrainMapFile))
                return false;
            return ((TerrainMapFile)obj).DirectoryPath == DirectoryPath;
        }

        public override int GetHashCode()
        {
            return DirectoryPath.GetHashCode();
        }

    }

}
