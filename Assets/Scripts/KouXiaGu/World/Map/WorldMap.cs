using KouXiaGu.Concurrent;
using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Resources;
using System.Threading;
using System.IO;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 游戏运行状态使用的地图数据;
    /// </summary>
    public class WorldMap
    {
        public WorldMap() : this(new MapData())
        {
        }

        public WorldMap(MapData data)
        {
            Init(data);
        }

        public WorldMap(MapData data, MapData archive)
        {
            Combine(data, archive);
            Init(data);
        }

        internal MapData data { get; private set; }
        internal ObservableDictionary<CubicHexCoord, MapNode> observableMap { get; private set; }
        internal MapChangedRecorder<CubicHexCoord, MapNode> mapChangedRecorder { get; private set; }

        /// <summary>
        /// 地图读写锁,在编辑地图时选择上锁;
        /// </summary>
        public ReaderWriterLockSlim MapEditorLock { get; private set; }

        /// <summary>
        /// 提供修改的地图结构;
        /// </summary>
        public IDictionary<CubicHexCoord, MapNode> Map
        {
            get { return observableMap; }
        }

        /// <summary>
        /// 只读的地图结构;
        /// </summary>
        public IReadOnlyDictionary<CubicHexCoord, MapNode> ReadOnlyMap
        {
            get { return observableMap; }
        }

        /// <summary>
        /// 订阅地图数据变化;
        /// </summary>
        public IObservableDictionary<CubicHexCoord, MapNode> ObservableMap
        {
            get { return observableMap; }
        }

        void Init(MapData data)
        {
            this.data = data;
            observableMap = new ObservableDictionary<CubicHexCoord, MapNode>(data.Data);
            mapChangedRecorder = new MapChangedRecorder<CubicHexCoord, MapNode>(observableMap);
            MapEditorLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// 将存档内容合并;
        /// </summary>
        void Combine(MapData dest, MapData archive)
        {
            dest.Data.AddOrUpdate(archive.Data);
            dest.Building = archive.Building;
            dest.Landform = archive.Landform;
            dest.Road = archive.Road;
            dest.TownCorePositions = archive.TownCorePositions;
        }

        /// <summary>
        /// 将地图保存;
        /// </summary>
        public void Write(IWriter<MapData> writer)
        {
            using (MapEditorLock.WriteLock())
            {
                writer.Write(data);
            }
        }

        /// <summary>
        /// 将地图存档保存;
        /// </summary>
        public void WriteArchivedData(IWriter<MapData> writer)
        {
            using (MapEditorLock.WriteLock())
            {
                MapData archivedData = new MapData()
                {
                    Data = GetChangedData(),
                    Building = data.Building,
                    Landform = data.Landform,
                    Road = data.Road,
                    TownCorePositions = data.TownCorePositions,
                };
                writer.Write(archivedData);
            }
        }

        /// <summary>
        /// 获取到发生变化的节点合集;
        /// </summary>
        Dictionary<CubicHexCoord, MapNode> GetChangedData()
        {
            Dictionary<CubicHexCoord, MapNode> map = data.Data;
            var changedData = new Dictionary<CubicHexCoord, MapNode>();
            foreach (var position in mapChangedRecorder.ChangedPositions)
            {
                var node = map[position];
                changedData.Add(position, node);
            }
            return changedData;
        }

        public static implicit operator MapData(WorldMap gameMap)
        {
            return gameMap.data;
        }
    }
}
