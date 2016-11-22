using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UniRx;

namespace KouXiaGu.World2D.Map
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
        private string archivedDirectoryName;
        
        [SerializeField]
        internal UseBlockMap bliockMap;
        [SerializeField]
        UseLoadByRange loadByRange;
        [SerializeField]
        UseMapBlockIO mapBlockIO;

        public IMap<IntVector2, WorldNode> Map { get { return bliockMap; } }

        void Awake()
        {
            loadByRange.BlockMap = bliockMap;
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
            mapBlockIO.OnGameArchive(fullArchivedDirectoryPath, bliockMap);
            item.Archived.World2D.PathPrefabMapDirectory = mapBlockIO.FullPrefabMapDirectoryPath;
            yield break;
        }

        IEnumerator IConstruct<QuitGameData>.Construction(QuitGameData item)
        {
            Map.Clear();
            yield break;
        }

        [Serializable]
        private class UseMapBlockIO : MapBlockIO<WorldNode> { }

        [Serializable]
        private class UseLoadByRange : LoadByRange<MapBlock<WorldNode>> { }

        [Serializable]
        public class UseBlockMap : BlockMap<WorldNode, MapBlock<WorldNode>> { }

    }

}
