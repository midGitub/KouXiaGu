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
            dataArchive = new DataArchive(data);
        }

        public GameMap(MapData data)
        {
            this.data = data;
            observableMap = new ObservableDictionary<CubicHexCoord, MapNode>(data.Map);
            dataArchive = new DataArchive(data);
        }

        readonly MapData data;
        readonly ObservableDictionary<CubicHexCoord, MapNode> observableMap;
        readonly DataArchive dataArchive;

        internal MapData Data
        {
            get { return data; }
        }

        internal DataArchive DataArchive
        {
            get { return dataArchive; }
        }

        internal IDictionary<CubicHexCoord, MapNode> Map
        {
            get { return dataArchive; }
        }

        public IReadOnlyDictionary<CubicHexCoord, MapNode> ReadOnlyMap
        {
            get { return dataArchive; }
        }
    }

}
