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

    public class LandformManager : ChunkWatcherUpdater
    {
        static LandformManager()
        {
            watcherList = new List<ILandformWatcher>();
        }

        static readonly List<ILandformWatcher> watcherList;

        public static List<ILandformWatcher> WatcherList
        {
            get { return watcherList; }
        }


        public LandformManager(IWorldData worldData)
        {
            builder = new LandformBuilder(worldData);
        }

        readonly LandformBuilder builder;

        public LandformBuilder Builder
        {
            get { return builder; }
        }

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
            get { return builder.SceneCoords; }
        }

        protected override void CreateAt(RectCoord coord)
        {
            builder.Create(coord);
        }

        protected override void DestroyAt(RectCoord coord)
        {
            builder.Destroy(coord);
        }
    }
}
