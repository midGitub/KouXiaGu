using KouXiaGu.Grids;
using KouXiaGu.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{


    public interface IBuildingWatcher
    {
        IEnumerable<RectCoord> GetDispaly();
    }

    public class BuildingManager
    {
        static BuildingManager()
        {
            watcherList = new List<IBuildingWatcher>();
            readOnlyWatcherList = watcherList.AsReadOnlyCollection();
        }

        public BuildingManager(IWorldData worldData, LandformManager landform)
        {
            builder = new BuildingBuilder(worldData, landform);
        }

        static readonly List<IBuildingWatcher> watcherList;
        static readonly IReadOnlyCollection<IBuildingWatcher> readOnlyWatcherList;
        readonly BuildingBuilder builder;
        readonly HashSet<RectCoord> createCoords;
        readonly List<RectCoord> destroyCoords;

        public static IReadOnlyCollection<IBuildingWatcher> WatcherList
        {
            get { return readOnlyWatcherList; }
        }

        public BuildingBuilder Builder
        {
            get { return builder; }
        }

        IEnumerable<RectCoord> sceneCoords
        {
            get { return builder.SceneChunks.Keys; }
        }

        void SendDisplay()
        {
            UpdateDispalyCoords();

            IEnumerable<RectCoord> needDestroyCoords = GetNeedDestroyCoords();
            foreach (var coord in needDestroyCoords)
            {
                this.builder.Destroy(coord);
            }

            IEnumerable<RectCoord> needCreateCoords = GetNeedCreateCoords();
            foreach (var coord in needCreateCoords)
            {
                this.builder.Create(coord);
            }

            createCoords.Clear();
            destroyCoords.Clear();
        }


        IEnumerable<RectCoord> GetNeedDestroyCoords()
        {
            foreach (var coord in sceneCoords)
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
        class BuildRequestUpdater : IUnityThreadBehaviour<Action>
        {
            public BuildRequestUpdater(BuildingManager manager)
            {
                Manager = manager;
                this.SubscribeUpdate();
            }

            readonly BuildingManager Manager;

            object IUnityThreadBehaviour<Action>.Sender
            {
                get { return "场景的地形块创建销毁管理"; }
            }

            Action IUnityThreadBehaviour<Action>.Action
            {
                get { return Manager.SendDisplay; }
            }

        }

        void UpdateDispalyCoords()
        {
            foreach (var watcher in watcherList)
            {
                var displayChunkCoords = watcher.GetDispaly();
                createCoords.IntersectWith(displayChunkCoords);
            }
        }
    }

}
