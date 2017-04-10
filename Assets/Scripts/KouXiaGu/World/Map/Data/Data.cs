using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    public class Data
    {
        static readonly MapDataReader mapReader = MapDataReader.Create();

        public MapData Map { get; private set; }
        public ArchiveMap ArchiveMap { get; private set; }

        public Data(MapData map, ArchiveMap archive)
        {
            Map = map;
            ArchiveMap = archive;

            Map.Enable();
            ArchiveMap.Subscribe(Map);
        }

        /// <summary>
        /// 重新输出地图(保存修改后的地图);
        /// </summary>
        public void WriteMap()
        {
            mapReader.Write(Map);
        }

        /// <summary>
        /// 输出存档;
        /// </summary>
        public void WriteArchived(string archivedDir)
        {
            ArchiveMapReader reader = ArchiveMapReader.Create(archivedDir);
            reader.Write(ArchiveMap);
        }

    }


    /// <summary>
    /// 创建空白的地图;
    /// </summary>
    public class EmptyDataReader : IReader<Data>
    {
        public Data Read()
        {
            MapData map = new MapData();
            ArchiveMap archive = new ArchiveMap();
            return new Data(map, archive);
        }
    }

    /// <summary>
    /// 直接读取预定义的地图文件;
    /// </summary>
    public class DataReader : IReader<Data>
    {
        static readonly MapDataReader mapReader = MapDataReader.Create();

        public Data Read()
        {
            MapData map = mapReader.Read();
            ArchiveMap archive = new ArchiveMap();
            return new Data(map, archive);
        }
    }

    /// <summary>
    /// 从存档读取到地图;
    /// </summary>
    public class ArchivedDataReader : IReader<Data>
    {
        static readonly MapDataReader mapReader = MapDataReader.Create();
        ArchiveMapReader archiveReader;

        public ArchivedDataReader(string archiveDir)
        {
            archiveReader = ArchiveMapReader.Create(archiveDir);
        }

        /// <summary>
        /// 读取到地图,包括存档内容;
        /// </summary>
        public Data Read()
        {
            MapData map = mapReader.Read();
            ArchiveMap archiveMap = archiveReader.Read();
            map.Update(archiveMap);

            Data data = new Data(map, archiveMap);
            return data;
        }
    }

}
