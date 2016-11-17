using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UniRx;
using UnityEngine;
using System.Threading;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 游戏中使用的地图结构;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldMap : MonoBehaviour, IBuildInThread, IArchiveInThread, IQuitInThread, IBuildInCoroutine, IQuitInCoroutine
    {

        [SerializeField]
        private Transform target;
        /// <summary>
        /// 保存到存档的位置;
        /// </summary>
        [SerializeField]
        private string archivedDirectoryName;
        [SerializeField]
        private WorldDynBlockMapEx worldDynMap;

        private IDisposable mapUpdateEvent;

        internal WorldDynBlockMapEx WorldDynMap
        {
            get { return worldDynMap; }
        }
        public IMap<IntVector2, WorldNode> Map
        {
            get { return worldDynMap; }
        }
        public IReadOnlyMap<IntVector2, IReadOnlyWorldNode> ReadMap
        {
            get { return worldDynMap; }
        }


        private void Awake()
        {
            worldDynMap.Awake();
        }


        /// <summary>
        /// 获取到完整的归档地图文件夹路径;
        /// </summary>
        private string GetFullArchivedDirectoryPath(ArchivedGroup item)
        {
            string fullArchivedDirectoryPath = Path.Combine(item.ArchivedPath, archivedDirectoryName);
            return fullArchivedDirectoryPath;
        }


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
            worldDynMap.DeleteMapFile(worldDynMap.FullArchiveTempDirectoryPath);
            if (item.FromFile)
            {
                string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
                worldDynMap.MapFileCopyTo(fullArchivedDirectoryPath, worldDynMap.FullArchiveTempDirectoryPath, true);
            }
        }

        /// <summary>
        /// 从存档读取信息;
        /// </summary>
        private void RecoveryLoadArchived(ArchivedGroup item, ICancelable cancelable)
        {
            worldDynMap.FullPrefabMapDirectoryPath = item.Archived.World2D.PathPrefabMapDirectory;
            if (!Directory.Exists(worldDynMap.FullPrefabMapDirectoryPath))
                throw new FileNotFoundException("地图丢失!" + worldDynMap.FullPrefabMapDirectoryPath);
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

        /// <summary>
        /// 对地图进行保存;
        /// </summary>
        private void ArchiveSaveMap(ArchivedGroup item, ICancelable cancelable)
        {
            worldDynMap.OnSave();
        }

        /// <summary>
        /// 复制缓存地图到存档路径下;
        /// </summary>
        private void ArchiveCopyData(ArchivedGroup item, ICancelable cancelable)
        {
            string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
            worldDynMap.ArchiveMapCopyTo(fullArchivedDirectoryPath, true);
        }

        /// <summary>
        /// 将存档保存的信息初始化;
        /// </summary>
        private void ArchiveOutput(ArchivedGroup item, ICancelable cancelable)
        {
            item.Archived.World2D.PathPrefabMapDirectory = worldDynMap.FullPrefabMapDirectoryPath;
        }


        void IThreadInit<Unit>.Initialize(
            Unit item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            try
            {
                OnQuitClearMap();
            }
            catch (Exception e)
            {
                onError(e);
            }
            runningDoneCallBreak();
        }

        /// <summary>
        /// 当退出时清除地图信息;
        /// </summary>
        private void OnQuitClearMap()
        {
            worldDynMap.Clear();
        }

        #region 协程初始化;

        IEnumerator ICoroutineInit<BuildGameData>.Initialize(
            BuildGameData item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            this.mapUpdateEvent = Observable.EveryUpdate().Subscribe(MapUpdate);

            for (int waitingTime = 10; waitingTime > 0; waitingTime--)
                yield return null;

            while (worldDynMap.MapState != MapState.None || cancelable.IsDisposed)
            {
                yield return null;
            }

            yield break;
        }

        private void MapUpdate(long unit)
        {
            IntVector2 mapPoint = WorldConvert.PlaneToHexPair(target.position);
            worldDynMap.OnMapUpdate(mapPoint);
        }

        IEnumerator ICoroutineInit<Unit>.Initialize(
            Unit item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            if (this.mapUpdateEvent != null)
            {
                this.mapUpdateEvent.Dispose();
            }
            yield break;
        }

        #endregion



#if UNITY_EDITOR
        [ContextMenu("ToString")]
        private void Test_Debug()
        {
            string str = base.ToString();

            str += "\n worldMap :\n" + worldDynMap.ToString();

            Debug.Log(str);
        }
#endif

        /// <summary>
        /// 地图结构;
        /// </summary>
        [Serializable]
        public sealed class WorldDynBlockMapEx : DynBlockMapEx<WorldNode, IReadOnlyWorldNode, MapBlock<WorldNode, IReadOnlyWorldNode>>,
            IMapBlockInfo
        {
            private WorldDynBlockMapEx() { }

            /// <summary>
            /// 地图当前状态;
            /// </summary>
            [SerializeField]
            private MapState mapState;
            /// <summary>
            /// 地图块前缀;
            /// </summary>
            [SerializeField]
            private string addressPrefix;
            /// <summary>
            /// 地图缓存文件目录;
            /// </summary>
            [SerializeField]
            private string archiveTempDirectoryName;

            public string AddressPrefix
            {
                get { return addressPrefix; }
            }
            public string FullArchiveTempDirectoryPath { get; set; }
            public string FullPrefabMapDirectoryPath { get; set; }

            public MapState MapState
            {
                get { return mapState; }
            }

            public override void Awake()
            {
                mapState = MapState.None;
                base.Awake();
                FullArchiveTempDirectoryPath = Path.Combine(Application.dataPath, archiveTempDirectoryName);
            }

            public override MapBlock<WorldNode, IReadOnlyWorldNode> GetBlock(ShortVector2 blockAddress)
            {
                return this.LoadMapBlock<WorldNode, IReadOnlyWorldNode>(blockAddress);
            }

            public override void ReleaseBlock(ShortVector2 blockAddress, MapBlock<WorldNode, IReadOnlyWorldNode> block)
            {
                this.SaveArchiveMapBlock(blockAddress, block);
            }

            /// <summary>
            /// 更新地图的数据;
            /// </summary>
            public void OnMapUpdate(IntVector2 mapPoint)
            {
                ShortVector2 address;
                lock (thisLock)
                {
                    if (NeedToUpdate(mapPoint, out address) || mapState == MapState.None)
                    {
                        mapState = MapState.Loading;
                        WaitCallback waitCallback = delegate
                        {
                            UpdateBlocks(mapPoint);
                            mapState = MapState.None;
                        };
                        ThreadPool.QueueUserWorkItem(waitCallback);
                    }
                }
            }

            /// <summary>
            /// 将地图保存到缓存地图文件夹内;
            /// </summary>
            public void OnSave()
            {
                lock (thisLock)
                {
                    mapState = MapState.Saving;
                    KeyValuePair<ShortVector2, MapBlock<WorldNode, IReadOnlyWorldNode>>[] pairs = MapCollection.ToArray();
                    foreach (var pair in pairs)
                    {
                        this.SaveArchiveMapBlock(pair.Key, pair.Value);
                    }
                    mapState = MapState.None;
                }
            }


        }

    }

}
