using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using KouXiaGu.Grids;
using System.Threading;
using KouXiaGu.Concurrent;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形更新;
    /// </summary>
    public class SceneUpdater : AsyncOperation, IDisposable
    {
        public SceneUpdater(IWorld world)
        {
            LandformDispatcher = LandformUnityDispatcher.Instance;
            LandformBuilder = new LandformBuilder(world);
            LandformUpdater = new LandformUpdater(LandformBuilder);
            BuildingBuilder = new BuildingBuilder(world, LandformBuilder, LandformDispatcher);
            BuildingUpdater = new BuildingUpdater(BuildingBuilder);
            MapWatcher = new MapWatcher(LandformUpdater.Builder, BuildingUpdater, world.WorldData.MapData.ObservableMap);
        }

        bool isUpdating;
        bool updateThreadRunning;
        internal IRequestDispatcher LandformDispatcher { get; private set; }
        internal LandformBuilder LandformBuilder { get; private set; }
        internal LandformUpdater LandformUpdater { get; private set; }
        internal BuildingBuilder BuildingBuilder { get; private set; }
        internal BuildingUpdater BuildingUpdater { get; private set; }
        internal MapWatcher MapWatcher { get; private set; }

        /// <summary>
        /// 在这个时刻场景是否构建完成?
        /// </summary>
        public override bool IsCompleted
        {
            get { return isUpdating && LandformUpdater.IsCompleted; }
        }

        public IAsyncOperation Start()
        {
            if (!updateThreadRunning)
            {
                ThreadPool.QueueUserWorkItem(Next);
                updateThreadRunning = true;
            }
            return this;
        }

        public void Stop()
        {
            updateThreadRunning = false;
        }

        void Next(object state)
        {
            try
            {
                while (updateThreadRunning)
                {
                    LandformUpdater.Next();
                    BuildingUpdater.Next();
                    isUpdating = true;
                    Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }

    public interface IChunkWatcher<TKey>
    {
        void UpdateDispaly(ICollection<TKey> dispalyCoords);
    }

    /// <summary>
    /// 线程安全的;
    /// </summary>
    public abstract class ChunkUpdater<TKey>
    {
        public ChunkUpdater()
        {
            dispalyCoords = new HashSet<TKey>();
            destroyCoords = new List<TKey>();
        }

        readonly object asyncLock = new object();
        readonly HashSet<TKey> dispalyCoords;
        readonly List<TKey> destroyCoords;

        protected abstract IEnumerable<TKey> SceneCoords { get; }
        protected abstract IEnumerable<IChunkWatcher<TKey>> Watchers { get; }
        protected abstract void CreateAt(TKey key);
        protected abstract void DestroyAt(TKey key);

        /// <summary>
        /// 更新;
        /// </summary>
        public void Next()
        {
            foreach (var watcher in Watchers)
            {
                watcher.UpdateDispaly(dispalyCoords);
            }

            foreach (var coord in SceneCoords)
            {
                if (!dispalyCoords.Contains(coord))
                {
                    destroyCoords.Add(coord);
                }
                else
                {
                    dispalyCoords.Remove(coord);
                }
            }

            foreach (var coord in destroyCoords)
            {
                DestroyAt(coord);
            }

            foreach (var coord in dispalyCoords)
            {
                CreateAt(coord);
            }

            destroyCoords.Clear();
            dispalyCoords.Clear();
        }
    }
}
