using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 读取地图接口;
    /// </summary>
    public interface IMapReader
    {
        /// <summary>
        /// 地图信息;
        /// </summary>
        MapFile File { get; }

        /// <summary>
        /// 获取到地图,包括存档内容;
        /// </summary>
        MapData GetMap();

        /// <summary>
        /// 获取到继续存档内容;
        /// </summary>
        ArchiveMap GetArchiveMap();
    }

}
