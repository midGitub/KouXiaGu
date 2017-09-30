//using JiongXiaGu.Grids;
//using System;
//using System.Collections.Concurrent;
//using JiongXiaGu.VoidableOperations;

//namespace JiongXiaGu.Unity.RectMaps
//{

//    /// <summary>
//    /// 游戏运行状态下使用的地图;
//    /// </summary>
//    public class GameMap
//    {
//        /// <summary>
//        /// 当前地图数据;
//        /// </summary>
//        internal MapData MapData { get; private set; }

//        /// <summary>
//        /// 用于存档的地图;
//        /// </summary>
//        internal ArchiveData Archive { get; private set; }

//        /// <summary>
//        /// 可观察的地图结构;
//        /// </summary>
//        internal ObservableAsyncDictionary<RectCoord, MapNode> ObservableMap { get; private set; }

//        public MapNode this[RectCoord key]
//        {
//            get { return MapData.Data[key]; }
//            set { throw new NotImplementedException(); }
//        }

//        public GameMap(MapData mapData)
//        {
//            MapData = mapData;
//            ObservableMap = new ObservableAsyncDictionary<RectCoord, MapNode>(MapData.Data);
//        }

//        public GameMap(MapData mapData, ArchiveData archiveData)
//        {
//            MapData = mapData;
//            MapData.AddArchive(archiveData);
//            ObservableMap = new ObservableAsyncDictionary<RectCoord, MapNode>(MapData.Data);
//        }

//        /// <summary>
//        /// 应用未设置到地图的内容;
//        /// </summary>
//        public void TrackChanged()
//        {
//        }
//    }
//}
