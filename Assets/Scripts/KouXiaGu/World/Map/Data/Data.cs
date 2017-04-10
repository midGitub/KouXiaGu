using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    public class Data
    {
        MapFile file;
        public MapData Map { get; private set; }
        public ArchiveMap ArchiveMap { get; private set; }

        public Data(MapFile file, MapData map, ArchiveMap archive)
        {
            this.file = file;
            Map = map;
            ArchiveMap = archive;

            Map.Enable();
            ArchiveMap.Subscribe(Map);
        }

        /// <summary>
        /// 创建为一个新的地图;
        /// </summary>
        public void CreateMap(MapInfo info)
        {
            file = MapFileManager.Create(info);
            WriteMap();
        }

        /// <summary>
        /// 重新输出地图(保存修改后的地图);
        /// </summary>
        public void WriteMap()
        {
            file.WriteMap(Map);
        }

        /// <summary>
        /// 输出为存档;
        /// </summary>
        public void WriteArchived(string archivedDir)
        {
            ArchiveMapFile archiveFile = ArchiveMapFile.Create(archivedDir);
            ArchiveMapInfo info = new ArchiveMapInfo(file);
            archiveFile.WriteInfo(info);
            archiveFile.WriteMap(ArchiveMap);
        }

    }


    /// <summary>
    /// 直接读取预定义的地图文件;
    /// </summary>
    public class DataReader : IReader<Data>
    {
        MapFile file;

        public DataReader(MapFile mapFile)
        {
            file = mapFile;
        }

        public Data Read()
        {
            MapData map = file.ReadMap();
            ArchiveMap archive = new ArchiveMap();
            return new Data(file, map, archive);
        }
    }


    /// <summary>
    /// 从存档读取到地图;
    /// </summary>
    public class ArchivedDataReader : IReader<Data>
    {
        MapFile file;
        ArchiveMapFile archiveFile;

        public ArchivedDataReader(string archiveDir)
        {
            archiveFile = ArchiveMapFile.Create(archiveDir);
            file = FindMapFile(archiveFile);
        }

        MapFile FindMapFile(ArchiveMapFile archiveMapFile)
        {
            ArchiveMapInfo archiveInfo = archiveMapFile.ReadInfo();
            try
            {
                var maps = MapFileManager.SearchAll();
                var file = maps.First(item => item.Value.ID == archiveInfo.ID);
                return file.Key;
            }
            catch (InvalidOperationException)
            {
                throw new FileNotFoundException("未找到对应的地图文件;MapID:" + archiveInfo.ID);
            }
        }

        /// <summary>
        /// 读取到地图,包括存档内容;
        /// </summary>
        public Data Read()
        {
            MapData map = file.ReadMap();
            ArchiveMap archiveMap = archiveFile.ReadMap();
            map.Update(archiveMap);

            Data data = new Data(file, map, archiveMap);
            return data;
        }

    }

}
