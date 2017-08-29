using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using KouXiaGu.World;
using KouXiaGu.Concurrent;

namespace KouXiaGu.Terrain3D
{

    public interface ILandformWatcher : IChunkWatcher<RectCoord>
    {
    }

    public class OLandformUpdater : ChunkUpdater<RectCoord>
    {
        static OLandformUpdater()
        {
            WatcherList = new List<ILandformWatcher>();
        }

        internal OLandformUpdater(OLandformBuilder builder)
        {
            Builder = builder;
        }

        internal OLandformBuilder Builder { get; private set; }
        public static List<ILandformWatcher> WatcherList { get; private set; }

        protected override IEnumerable<RectCoord> SceneCoords
        {
            get { return Builder.SceneChunks.Keys; }
        }

        protected override IEnumerable<IChunkWatcher<RectCoord>> Watchers
        {
            get { return WatcherList.OfType<IChunkWatcher<RectCoord>>(); }
        }

        protected override void CreateAt(RectCoord coord)
        {
            Builder.CreateAsync(coord);
        }

        protected override void DestroyAt(RectCoord coord)
        {
            Builder.DestroyAsync(coord);
        }
    }
}
