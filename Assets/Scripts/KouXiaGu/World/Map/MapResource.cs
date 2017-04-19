using System;
using System.Collections.Generic;
using KouXiaGu.Grids;

namespace KouXiaGu.World.Map
{

    public class MapResource
    {

        public static MapResource Read()
        {
            throw new NotImplementedException();
        }

        public static IAsyncOperation<MapResource> ReadAsync()
        {
            throw new NotImplementedException();
        }


        MapResource()
        {
        }

        internal MapData MapData { get; private set; }

        /// <summary>
        /// 游戏使用的地图数据;
        /// </summary>
        public IDictionary<CubicHexCoord, MapNode> Map
        {
            get { return MapData.PredefinedMap.Data; }
        }

        


    }

}
