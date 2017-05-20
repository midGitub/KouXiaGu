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

    public class BuildingManager : ChunkWatcherUpdater
    {
        static BuildingManager()
        {
            watcherList = new List<IBuildingWatcher>();
        }

        static readonly List<IBuildingWatcher> watcherList;

        public static List<IBuildingWatcher> WatcherList
        {
            get { return watcherList; }
        }


        public BuildingManager(IWorldData worldData, LandformBuilder landform)
        {
            builder = new BuildingBuilder(worldData, landform);
        }

        readonly BuildingBuilder builder;

        public BuildingBuilder Builder
        {
            get { return builder; }
        }

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
            get { return builder.SceneChunks; }
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
