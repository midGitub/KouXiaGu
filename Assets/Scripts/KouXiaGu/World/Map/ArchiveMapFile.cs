using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 存档地图文件 管理\记录;
    /// </summary>
    public class ArchiveMapFile
    {

        const string ArchiveMapInfoFileName = "Map/Info";
        const string ArchiveMapFileName = "Map/Map";

        public string ArchiveDir { get; private set; }

        ArchiveMapInfoReader archiveInfoReader
        {
            get { return ArchiveMapInfoReader.DefaultReader; }
        }

        ArchiveMapReader archiveMapReader
        {
            get { return ArchiveMapReader.DefaultReader; }
        }

        public ArchiveMapFile(string archiveDir)
        {
            ArchiveDir = archiveDir;
        }

        public ArchiveMapInfo ReadInfo()
        {
            string filePath = GetInfoFilePath();
            return archiveInfoReader.Read(filePath);
        }

        public void WriteInfo(ArchiveMapInfo info)
        {
            string filePath = GetInfoFilePath();
            archiveInfoReader.Write(filePath, info);
        }

        string GetInfoFilePath()
        {
            string path = Path.Combine(ArchiveDir, ArchiveMapInfoFileName);
            Path.ChangeExtension(path, archiveInfoReader.FileExtension);
            return path;
        }


        public ArchiveMap ReadMap()
        {
            string filePath = GetMapFilePath();
            return archiveMapReader.Read(filePath);
        }

        public void WriteMap(ArchiveMap map)
        {
            string filePath = GetMapFilePath();
            archiveMapReader.Write(filePath, map);
        }

        string GetMapFilePath()
        {
            string path = Path.Combine(ArchiveDir, ArchiveMapFileName);
            Path.ChangeExtension(path, archiveMapReader.FileExtension);
            return path;
        }

    }

}
