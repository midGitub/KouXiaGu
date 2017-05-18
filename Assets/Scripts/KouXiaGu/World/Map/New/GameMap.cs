using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{


    public class GameMap
    {
        public GameMap()
        {
            data = new MapData();
        }

        readonly MapData data;

        internal MapData Data
        {
            get { return data; }
        }

        internal IDictionary<CubicHexCoord, MapNode> Map
        {
            get { throw new NotImplementedException(); }
        }

        public IReadOnlyDictionary<CubicHexCoord, MapNode> ReadOnlyMap
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 将地图数据保存到
        /// </summary>
        public void Save(string filePath)
        {
            throw new NotImplementedException();
        }

    }

}
