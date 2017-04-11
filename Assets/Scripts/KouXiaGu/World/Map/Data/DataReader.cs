using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

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
            return new Data(map);
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
            return new Data(map);
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
            Data data = new Data(map, archiveMap);
            return data;
        }
    }

}
