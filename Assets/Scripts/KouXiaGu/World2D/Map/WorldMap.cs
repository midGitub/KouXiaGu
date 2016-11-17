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
          BuildGameData item, ICancelable cancelable, Action<Exception> onError, Action onComplete)
        {
            try
            {
                string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
                string fullPrefabMapDirectory = item.ArchivedData.Archived.World2D.PathPrefabMapDirectory;
                worldDynMap.OnGameStart(fullArchivedDirectoryPath, fullPrefabMapDirectory);
            }
            catch (Exception e)
            {
                onError(e);
            }
            onComplete();
        }

        void IThreadInit<ArchivedGroup>.Initialize(
            ArchivedGroup item, ICancelable cancelable, Action<Exception> onError, Action onComplete)
        {
            try
            {
                string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
                item.Archived.World2D.PathPrefabMapDirectory = worldDynMap.OnGameArchive(fullArchivedDirectoryPath);
            }
            catch (Exception e)
            {
                onError(e);
            }
            onComplete();
        }

        void IThreadInit<Unit>.Initialize(
            Unit item, ICancelable cancelable, Action<Exception> onError, Action onComplete)
        {
            try
            {
                worldDynMap.OnGameQuit();
            }
            catch (Exception e)
            {
                onError(e);
            }
            onComplete();
        }

        #region 协程初始化;

        IEnumerator ICoroutineInit<BuildGameData>.Initialize(
            BuildGameData item, ICancelable cancelable, Action<Exception> onError, Action onComplete)
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
            Unit item, ICancelable cancelable, Action<Exception> onError, Action onComplete)
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
        /// 为了监视面板显示而继承;
        /// </summary>
        [Serializable]
        public sealed class WorldDynBlockMapEx : DynBlockMapGeneric<WorldNode, IReadOnlyWorldNode>,
            IMapBlockInfo
        {
            private WorldDynBlockMapEx() { }
        }

    }

}
