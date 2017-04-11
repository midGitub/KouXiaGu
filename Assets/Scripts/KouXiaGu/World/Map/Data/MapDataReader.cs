using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 直接读取预定义的地图文件;
    /// </summary>
    public class MapDataReader : IReader<MapData>
    {
        protected PredefinedMapReader PredefinedMapReader
        {
            get { return PredefinedMapReader.instance; }
        }

        public virtual MapData Read()
        {
            PredefinedMap map = PredefinedMapReader.Read();
            return new MapData(map);
        }
    }

    /// <summary>
    /// 创建空白的地图;
    /// </summary>
    public class EmptyMapDataReader : IReader<MapData>
    {
        public MapData Read()
        {
            PredefinedMap map = new PredefinedMap();
            return new MapData(map);
        }
    }

    /// <summary>
    /// 从存档读取到地图;
    /// </summary>
    public class ArchivedDataReader : MapDataReader
    {
        ArchiveMapReader archiveReader;

        public ArchivedDataReader(string archiveDir)
        {
            archiveReader = ArchiveMapReader.Create(archiveDir);
        }

        /// <summary>
        /// 读取到地图,包括存档内容;
        /// </summary>
        public override MapData Read()
        {
            PredefinedMap map = PredefinedMapReader.Read();
            ArchiveMap archiveMap = archiveReader.Read();
            MapData data = new MapData(map, archiveMap);
            return data;
        }
    }

}
