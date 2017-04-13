using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
            var map = PredefinedMapReader.Read();
            return new MapData(map);
        }
    }

    /// <summary>
    /// 读取预定义的地图文件,若不存在则创建空白的地图;
    /// </summary>
    public class MapDataReaderOrEmpty : IReader<MapData>
    {
        static readonly MapDataReader Reader = new MapDataReader();

        public MapData Read()
        {
            try
            {
                return Reader.Read();
            }
            catch (FileNotFoundException)
            {
                var map = new PredefinedMap();
                return new MapData(map);
            }
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
