using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    public interface IChunkWatcher
    {
        IEnumerable<RectCoord> GetDispaly();
    }

    public abstract class ChunkWatcherUpdater
    {
        public ChunkWatcherUpdater()
        {
            createCoords = new HashSet<RectCoord>();
            destroyCoords = new List<RectCoord>();
        }

        readonly HashSet<RectCoord> createCoords;
        readonly List<RectCoord> destroyCoords;
        IDisposable updaterDisposer;

        public bool IsUpdating
        {
            get { return updaterDisposer != null; }
        }

        protected abstract IEnumerable<IChunkWatcher> Watchers { get; }
        protected abstract IEnumerable<RectCoord> SceneCoords { get; }
        protected abstract void CreateAt(RectCoord coord);
        protected abstract void DestroyAt(RectCoord coord);

        public void StartUpdate()
        {
            if (updaterDisposer == null)
            {
                updaterDisposer = new UnityThreadBehaviour(this).SubscribeUpdate();
            }
        }

        public void StopUpdate()
        {
            if (updaterDisposer != null)
            {
                updaterDisposer.Dispose();
                updaterDisposer = null;
            }
        }

        void SendDisplay()
        {
            UpdateDispalyCoords();

            IEnumerable<RectCoord> needDestroyCoords = GetNeedDestroyCoords();
            foreach (var coord in needDestroyCoords)
            {
                DestroyAt(coord);
            }

            IEnumerable<RectCoord> needCreateCoords = GetNeedCreateCoords();
            foreach (var coord in needCreateCoords)
            {
                CreateAt(coord);
            }

            createCoords.Clear();
            destroyCoords.Clear();
        }

        void UpdateDispalyCoords()
        {
            foreach (var watcher in Watchers)
            {
                var displayChunkCoords = watcher.GetDispaly();
                createCoords.IntersectWith(displayChunkCoords);
            }
        }

        IEnumerable<RectCoord> GetNeedDestroyCoords()
        {
            foreach (var coord in SceneCoords)
            {
                if (!createCoords.Contains(coord))
                    destroyCoords.Add(coord);
            }
            return destroyCoords;
        }

        IEnumerable<RectCoord> GetNeedCreateCoords()
        {
            return createCoords;
        }

        /// <summary>
        /// 对场景创建管理进行更新;
        /// </summary>
        class UnityThreadBehaviour : IUnityThreadBehaviour<Action>
        {
            public UnityThreadBehaviour(ChunkWatcherUpdater manager)
            {
                Manager = manager;
                this.SubscribeUpdate();
            }

            readonly ChunkWatcherUpdater Manager;

            object IUnityThreadBehaviour<Action>.Sender
            {
                get { return "场景的地形块创建销毁管理"; }
            }

            Action IUnityThreadBehaviour<Action>.Action
            {
                get { return Manager.SendDisplay; }
            }
        }
    }

}
