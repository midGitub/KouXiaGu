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
        public Map MapData { get; private set; }

        /// <summary>
        /// 可观察的地图字典结构,提供修改的地图结构;
        /// </summary>
        public ObservableDictionary<RectCoord, MapNode> Map { get; private set; }

        /// <summary>
        /// 地图变化记录;
        /// </summary>
        public DictionaryChangedKeyRecorder<RectCoord, MapNode> MapChangedRecorder { get; private set; }

        public WorldMap() : this(new Map())
        {
        }

        public WorldMap(Map data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            MapData = data;
            Map = new ObservableDictionary<RectCoord, MapNode>(MapData.Data);
            MapChangedRecorder = new DictionaryChangedKeyRecorder<RectCoord, MapNode>(new HashSet<RectCoord>());
            Map.Subscribe(MapChangedRecorder);
        }

        public WorldMap(Map data, Map archive)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (archive == null)
                throw new ArgumentNullException("archive");

            data.AddArchive(archive);
            Map = new ObservableDictionary<RectCoord, MapNode>(MapData.Data);
            MapChangedRecorder = new DictionaryChangedKeyRecorder<RectCoord, MapNode>(new HashSet<RectCoord>(archive.Data.Keys));
            Map.Subscribe(MapChangedRecorder);
        }

        /// <summary>
        /// 获取到用于存档的地图数据,若不存在需要存档的内容,则返回null;
        /// </summary>
        public Map GetArchiveMap()
        {
            if (MapChangedRecorder.ChangedPositions.Count == 0)
            {
                return null;
            }
            else
            {
                Map archivedMap = new Map();
                GetChangedData(archivedMap.Data);
                return archivedMap;
            }
        }

        /// <summary>
        /// 获取到发生变化的节点合集;
        /// </summary>
        void GetChangedData(IDictionary<RectCoord, MapNode> map)
        {
            var changedData = new Dictionary<RectCoord, MapNode>();
            foreach (var position in MapChangedRecorder.ChangedPositions)
            {
                var node = map[position];
                changedData.Add(position, node);
            }
        }
    }
}
