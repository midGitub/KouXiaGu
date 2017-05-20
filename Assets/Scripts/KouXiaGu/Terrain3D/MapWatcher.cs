﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 监视地图变化;
    /// </summary>
    public class MapWatcher : IDictionaryObserver<CubicHexCoord, MapNode>
    {
        public MapWatcher(LandformBuilder builder, IObservableDictionary<CubicHexCoord, MapNode> observable)
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

        readonly List<RectCoord> chunkCoordList = new List<RectCoord>();

        IEnumerable<RectCoord> GetBakeChunks(CubicHexCoord coord)
        {
            chunkCoordList.Clear();
            foreach (var checkCoord in coord.GetNeighbours())
            {
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