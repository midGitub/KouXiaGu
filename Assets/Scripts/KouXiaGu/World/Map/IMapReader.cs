using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 读取地图接口;
    /// </summary>
    public interface IMapReader
    {
        MapInfo Info { get; }
        Map Read();
    }

    /// <summary>
    /// 从存档读取到地图;
    /// </summary>
    public class MapFromArchivedReader : IMapReader
    {
        public MapInfo Info { get; private set; }

        public Map Read()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 从文件读取到地图;
    /// </summary>
    public class MapReader : IMapReader
    {
        public MapInfo Info { get; private set; }

        public Map Read()
        {
            throw new NotImplementedException();
        }
    }

}
