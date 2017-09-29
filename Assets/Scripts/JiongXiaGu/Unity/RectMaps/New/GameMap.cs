using JiongXiaGu.Grids;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using JiongXiaGu.VoidableOperations;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 游戏运行状态下使用的地图;
    /// </summary>
    public class GameMap
    {
        /// <summary>
        /// 当前地图数据;
        /// </summary>
        internal MapData MapData { get; private set; }

        /// <summary>
        /// 用于存档的地图;
        /// </summary>
        internal ArchiveData Archive { get; private set; }

        /// <summary>
        /// 可观察的地图结构;
        /// </summary>
        internal ObservableDictionary<RectCoord, MapNode> ObservableMap { get; private set; }

        /// <summary>
        /// 地图操作缓存队列;
        /// </summary>
        internal ConcurrentQueue<VoidableOperation> mapOperateCache;

        public MapNode this[RectCoord key]
        {
            get { return MapData.Data[key]; }
            set { throw new NotImplementedException(); }
        }

        public GameMap(MapData mapData)
        {
            MapData = mapData;
            ObservableMap = new ObservableDictionary<RectCoord, MapNode>(MapData.Data);
        }

        public GameMap(MapData mapData, ArchiveData archiveData)
        {
            MapData = mapData;
            MapData.AddArchive(archiveData);
            ObservableMap = new ObservableDictionary<RectCoord, MapNode>(MapData.Data);
        }

        /// <summary>
        /// 设置节点新的值(非即时应用到合集,此操作会延迟到下一个更新周期);
        /// </summary>
        public VoidableOperation SetValue(RectCoord key, MapNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加新的节点(非即时应用到合集,此操作会延迟到下一个更新周期);
        /// </summary>
        public VoidableOperation Add(RectCoord key, MapNode node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移除指定节点(非即时应用到合集,此操作会延迟到下一个更新周期);
        /// </summary>
        public VoidableOperation Remove(RectCoord key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 应用未设置到地图的内容;
        /// </summary>
        public void ApplyToMap()
        {
            VoidableOperation operate;
            while (mapOperateCache.TryDequeue(out operate))
            {
                operate.PerformDo();
            }
        }
    }
}
