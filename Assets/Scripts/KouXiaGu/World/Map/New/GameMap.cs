using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 游戏运行状态使用的地图数据;
    /// </summary>
    public class GameMap
    {
        public GameMap()
        {
            data = new MapData();
        }

        public GameMap(MapData data)
        {
            this.data = data;
            observableMap = new ObservableDictionary<CubicHexCoord, MapNode>(data.Map);
        }

        readonly MapData data;
        readonly ObservableDictionary<CubicHexCoord, MapNode> observableMap;

        internal MapData Data
        {
            get { return data; }
        }

        internal IDictionary<CubicHexCoord, MapNode> Map
        {
            get { return observableMap; }
        }

        public IReadOnlyDictionary<CubicHexCoord, MapNode> ReadOnlyMap
        {
            get { return observableMap; }
        }

        public IObservableDictionary<CubicHexCoord, MapNode> ObservableMap
        {
            get { return observableMap; }
        }


        class DataArchive
        {

        }
    }

}
