using System;
using System.Collections;
using System.IO;
using UniRx;
using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 游戏中使用的地图结构;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldMap : MonoBehaviour, IBuildInThread, IArchiveInThread, IQuitInThread, IBuildInCoroutine, IQuitInCoroutine
    {
        /// <summary>
        /// 地图块前缀;
        /// </summary>
        [SerializeField, Header("地图文件路径定义")]
        private string addressPrefix;
        /// <summary>
        /// 保存到存档的位置;
        /// </summary>
        [SerializeField]
        private string archivedDirectoryName;
        /// <summary>
        /// 地图缓存文件目录;
        /// </summary>
        [SerializeField]
        private string archiveTempDirectoryName;
        /// <summary>
        /// 游戏地图;
        /// </summary>
        [SerializeField]
        private wnBlockMap worldMap;
        [SerializeField]
        private BlockMapUpdater followToUpdate;

        protected string ArchivedSearchPattern
        {
            get { return addressPrefix + "*"; }
        }
        internal wnBlockMap wnWorldMap
        {
            get { return worldMap; }
        }
        public IMap<IntVector2, WorldNode> Map
        {
            get { return worldMap; }
        }
        public IReadOnlyMap<IntVector2, IReadOnlyWorldNode> ReadMap
        {
            get { return worldMap; }
        }

        public string FullArchiveTempDirectoryPath
        {
            get { return worldMap.FullArchiveTempDirectoryPath; }
            private set { worldMap.FullArchiveTempDirectoryPath = value; }
        }
        public string FullPrefabMapDirectoryPath
        {
            get { return worldMap.FullPrefabMapDirectoryPath; }
            private set { worldMap.FullPrefabMapDirectoryPath = value; }
        }


        private void Awake()
        {
            worldMap.Awake();
            worldMap.AddressPrefix = addressPrefix;
            FullArchiveTempDirectoryPath = Path.Combine(Application.dataPath, archiveTempDirectoryName);

            followToUpdate.Awake(worldMap);
        }

        /// <summary>
        /// 获取到完整的归档地图文件夹路径;
        /// </summary>
        private string GetFullArchivedDirectoryPath(ArchivedGroup item)
        {
            string fullArchivedDirectoryPath = Path.Combine(item.ArchivedPath, archivedDirectoryName);
            return fullArchivedDirectoryPath;
        }

        [ContextMenu("存档合并到预制")]
        public void CombineToPrefabMap()
        {
            worldMap.CombineToPrefabMap();
        }


        #region 线程初始化;

        void IThreadInit<BuildGameData>.Initialize(
            BuildGameData item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            try
            {
                RecoveryLoadArchived(item, cancelable);
                RecoveryTempData(item, cancelable);
            }
            catch (Exception e)
            {
                onError(e);
            }
            runningDoneCallBreak();
        }

        /// <summary>
        /// 将存档的归档地图拷贝到缓存地图文件夹下;
        /// </summary>
        private void RecoveryTempData(ArchivedGroup item, ICancelable cancelable)
        {
            FileHelper.DeleteFileInDirectory(FullArchiveTempDirectoryPath, ArchivedSearchPattern);
            if (item.FromFile)
            {
                string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
                FileHelper.CopyDirectory(cancelable,
                    fullArchivedDirectoryPath, FullArchiveTempDirectoryPath, ArchivedSearchPattern, true);
            }
        }

        /// <summary>
        /// 从存档读取信息;
        /// </summary>
        private void RecoveryLoadArchived(ArchivedGroup item, ICancelable cancelable)
        {
            FullPrefabMapDirectoryPath = item.Archived.World2D.PathPrefabMapDirectory;
            if (!Directory.Exists(FullPrefabMapDirectoryPath))
                throw new FileNotFoundException("地图丢失!" + FullPrefabMapDirectoryPath);
        }


        void IThreadInit<ArchivedGroup>.Initialize(
            ArchivedGroup item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            try
            {
                ArchiveSaveMap(item, cancelable);
                ArchiveCopyData(item, cancelable);
                ArchiveOutput(item, cancelable);
            }
            catch (Exception e)
            {
                onError(e);
            }
            runningDoneCallBreak();
        }

        private void ArchiveSaveMap(ArchivedGroup item, ICancelable cancelable)
        {
            followToUpdate.Save();
        }

        private void ArchiveCopyData(ArchivedGroup item, ICancelable cancelable)
        {
            string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
            FileHelper.CopyDirectory(cancelable,
                FullArchiveTempDirectoryPath, fullArchivedDirectoryPath, ArchivedSearchPattern, true);
        }

        private void ArchiveOutput(ArchivedGroup item, ICancelable cancelable)
        {
            item.Archived.World2D.PathPrefabMapDirectory = FullPrefabMapDirectoryPath;
        }


        void IThreadInit<Unit>.Initialize(
            Unit item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            try
            {
                worldMap.Clear();
            }
            catch (Exception e)
            {
                onError(e);
            }
            runningDoneCallBreak();
        }

        #endregion


        #region 协程初始化;

        IEnumerator ICoroutineInit<BuildGameData>.Initialize(
            BuildGameData item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            followToUpdate.StartLoad();

            for (int waitingTime = 8; waitingTime > 0; waitingTime --)
                yield return null;

            while (followToUpdate.StateForNow != MapState.None)
                yield return null;

            yield break;
        }

        IEnumerator ICoroutineInit<Unit>.Initialize(
            Unit item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            followToUpdate.StopLoad();
            yield break;
        }

        #endregion


        public override string ToString()
        {
            string str = base.ToString();

            str += "\n worldMap :\n" + worldMap.ToString();

            return str;
        }

#if UNITY_EDITOR
        [ContextMenu("ToString")]
        private void Test_DebugToString()
        {
            Debug.Log(this.ToString());
        }
#endif

    }

}
