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
    /// 游戏地图;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldMap : MonoBehaviour, IStartGameEvent, IArchiveEvent, IQuitGameEvent
    {

        [SerializeField]
        private Transform target;

        /// <summary>
        /// 保存到存档的位置;
        /// </summary>
        [SerializeField]
        string archivedDirectoryName;
        /// <summary>
        /// 地图块大小;
        /// </summary>
        [SerializeField]
        ShortVector2 partitionSizes = new ShortVector2(100, 100);

        [SerializeField]
        internal ContentBlockMap<WorldNode, ArchiveBlock<WorldNode>> worldMap;
        [SerializeField]
        UseLoadBlockByRange loadByRange;
        [SerializeField]
        UseArchiveBlockIO mapBlockIO;

        public IMap<IntVector2, WorldNode> Map
        {
            get { return worldMap; }
        }
        public IObservable<KeyValuePair<IntVector2, WorldNode>> observeChanges
        {
            get { return worldMap.observeChanges; }
        }

        void Awake()
        {
            worldMap = new ContentBlockMap<WorldNode, ArchiveBlock<WorldNode>>(partitionSizes);
            loadByRange.BlockMap = worldMap.BlockMap;
            loadByRange.MapBlockIO = mapBlockIO;
        }

        void OnMapDataUpdate(Vector3 targetPlanePoint)
        {
            IntVector2 mapPoint = WorldConvert.PlaneToHexPair(targetPlanePoint);
            loadByRange.UpdateCenterPoint(mapPoint);
        }

        /// <summary>
        /// 开始游戏时调用;
        /// </summary>
        IEnumerator IConstruct<BuildGameData>.Construction(BuildGameData item)
        {
            string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
            string fullPrefabMapDirectoryPath = item.ArchivedData.Archived.World2D.PathPrefabMapDirectory;

            mapBlockIO.OnBulidGame(fullArchivedDirectoryPath, fullPrefabMapDirectoryPath);

            target.ObserveEveryValueChanged(_ => target.position).
                Subscribe(OnMapDataUpdate);

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
            Map.Clear();
            yield break;
        }

        [Serializable]
        private class UseArchiveBlockIO : ArchiveBlockIO<WorldNode> { }

        [Serializable]
        private class UseLoadBlockByRange : LoadBlockByRange<ArchiveBlock<WorldNode>> { }

    }

}
