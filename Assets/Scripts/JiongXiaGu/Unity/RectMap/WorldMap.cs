using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Grids;
using System.Threading;
using JiongXiaGu.Resources;
using JiongXiaGu.Concurrent;
using JiongXiaGu.Collections;

namespace JiongXiaGu.World.RectMap
{

    /// <summary>
    /// 游戏运行状态使用的地图;
    /// </summary>
    public class WorldMap
    {
        public WorldMap() : this(new MapData())
        {
        }

        public WorldMap(MapData data)
        {
            MapData = data;
            observableMap = new ObservableDictionary<RectCoord, MapNode>(MapData.Data);
            MapChangedRecorder = new WorldMapChangedRecorder<RectCoord, MapNode>(observableMap);
            MapEditorLock = new ReaderWriterLockSlim();
        }

        public WorldMap(MapData data, MapData archive)
        {
            MapData = Combine(data, archive);
            observableMap = new ObservableDictionary<RectCoord, MapNode>(MapData.Data);
            MapChangedRecorder = new WorldMapChangedRecorder<RectCoord, MapNode>(observableMap, archive.Data.Keys);
            MapEditorLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// 当前地图数据;
        /// </summary>
        public MapData MapData { get; private set; }
        ObservableDictionary<RectCoord, MapNode> observableMap;
        public WorldMapChangedRecorder<RectCoord, MapNode> MapChangedRecorder { get; private set; }

        /// <summary>
        /// 地图读写锁;
        /// </summary>
        public ReaderWriterLockSlim MapEditorLock { get; private set; }

        /// <summary>
        /// 提供修改的地图结构;
        /// </summary>
        public IDictionary<RectCoord, MapNode> Map
        {
            get { return observableMap; }
        }

        /// <summary>
        /// 只读的地图结构;
        /// </summary>
        public IReadOnlyDictionary<RectCoord, MapNode> ReadOnlyMap
        {
            get { return MapData.Data; }
        }

        /// <summary>
        /// 订阅地图数据变化;
        /// </summary>
        public IObservableDictionary<RectCoord, MapNode> ObservableMap
        {
            get { return observableMap; }
        }

        /// <summary>
        /// 整合存档数据;
        /// </summary>
        MapData Combine(MapData dest, MapData archive)
        {
            dest.Data.AddOrUpdate(archive.Data);
            return dest;
        }

        /// <summary>
        /// 获取到用于存档的地图数据;
        /// </summary>
        public MapData GetArchivedMapData()
        {
            MapData archivedData = new MapData()
            {
                Data = GetChangedData(),
            };
            return archivedData;
        }

        /// <summary>
        /// 获取到发生变化的节点合集;
        /// </summary>
        Dictionary<RectCoord, MapNode> GetChangedData()
        {
            Dictionary<RectCoord, MapNode> map = MapData.Data;
            var changedData = new Dictionary<RectCoord, MapNode>();
            foreach (var position in MapChangedRecorder.ChangedPositions)
            {
                var node = map[position];
                changedData.Add(position, node);
            }
            return changedData;
        }

        public static implicit operator MapData(WorldMap gameMap)
        {
            return gameMap.MapData;
        }

        /// <summary>
        /// 记录地图变化;
        /// </summary>
        public class WorldMapChangedRecorder<TKey, Tvalue> : IDictionaryObserver<TKey, Tvalue>, IDisposable
        {
            public WorldMapChangedRecorder(IObservableDictionary<TKey, Tvalue> observableMap)
            {
                ChangedPositions = new HashSet<TKey>();
                unsubscriber = observableMap.Subscribe(this);
            }

            public WorldMapChangedRecorder(IObservableDictionary<TKey, Tvalue> observableMap, IEnumerable<TKey> changedPositions)
            {
                ChangedPositions = new HashSet<TKey>(changedPositions);
                unsubscriber = observableMap.Subscribe(this);
            }

            IDisposable unsubscriber;
            public HashSet<TKey> ChangedPositions { get; private set; }

            void IDictionaryObserver<TKey, Tvalue>.OnAdded(TKey key, Tvalue newValue)
            {
                ChangedPositions.Add(key);
            }

            void IDictionaryObserver<TKey, Tvalue>.OnRemoved(TKey key, Tvalue originalValue)
            {
                ChangedPositions.Remove(key);
            }

            void IDictionaryObserver<TKey, Tvalue>.OnUpdated(TKey key, Tvalue originalValue, Tvalue newValue)
            {
                ChangedPositions.Add(key);
            }

            void IDictionaryObserver<TKey, Tvalue>.OnClear(IDictionary<TKey, Tvalue> dictionary)
            {
                ChangedPositions.Clear();
            }

            public void Clear()
            {
                ChangedPositions.Clear();
            }

            /// <summary>
            /// 取消订阅;
            /// </summary>
            public void Dispose()
            {
                if (unsubscriber != null)
                {
                    unsubscriber.Dispose();
                    unsubscriber = null;
                }
            }
        }
    }
}
