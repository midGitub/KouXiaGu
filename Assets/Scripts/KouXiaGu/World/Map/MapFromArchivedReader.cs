using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 从存档读取到地图;
    /// </summary>
    public class MapFromArchivedReader : IMapReader
    {
        const string ArchiveMapInfoFileName = "Map/Info";
        const string ArchiveMapFileName = "Map/Map";

        public string ArchiveDir { get; private set; }
        public MapFile MapFile { get; private set; }
        public ArchiveMapInfo ArchiveInfo { get; private set; }

        public MapInfo Info
        {
            get { return MapFile.Info; }
        }

        ArchiveMapInfoReader archiveInfoReader
        {
            get { return ArchiveMapInfoReader.DefaultReader; }
        }

        ArchiveMapReader archiveMapReader
        {
            get { return ArchiveMapReader.DefaultReader; }
        }

        public MapFromArchivedReader(string archiveDir)
        {
            ArchiveDir = archiveDir;
            ArchiveInfo = ReadArchiveMapInfo(archiveDir);
            MapFile = FindMapFileOrDefault(ArchiveInfo);

            if (MapFile == null)
                throw new FileNotFoundException("未找到对应的地图文件;MapID:" + ArchiveInfo.ID);
            if (!File.Exists(GetArchiveMapFilePath(archiveDir)))
                throw new FileNotFoundException("未找到地图存档;");
        }

        ArchiveMapInfo ReadArchiveMapInfo(string archiveDir)
        {
            string filePath = GetArchiveMapInfoFilePath(archiveDir);
            return archiveInfoReader.Read(filePath);
        }

        string GetArchiveMapInfoFilePath(string archiveDir)
        {
            string path = Path.Combine(archiveDir, ArchiveMapInfoFileName);
            Path.ChangeExtension(path, archiveInfoReader.FileExtension);
            return path;
        }

        MapFile FindMapFileOrDefault(ArchiveMapInfo archiveInfo)
        {
            var maps = MapFileManager.Default.SearchAll();
            var file = maps.FirstOrDefault(item => item.Info.ID == archiveInfo.ID);
            return file;
        }

        /// <summary>
        /// 读取到地图,包括存档内容;
        /// </summary>
        public Map Read()
        {
            Map map = MapFile.ReadMap();
            ArchiveMap archiveMap = ReadArchiveMap(ArchiveDir);
            map.Update(archiveMap);
            return map;
        }

        ArchiveMap ReadArchiveMap(string archiveDir)
        {
            string path = GetArchiveMapFilePath(archiveDir);
            return archiveMapReader.Read(path);
        }

        string GetArchiveMapFilePath(string archiveDir)
        {
            string path = Path.Combine(archiveDir, ArchiveMapFileName);
            Path.ChangeExtension(path, archiveMapReader.FileExtension);
            return path;
        }

    }

}
