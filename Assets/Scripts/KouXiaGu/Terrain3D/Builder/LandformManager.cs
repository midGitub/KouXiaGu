using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World;

namespace KouXiaGu.Terrain3D
{

    public interface ILandformWatcher
    {
        void UpdateDispaly(LandformManager scene);
    }

    /// <summary>
    /// 场景创建请求管理;统一创建和销毁请求;
    /// </summary>
    public class LandformManager
    {
        static LandformManager()
        {
            watcherList = new List<ILandformWatcher>();
            readOnlyWatcherList = watcherList.AsReadOnlyCollection();
        }

        public LandformManager(IWorldData worldData)
        {
            manager = new LandformBuilder(worldData);
            updater = new BuildRequestUpdater(this);
            createCoords = new Dictionary<RectCoord, BakeTargets>();
            destroyCoords = new List<RectCoord>();
        }

        static readonly List<ILandformWatcher> watcherList;
        static readonly IReadOnlyCollection<ILandformWatcher> readOnlyWatcherList;
        readonly LandformBuilder manager;
        readonly BuildRequestUpdater updater;
        readonly Dictionary<RectCoord, BakeTargets> createCoords;
        readonly List<RectCoord> destroyCoords;

        public static IReadOnlyCollection<ILandformWatcher> WatcherList
        {
            get { return readOnlyWatcherList; }
        }

        public LandformBuilder Manager
        {
            get { return manager; }
        }

        IReadOnlyDictionary<RectCoord, ChunkBakeRequest> sceneDisplayedChunks
        {
            get { return manager.SceneDisplayedChunks; }
        }

        IEnumerable<RectCoord> sceneCoords
        {
            get { return sceneDisplayedChunks.Keys; }
        }

        public static void AddLandformWatcher(ILandformWatcher watcher)
        {
            if (watcherList.Contains(watcher))
                throw new ArgumentException();

            watcherList.Add(watcher);
        }

        public static bool RemoveLandformWatcher(ILandformWatcher watcher)
        {
            return watcherList.Remove(watcher);
        }

        public void Display(RectCoord chunkCoord, BakeTargets targets)
        {
            if (createCoords.ContainsKey(chunkCoord))
            {
                createCoords[chunkCoord] |= targets;
            }
            else
            {
                createCoords.Add(chunkCoord, targets);
            }
        }

        void SendDisplay()
        {
            UpdateDispalyCoords();

            ICollection<RectCoord> needDestroyCoords = GetNeedDestroyCoords();
            foreach (var coord in needDestroyCoords)
            {
                this.manager.Destroy(coord);
            }

            IDictionary<RectCoord, BakeTargets> needCreateCoords = GetNeedCreateCoords();
            foreach (var item in needCreateCoords)
            {
                this.manager.Create(item.Key, item.Value);
            }

            createCoords.Clear();
            destroyCoords.Clear();
        }

        void UpdateDispalyCoords()
        {
            foreach (var item in watcherList)
            {
                item.UpdateDispaly(this);
            }
        }

        ICollection<RectCoord> GetNeedDestroyCoords()
        {
            foreach (var coord in sceneCoords)
            {
                if (!createCoords.ContainsKey(coord))
                    destroyCoords.Add(coord);
            }
            return destroyCoords;
        }

        IDictionary<RectCoord, BakeTargets> GetNeedCreateCoords()
        {
            return createCoords;
        }

        /// <summary>
        /// 对场景创建管理进行更新;
        /// </summary>
        class BuildRequestUpdater : IUnityThreadBehaviour<Action>
        {
            public BuildRequestUpdater(LandformManager buildRequestManager)
            {
                this.buildRequestManager = buildRequestManager;
                this.SubscribeUpdate();
            }

            readonly LandformManager buildRequestManager;

            object IUnityThreadBehaviour<Action>.Sender
            {
                get { return "场景的地形块创建销毁管理"; }
            }

            Action IUnityThreadBehaviour<Action>.Action
            {
                get { return buildRequestManager.SendDisplay; }
            }

        }
    }

}
