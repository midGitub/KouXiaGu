using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 监视地图变化;
    /// </summary>
    public class WorldMapWatcher : IDictionaryObserver<CubicHexCoord, MapNode>
    {
        public WorldMapWatcher(LandformBuilder builder, IObservableDictionary<CubicHexCoord, MapNode> observable)
        {
            this.builder = builder;
            Subscribe(observable);
        }

        IDisposable unsubscriber;
        readonly LandformBuilder builder;

        public void Subscribe(IObservableDictionary<CubicHexCoord, MapNode> observable)
        {
            if (unsubscriber != null)
                throw new ArgumentException();

            unsubscriber = observable.Subscribe(this);
        }

        public void Unsubscribe()
        {
            if (unsubscriber != null)
            {
                unsubscriber.Dispose();
                unsubscriber = null;
            }
        }

        void IDictionaryObserver<CubicHexCoord, MapNode>.OnAdded(CubicHexCoord key, MapNode newValue)
        {
            UpdateChunks(key, BakeTargets.All);
        }

        void IDictionaryObserver<CubicHexCoord, MapNode>.OnRemoved(CubicHexCoord key, MapNode originalValue)
        {
            UpdateChunks(key, BakeTargets.All);
        }

        void IDictionaryObserver<CubicHexCoord, MapNode>.OnUpdated(CubicHexCoord key, MapNode originalValue, MapNode newValue)
        {
            BakeTargets targets = BakeTargets.None;

            if (originalValue.Road != newValue.Road)
            {
                targets |= BakeTargets.Road;
            }

            if (targets != BakeTargets.None)
            {
                UpdateChunks(key, targets);
            }
        }

        void UpdateChunks(CubicHexCoord coord, BakeTargets targets)
        {
            var belongChunks = GetBakeChunks(coord);
            foreach (var belongChunk in belongChunks)
            {
                builder.Update(belongChunk, targets);
            }
        }

        static readonly CubicHexCoord[] checkDirections = new CubicHexCoord[]
            {
                //CubicHexCoord.Self,
                CubicHexCoord.DIR_North,
                //CubicHexCoord.DIR_South,
                //CubicHexCoord.DIR_Northeast,
                //CubicHexCoord.DIR_Northwest,
                CubicHexCoord.DIR_Southeast,
                CubicHexCoord.DIR_Southwest,
            };

        readonly List<RectCoord> chunkCoordList = new List<RectCoord>();

        IEnumerable<RectCoord> GetBakeChunks(CubicHexCoord coord)
        {
            chunkCoordList.Clear();
            foreach (var direction in checkDirections)
            {
                CubicHexCoord checkCoord = coord + direction;
                var chunkCoords = ChunkInfo.GetBelongChunks(checkCoord);

                foreach (var chunkCoord in chunkCoords)
                {
                    if (!chunkCoordList.Contains(chunkCoord))
                        chunkCoordList.Add(chunkCoord);
                }
            }
            return chunkCoordList;
        }

    }

}
