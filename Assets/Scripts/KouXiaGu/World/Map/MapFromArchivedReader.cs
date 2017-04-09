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

        public MapFile File { get; private set; }
        public ArchiveMapInfo ArchiveInfo { get; private set; }

        public MapInfo Info
        {
            get { return File.Info; }
        }

        ArchiveMapInfoReader archiveInfoReader
        {
            get { return ArchiveMapInfoReader.DefaultReader; }
        }

        public MapFromArchivedReader(string archiveDir)
        {
            ArchiveInfo = ReadArchiveMapInfo(archiveDir);
            File = FindMapFileOrDefault(ArchiveInfo);

            if (File == null)
            {
                throw new FileNotFoundException("未找到对应的地图文件;MapID:" + ArchiveInfo.ID);
            }
        }

        public Map Read()
        {
            return File.ReadMap();
        }

        ArchiveMapInfo ReadArchiveMapInfo(string archiveDir)
        {
            string filePath = GetArchiveMapInfoPath(archiveDir);
            return archiveInfoReader.Read(filePath);
        }

        string GetArchiveMapInfoPath(string archiveDir)
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

    }

}
