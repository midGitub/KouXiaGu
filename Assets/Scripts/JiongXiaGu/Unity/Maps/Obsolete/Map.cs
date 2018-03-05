//using JiongXiaGu.Collections;
//using JiongXiaGu.Grids;
//using System;
//using System.Collections.Generic;
//using System.Threading;

//namespace JiongXiaGu.Unity.Maps
//{

//    /// <summary>
//    /// 游戏运行状态使用的地图;
//    /// </summary>
//    public class WorldMap
//    {
//        /// <summary>
//        /// 可观察的地图字典结构,提供修改的地图结构;
//        /// </summary>
//        private ObservableDictionary<RectCoord, MapNode> map;

//        /// <summary>
//        /// 当前地图数据;
//        /// </summary>
//        public OMap MapData { get; private set; }

//        /// <summary>
//        /// 地图字典结构;
//        /// </summary>
//        public IDictionary<RectCoord, MapNode> Map
//        {
//            get { return map; }
//        }

//        /// <summary>
//        /// 地图变化观察接口;
//        /// </summary>
//        public IObservable<DictionaryEvent<RectCoord, MapNode>> MapChangedTracker
//        {
//            get { return map; }
//        }

//        /// <summary>
//        /// 地图读写锁,不支持递归;根据对地图进行读写操作进行对应的锁;
//        /// </summary>
//        public ReaderWriterLockSlim Lock { get; private set; } = new ReaderWriterLockSlim();

//        /// <summary>
//        /// 地图变化记录;
//        /// </summary>
//        public MapChangedRecorder MapChangedRecorder { get; private set; }

//        /// <summary>
//        /// 是否为只读?
//        /// </summary>
//        public bool IsReadOnly { get; internal set; }

//        public WorldMap(OMap data)
//        {
//            if (data == null)
//                throw new ArgumentNullException(nameof(data));

//            MapData = data;
//            map = new ObservableDictionary<RectCoord, MapNode>(data.MapData.Data);
//            MapChangedRecorder = new MapChangedRecorder();
//            map.Subscribe(MapChangedRecorder);
//        }

//        /// <summary>
//        /// 获取到用于存档的地图数据,若不存在需要存档的内容,则返回null;
//        /// </summary>
//        public OMap GetArchiveMap()
//        {
//            //if (MapChangedRecorder.ChangedPositions.Count == 0)
//            //{
//            //    return null;
//            //}
//            //else
//            //{
//            //    Map archivedMap = new Map(MapData.Description);
//            //    UpdateChangedData(archivedMap.Data);
//            //    return archivedMap;
//            //}
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// 获取到发生变化的节点合集;
//        /// </summary>
//        void UpdateChangedData(IDictionary<RectCoord, MapNode> map)
//        {
//            var changedData = new Dictionary<RectCoord, MapNode>();
//            foreach (var position in MapChangedRecorder.ChangedPositions)
//            {
//                var node = map[position];
//                changedData.Add(position, node);
//            }
//        }
//    }
//}
