using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    public interface ILandformWatcher
    {
        void UpdateDispaly(LandformBuilder scene);
    }

    /// <summary>
    /// 场景创建请求管理,负责对场景地形块的创建和销毁进行管理;
    /// </summary>
    public class LandformBuilder
    {
        static LandformBuilder()
        {
            watcherList = new List<ILandformWatcher>();
            readOnlyWatcherList = watcherList.AsReadOnlyCollection();
        }

        static readonly List<ILandformWatcher> watcherList;
        static readonly IReadOnlyCollection<ILandformWatcher> readOnlyWatcherList;

        public static IReadOnlyCollection<ILandformWatcher> WatcherList
        {
            get { return readOnlyWatcherList; }
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



        public LandformBuilder(LandformManager builder)
        {
            this.builder = builder;
            createCoords = new Dictionary<RectCoord, BakeTargets>();
            destroyCoords = new List<RectCoord>();
        }

        readonly LandformManager builder;
        readonly Dictionary<RectCoord, BakeTargets> createCoords;
        readonly List<RectCoord> destroyCoords;

        IReadOnlyDictionary<RectCoord, ChunkBakeRequest> sceneDisplayedChunks
        {
            get { return builder.SceneDisplayedChunks; }
        }

        IEnumerable<RectCoord> sceneCoords
        {
            get { return sceneDisplayedChunks.Keys; }
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

        public void SendDisplay()
        {
            UpdateDispalyCoords();

            ICollection<RectCoord> needDestroyCoords = GetNeedDestroyCoords();
            foreach (var coord in needDestroyCoords)
            {
                this.builder.Destroy(coord);
            }

            IDictionary<RectCoord, BakeTargets> needCreateCoords = GetNeedCreateCoords();
            foreach (var item in needCreateCoords)
            {
                this.builder.Create(item.Key, item.Value);
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
    }

}
