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
    public class WorldMapData : UnitySingleton<WorldMapData>, IStartGameEvent, IArchiveEvent/*, IQuitGameEvent*/
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

        public IHexMap<ShortVector2, WorldNode> Map
        {
            get { return worldMap; }
        }
        public IObservable<MapNodeState<WorldNode>> observeChanges
        {
            get { return worldMap.observeChanges; }
        }

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

        IEnumerator IConstruct2<BuildGameData>.Prepare(BuildGameData item)
        {
            string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
            string fullPrefabMapDirectoryPath = item.ArchivedData.Archived.Map.PathPrefabMapDirectory;

            mapBlockIO.OnBulidGame(fullArchivedDirectoryPath, fullPrefabMapDirectoryPath);

            yield break;
        }

        IEnumerator IConstruct2<BuildGameData>.Construction(BuildGameData item)
        {
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

        IEnumerator IConstruct1<ArchivedGroup>.Construction(ArchivedGroup item)
        {
            string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
            mapBlockIO.OnGameArchive(fullArchivedDirectoryPath, worldMap.BlockMap);
            item.Archived.Map.PathPrefabMapDirectory = mapBlockIO.FullPrefabMapDirectoryPath;
            yield break;
        }

        [Serializable]
        private class UseArchiveBlockIO : ArchiveBlockIO<WorldNode> { }

        [Serializable]
        private class UseLoadBlockByRange : LoadBlockByRange<ArchiveBlock<WorldNode>> { }

    }

}
