using JiongXiaGu.Collections;
using JiongXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Threading;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 游戏运行状态使用的地图;
    /// </summary>
    public class WorldMap
    {
        /// <summary>
        /// 当前地图数据;
        /// </summary>
        public MapData MapData { get; private set; }

        /// <summary>
        /// 可观察的地图字典结构,提供修改的地图结构;
        /// </summary>
        public ObservableDictionary<RectCoord, MapNode> Map { get; private set; }

        /// <summary>
        /// 地图变化记录;
        /// </summary>
        public DictionaryChangedKeyRecorder<RectCoord, MapNode> MapChangedRecorder { get; private set; }

        public WorldMap() : this(new MapData())
        {
        }

        public WorldMap(MapData data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            MapData = data;
            Map = new ObservableDictionary<RectCoord, MapNode>(MapData.Data);
            MapChangedRecorder = new DictionaryChangedKeyRecorder<RectCoord, MapNode>(new HashSet<RectCoord>());
            Map.Subscribe(MapChangedRecorder);
        }

        public WorldMap(MapData data, ArchiveData archive)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (archive == null)
                throw new ArgumentNullException("archive");

            MapData = Combine(data, archive);
            Map = new ObservableDictionary<RectCoord, MapNode>(MapData.Data);
            MapChangedRecorder = new DictionaryChangedKeyRecorder<RectCoord, MapNode>(new HashSet<RectCoord>(archive.Data.Keys));
            Map.Subscribe(MapChangedRecorder);
        }

        /// <summary>
        /// 整合存档数据;
        /// </summary>
        MapData Combine(MapData dest, ArchiveData archive)
        {
            dest.Data.AddOrUpdate(archive.Data);
            return dest;
        }

        /// <summary>
        /// 获取到用于存档的地图数据,若不存在需要存档的内容,则返回null;
        /// </summary>
        public ArchiveData GetArchiveData()
        {
            if (MapChangedRecorder.ChangedPositions.Count == 0)
            {
                return null;
            }
            else
            {
                ArchiveData archivedData = new ArchiveData()
                {
                    Data = GetChangedData(),
                };
                return archivedData;
            }
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
    }
}
