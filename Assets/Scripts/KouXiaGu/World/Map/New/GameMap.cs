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
        public GameMap() : this(new MapData())
        {
        }

        public GameMap(MapData data)
        {
            this.data = data;
            observableMap = new ObservableDictionary<CubicHexCoord, MapNode>(data.Map);
            mapChangedRecorder = new MapChangedRecorder<CubicHexCoord, MapNode>(observableMap);
        }
        
        readonly MapData data;
        readonly ObservableDictionary<CubicHexCoord, MapNode> observableMap;
        readonly MapChangedRecorder<CubicHexCoord, MapNode> mapChangedRecorder;

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

        /// <summary>
        /// 获取到用于归档的数据;
        /// </summary>
        public MapData GetArchivedData()
        {
            MapData archivedData = new MapData()
            {
                Map = mapChangedRecorder.GetChangedData(),
            };
            return archivedData;
        }
    }

    /// <summary>
    /// 地图存档;
    /// </summary>
    class GameMapArchiver : IArchiver
    {
        public GameMapArchiver(GameMap data)
        {
            gameMap = data;
        }

        readonly GameMap gameMap;
        MapData archivedData;

        public void Prepare()
        {
            archivedData = gameMap.GetArchivedData();
        }

        public void Write(ArchiveFile file)
        {
        }
    }
}
