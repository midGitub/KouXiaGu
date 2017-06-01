using KouXiaGu.Grids;
using KouXiaGu.World;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.Terrain3D
{
    public interface IBuildingWatcher : IChunkWatcher
    {
    }

    public class BuildingUpdater : ChunkWatcherUpdater
    {
        static BuildingUpdater()
        {
            watcherList = new List<IBuildingWatcher>();
        }

        static readonly List<IBuildingWatcher> watcherList;

        public static List<IBuildingWatcher> WatcherList
        {
            get { return watcherList; }
        }

        public BuildingUpdater()
        {
        }

        public BuildingBuilder Builder { get; private set; }

        protected override object Sender
        {
            get { return "场景的建筑块创建和销毁管理"; }
        }

        protected override IEnumerable<IChunkWatcher> Watchers
        {
            get { return watcherList.Cast<IChunkWatcher>(); }
        }

        protected override IEnumerable<RectCoord> SceneCoords
        {
            get { return Builder.BuildingCollection.SceneChunks; }
        }

        public void StartUpdate(IWorld world)
        {

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
