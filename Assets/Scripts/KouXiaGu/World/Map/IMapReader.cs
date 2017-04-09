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

}
