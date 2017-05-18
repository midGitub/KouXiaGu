using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    public interface IMapDataReader
    {
        /// <summary>
        /// 异步读取到地图数据;
        /// </summary>
        IAsyncOperation<MapData> ReadAsync(IGameData info);
    }

    abstract class MapDataReader
    {

    }

    class RandomMapDataCreater : MapDataReader
    {

    }

}
