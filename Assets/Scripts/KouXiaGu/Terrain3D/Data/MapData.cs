using KouXiaGu.Collections;
using KouXiaGu.Grids;
using ProtoBuf;
using System.IO;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形地图;
    /// </summary>
    [ProtoContract]
    public class MapData
    {
        
        /// <summary>
        /// 存储文件后缀名;
        /// </summary>
        public const string FILE_EXTENSION = ".data";


        /// <summary>
        /// 创建一个空的地图;
        /// </summary>
        public static MapData Create()
        {
            MapData map = new MapData();

            map.Data = new ObservableDictionary<CubicHexCoord, TerrainNode>();
            map.Road = new RoadData(map.Data);
            map.Landform = new LandformData(map.Data);
            SubscribeChanged(map);

            return map;
        }

        /// <summary>
        /// 从文件读取到数据;
        /// </summary>
        public static MapData Create(string filePath)
        {
            MapData map = Read(filePath);

            map.Road.Data = map.Data;
            map.Landform = new LandformData(map.Data);
            SubscribeChanged(map);

            return map;
        }

        /// <summary>
        /// 从文件读取到数据,并且恢复记录;
        /// </summary>
        public static MapData Create(string filePath, string archiveFilePath)
        {
            MapData map = Read(filePath);
            ArchiveFile archive = ReadArchive(archiveFilePath);

            map.Road = archive.Road;
            map.Landform = new LandformData(map.Data);
            SubscribeChanged(map, archive);

            return map;
        }


        /// <summary>
        /// 从文件读取到;
        /// </summary>
        static MapData Read(string filePath)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            MapData data = ProtoBufExtensions.Deserialize<MapData>(filePath);

            if (!IsIntegrated(data))
                throw new FileNotFoundException("文件顺坏;");

            return data;
        }

        /// <summary>
        /// 从文件读取后检查是否完整;
        /// </summary>
        static bool IsIntegrated(MapData data)
        {
            return 
                data.Data != null &&
                data.Road != null;
        }

        /// <summary>
        /// 从文件读取到;
        /// </summary>
        static ArchiveFile ReadArchive(string archiveFilePath)
        {
            archiveFilePath = Path.ChangeExtension(archiveFilePath, FILE_EXTENSION);
            return ProtoBufExtensions.Deserialize<ArchiveFile>(archiveFilePath);
        }


        /// <summary>
        /// 监视到数据变化;
        /// </summary>
        static void SubscribeChanged(MapData map)
        {
            map.ArchiveData = new DictionaryArchiver<CubicHexCoord, TerrainNode>();
            map.ArchiveData.Subscribe(map.Data);
        }

        /// <summary>
        /// 从记录恢复到数据,且监视到数据变化;
        /// </summary>
        static void SubscribeChanged(MapData map, ArchiveFile archive)
        {
            map.ArchiveData = archive.ArchiveData;
            map.Data.AddOrUpdate(map.ArchiveData);
            map.ArchiveData.Subscribe(map.Data);
        }


        /// <summary>
        /// 输出数据到文件;
        /// </summary>
        public static void Write(string filePath, MapData map)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            ProtoBufExtensions.Serialize(filePath, map);
        }

        /// <summary>
        /// 输出记录到文件;
        /// </summary>
        public static void WriteArchive(string archiveFilePath, MapData map)
        {
            ArchiveFile archive = new ArchiveFile()
            {
                ArchiveData = map.ArchiveData,
                Road = map.Road,
            };

            archiveFilePath = Path.ChangeExtension(archiveFilePath, FILE_EXTENSION);
            ProtoBufExtensions.Serialize(archiveFilePath, archive);
        }


        MapData()
        {
        }


        /// <summary>
        /// 地图数据;
        /// </summary>
        [ProtoMember(1)]
        public ObservableDictionary<CubicHexCoord, TerrainNode> Data { get; private set; }

        /// <summary>
        /// 地形道路信息;
        /// </summary>
        [ProtoMember(2)]
        public RoadData Road { get; private set; }

        public LandformData Landform { get; private set; }

        /// <summary>
        /// 用于监视地图变化;
        /// </summary>
        public DictionaryArchiver<CubicHexCoord, TerrainNode> ArchiveData { get; private set; }


        /// <summary>
        /// 清除记录;
        /// </summary>
        public void ClearArchiveData()
        {
            this.ArchiveData.Clear();
        }


        /// <summary>
        /// 记录状态;
        /// </summary>
        [ProtoContract]
        class ArchiveFile
        {

            /// <summary>
            /// 地图变化信息;
            /// </summary>
            [ProtoMember(1)]
            public DictionaryArchiver<CubicHexCoord, TerrainNode> ArchiveData { get; set; }

            /// <summary>
            /// 地形道路信息;
            /// </summary>
            [ProtoMember(2)]
            public RoadData Road { get; set; }

        }


    }

}
