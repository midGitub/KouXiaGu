using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UniRx;
using KouXiaGu.World2D.Map;
using System.Collections.Generic;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 游戏地图数据,保存地图和读取地图;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldMapData : UnitySingleton<WorldMapData>, IStartGameEvent, IArchiveEvent, IQuitGameEvent
    {

        /// <summary>
        /// 保存到存档的位置;
        /// </summary>
        [SerializeField]
        string archivedDirectoryName = "Maps";
        /// <summary>
        /// 地图块大小;
        /// </summary>
        [SerializeField]
        ShortVector2 partitionSizes = new ShortVector2(100, 100);

        [SerializeField]
        internal ObservableBlockMap<WorldNode, ArchiveBlock<WorldNode>> worldMap;
        [SerializeField]
        UseLoadBlockByRange loadByRange;
        [SerializeField]
        UseArchiveBlockIO mapBlockIO;

        public IMap<ShortVector2, WorldNode> Map
        {
            get { return worldMap; }
        }
        public IObservable<MapNodeState<WorldNode>> observeChanges
        {
            get { return worldMap.observeChanges; }
        }

        /// <summary>
        /// 是否已经准备完毕?
        /// </summary>
        [ShowOnlyProperty]
        public bool IsReady { get; private set; }

        void Awake()
        {
            worldMap = new ObservableBlockMap<WorldNode, ArchiveBlock<WorldNode>>(partitionSizes);

            loadByRange.BlockMap = worldMap.BlockMap;
            loadByRange.MapBlockIO = mapBlockIO;
        }

        /// <summary>
        /// 根据中心点更新地图数据;
        /// </summary>
        public void OnMapDataUpdate(Vector3 targetPlanePoint, ShortVector2 targetMapPoint)
        {
            loadByRange.UpdateCenterPoint(targetMapPoint);
        }

        /// <summary>
        /// 开始游戏时调用;
        /// </summary>
        IEnumerator IConstruct<BuildGameData>.Construction(BuildGameData item)
        {
            string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
            string fullPrefabMapDirectoryPath = item.ArchivedData.Archived.World2D.PathPrefabMapDirectory;

            mapBlockIO.OnBulidGame(fullArchivedDirectoryPath, fullPrefabMapDirectoryPath);

            IsReady = true;
            yield break;
        }

        /// <summary>
        /// 获取到完整的归档地图文件夹路径;
        /// </summary>
        private string GetFullArchivedDirectoryPath(ArchivedGroup item)
        {
            string fullArchivedDirectoryPath = Path.Combine(item.ArchivedPath, archivedDirectoryName);
            return fullArchivedDirectoryPath;
        }

        IEnumerator IConstruct<ArchivedGroup>.Construction(ArchivedGroup item)
        {
            string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
            mapBlockIO.OnGameArchive(fullArchivedDirectoryPath, worldMap.BlockMap);
            item.Archived.World2D.PathPrefabMapDirectory = mapBlockIO.FullPrefabMapDirectoryPath;
            yield break;
        }

        IEnumerator IConstruct<QuitGameData>.Construction(QuitGameData item)
        {
            //Map.Clear();

            IsReady = false;
            yield break;
        }

        [Serializable]
        private class UseArchiveBlockIO : ArchiveBlockIO<WorldNode> { }

        [Serializable]
        private class UseLoadBlockByRange : LoadBlockByRange<ArchiveBlock<WorldNode>> { }

    }

}
