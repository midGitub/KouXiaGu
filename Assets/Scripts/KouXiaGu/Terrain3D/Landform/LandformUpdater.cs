using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using KouXiaGu.World;

namespace KouXiaGu.Terrain3D
{

    public interface ILandformWatcher : IChunkWatcher
    {
    }

    public class LandformUpdater : ChunkWatcherUpdater
    {
        static LandformUpdater()
        {
            watcherList = new List<ILandformWatcher>();
        }

        static readonly List<ILandformWatcher> watcherList;

        public static List<ILandformWatcher> WatcherList
        {
            get { return watcherList; }
        }


        public LandformUpdater()
        {
        }

        public IWorld World { get; private set; }
        public LandformBuilder Builder { get; private set; }

        protected override object Sender
        {
            get { return "场景的地形块创建和销毁管理"; }
        }

        protected override IEnumerable<IChunkWatcher> Watchers
        {
            get { return watcherList.Cast<IChunkWatcher>(); }
        }

        protected override IEnumerable<RectCoord> SceneCoords
        {
            get { return Builder.SceneCoords; }
        }

        public void StartUpdate(IWorld world)
        {
            World = world;
            StartUpdate();
        }

        protected override void CreateAt(RectCoord coord)
        {
            Builder.Create(coord);
        }

        protected override void DestroyAt(RectCoord coord)
        {
            Builder.Destroy(coord);
        }
    }
}
