using System;
using System.IO;
using KouXiaGu.Collections;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形地图文件;
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
        /// 从文件夹读取到;
        /// </summary>
        public static MapFileInfo Load(string directoryPath)
        {
            string filePath = Path.Combine(directoryPath, MAP_DESCRIPTION_FILE_NAME);
            var descr = (MapDescr)MapDescr.Serializer.DeserializeXiaGu(filePath);
            return new MapFileInfo(descr, directoryPath);
        }

        /// <summary>
        /// 创建一个新的地图;
        /// </summary>
        public static MapFileInfo Create(MapDescr description, string directoryPath)
        {
            var info = new MapFileInfo(description, directoryPath);

            Directory.CreateDirectory(directoryPath);
            info.UpdateDescription();

            return info;
        }


        /// <summary>
        /// 根据ID创建一个新地图到预定义的目录下,若已经存在则返回异常;;
        /// </summary>
        public MapFileInfo(MapDescr description, string directoryPath)
        {
            this.description = description;
            this.directoryPath = directoryPath;
            landformObserver = new LandformObserver();
        }

        /// <summary>
        /// 地貌记录;
        /// </summary>
        readonly LandformObserver landformObserver;

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
        public bool Loaded
        {
            get { return Map != null; }
        }

        /// <summary>
        /// 地形地图;
        /// </summary>
        public ObservableDictionary<CubicHexCoord, TerrainNode> Map { get; private set; }


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
            return "[Loaded" + Loaded + ";Path:" + directoryPath + "]";
        }


        /// <summary>
        /// 更新描述文件;
        /// </summary>
        public void UpdateDescription()
        {
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);

            string filePath = Path.Combine(DirectoryPath, MAP_DESCRIPTION_FILE_NAME);

            description.landformRecord = landformObserver.ToLandformRecord().ToArray();

            MapDescr.Serializer.SerializeXiaGu(filePath, Description);
        }

        /// <summary>
        /// 读取地图数据到内存;
        /// </summary>
        public void Load()
        {
            if (Map == null)
            {
                string prefabFilePath = Path.Combine(DirectoryPath, MAP_DATA_FILE_NAME);
                Map = LoadMap(prefabFilePath);

                landformObserver.Reset(description.landformRecord);
                landformObserver.Subscribe(Map);
            }
            else
            {
                throw new ArgumentException("重复读取地图!");
            }
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
            string prefabFilePath = Path.Combine(DirectoryPath, MAP_DATA_FILE_NAME);
            SaveMap(Map, prefabFilePath);

            UpdateDescription();
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

        ObservableDictionary<CubicHexCoord, TerrainNode> LoadMap(string filePath)
        {
            return SerializeExtensions.DeserializeProtoBuf<ObservableDictionary<CubicHexCoord, TerrainNode>>(filePath);
        }

        void SaveMap(ObservableDictionary<CubicHexCoord, TerrainNode> map, string filePath)
        {
            SerializeExtensions.SerializeProtoBuf(filePath, map);
        }

    }

}
