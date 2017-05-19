//using KouXiaGu.Grids;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections;

//namespace KouXiaGu.World.Map
//{

//    /// <summary>
//    /// 记录地图变化;
//    /// </summary>
//    class DataArchive : IDictionaryObserver<CubicHexCoord, MapNode>
//    {
//        public DataArchive(MapData data, IObservableDictionary<CubicHexCoord, MapNode> observableMap)
//        {
//            this.data = data;
//            this.observableMap = observableMap;
//            changedPositions = new HashSet<CubicHexCoord>();
//            observableMap.Subscribe(this);
//        }

//        readonly MapData data;
//        readonly IObservableDictionary<CubicHexCoord, MapNode> observableMap;
//        readonly HashSet<CubicHexCoord> changedPositions;

//        IDictionary<CubicHexCoord, MapNode> map
//        {
//            get { return data.Map; }
//        }

//        public ICollection<CubicHexCoord> ChangedPositions
//        {
//            get { return changedPositions; }
//        }

//        void IDictionaryObserver<CubicHexCoord, MapNode>.OnAdded(CubicHexCoord key, MapNode newValue)
//        {
//            changedPositions.Add(key);
//        }

//        void IDictionaryObserver<CubicHexCoord, MapNode>.OnRemoved(CubicHexCoord key, MapNode originalValue)
//        {
//            changedPositions.Remove(key);
//        }

//        void IDictionaryObserver<CubicHexCoord, MapNode>.OnUpdated(CubicHexCoord key, MapNode originalValue, MapNode newValue)
//        {
//            return;
//        }

//        /// <summary>
//        /// 获取变化的到用于归档的数据;
//        /// </summary>
//        public MapData GetArchivedData()
//        {
//            MapData archivedData = new MapData()
//            {
//                Map = GetChangedData(),
//            };
//            return archivedData;
//        }

//        /// <summary>
//        /// 获取到发生变化的节点结构;
//        /// </summary>
//        public Dictionary<CubicHexCoord, MapNode> GetChangedData()
//        {
//            Dictionary<CubicHexCoord, MapNode> changedData = new Dictionary<CubicHexCoord, MapNode>();
//            foreach (var position in changedPositions)
//            {
//                MapNode node = map[position];
//                changedData.Add(position, node);
//            }
//            return changedData;
//        }
//    }
//}
