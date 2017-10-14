using JiongXiaGu.Unity.Archives;
using JiongXiaGu.Unity.Resources;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图场景数据;
    /// </summary>
    public class MapSceneArchivalData : IDataArchival
    {
        /// <summary>
        /// 游戏地图存放目录;
        /// </summary>
        [PathDefinition(ResourceTypes.Archive, "存档地图存放路径;")]
        internal const string ArchiveMapFileName = "Map/ArchiveMap";

        /// <summary>
        /// 地图读写器;
        /// </summary>
        private readonly MapReader mapReader = new MapReader();

        /// <summary>
        /// 主地图文件信息;
        /// </summary>
        public MapFileInfo MainMapFileInfo { get; private set; }

        /// <summary>
        /// 用于存档的地图,若不存在则为Null;
        /// </summary>
        public Map ArchiveMap { get; private set; }

        public MapSceneArchivalData(MapFileInfo mainMapFileInfo, Map archiveMap)
        {
            if (mainMapFileInfo == null)
                throw new ArgumentNullException(nameof(mainMapFileInfo));
            if (archiveMap == null)
                throw new ArgumentNullException(nameof(archiveMap));

            MainMapFileInfo = mainMapFileInfo;
            ArchiveMap = archiveMap;
        }

        /// <summary>
        /// 从存档读取到场景地图状态;
        /// </summary>
        public MapSceneArchivalData(IArchiveFileInfo archive)
        {
            if (archive == null)
                throw new ArgumentNullException(nameof(archive));

            string archiveMapFilePath = GetArchiveMapFilePath(archive);
            ArchiveMap = mapReader.Read(archiveMapFilePath);
            MainMapFileInfo = mapReader.FindByName(ArchiveMap.Name);
        }

        /// <summary>
        /// 从地图名获取到场景地图状态;
        /// </summary>
        public MapSceneArchivalData(string mapName)
        {
            MainMapFileInfo = mapReader.FindByName(mapName);
        }

        /// <summary>
        /// 从地图文件信息创建一个初始的场景地图状态;
        /// </summary>
        public MapSceneArchivalData(MapFileInfo mainMapFileInfo)
        {
            MainMapFileInfo = mainMapFileInfo;
        }

        /// <summary>
        /// 创建一个新的游戏使用的地图;
        /// </summary>
        public WorldMap CreateMap()
        {
            Map mainMap = mapReader.Read(MainMapFileInfo);
            if (ArchiveMap != null)
            {
                return new WorldMap(mainMap, ArchiveMap);
            }
            else
            {
                return new WorldMap(mainMap);
            }
        }

        Task IDataArchival.Write(IArchiveFileInfo archive, CancellationToken cancellationToken)
        {
            return Task.Run(() => Write(archive, cancellationToken));
        }

        /// <summary>
        /// 输出地图资源到存档;
        /// </summary>
        public void Write(IArchiveFileInfo archive, CancellationToken cancellationToken)
        {
            string filePath = GetArchiveMapFilePath(archive);
            mapReader.WriteToFile(filePath, ArchiveMap);
        }

        /// <summary>
        /// 获取到对应的存档地图路径;
        /// </summary>
        private static string GetArchiveMapFilePath(IArchiveFileInfo archive)
        {
            string filePath = Path.Combine(archive.ArchiveDirectory, ArchiveMapFileName);
            return filePath;
        }
    }
}
