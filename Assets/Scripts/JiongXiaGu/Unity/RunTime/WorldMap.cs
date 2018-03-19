using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Collections;
using JiongXiaGu.Grids;
using JiongXiaGu.Unity.Maps;

namespace JiongXiaGu.Unity.RunTime
{

    public class WorldMap<T>
    {
        private Map<T> map;
        private MapChangeRecorder<T> mapChangeRecorder;

        public WorldMap(IDictionary<T, MapNode> map)
        {
            this.map = new Map<T>(map);
            mapChangeRecorder = new MapChangeRecorder<T>();
            this.map.Subscribe(mapChangeRecorder);
        }

        public WorldMap(IDictionary<T, MapNode> map, IReadOnlyDictionary<T, ArchiveMapNode> archived)
        {
            this.map = new Map<T>(map);
            mapChangeRecorder = new MapChangeRecorder<T>(map, archived);
            this.map.Subscribe(mapChangeRecorder);
        }

        public IDictionary<T, ArchiveMapNode> GetArchived()
        {
            return mapChangeRecorder.GetArchiveMap(map);
        }
    }
}
