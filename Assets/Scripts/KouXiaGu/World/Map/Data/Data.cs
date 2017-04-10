using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    public class Data
    {
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
            MapDataReader.instance.Write(Map);
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
    /// 直接读取预定义的地图文件;
    /// </summary>
    public class DataReader : IReader<Data>
    {
        protected MapDataReader MapDataReader
        {
            get { return MapDataReader.instance; }
        }

        public virtual Data Read()
        {
            MapData map = MapDataReader.Read();
            ArchiveMap archive = new ArchiveMap();
            return new Data(map, archive);
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
    /// 从存档读取到地图;
    /// </summary>
    public class ArchivedDataReader : DataReader
    {
        ArchiveMapReader archiveReader;

        public ArchivedDataReader(string archiveDir)
        {
            archiveReader = ArchiveMapReader.Create(archiveDir);
        }

        /// <summary>
        /// 读取到地图,包括存档内容;
        /// </summary>
        public override Data Read()
        {
            MapData map = MapDataReader.Read();
            ArchiveMap archiveMap = archiveReader.Read();
            map.Update(archiveMap);

            Data data = new Data(map, archiveMap);
            return data;
        }
    }

}
