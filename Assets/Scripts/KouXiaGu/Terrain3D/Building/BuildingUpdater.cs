using KouXiaGu.Grids;
using KouXiaGu.World;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.Terrain3D
{
    public interface IBuildingWatcher : IChunkWatcher<RectCoord>
    {
    }

    class BuildingUpdater : ChunkUpdater<RectCoord>
    {
        static BuildingUpdater()
        {
            WatcherList = new List<IBuildingWatcher>();
        }

        public BuildingUpdater(OBuildingBuilder builder)
        {
            Builder = builder;
        }

        internal OBuildingBuilder Builder { get; private set; }
        public static List<IBuildingWatcher> WatcherList { get; private set; }

        protected override IEnumerable<IChunkWatcher<RectCoord>> Watchers
        {
            get { return WatcherList.Cast<IChunkWatcher<RectCoord>>(); }
        }

        protected override IEnumerable<RectCoord> SceneCoords
        {
            get { return Builder.BuildingCollection.SceneChunks; }
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
