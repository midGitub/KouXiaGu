using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Grids;
using JiongXiaGu.World.Map;
using JiongXiaGu.World;

namespace JiongXiaGu.Terrain3D
{

    /// <summary>
    /// 监视地图变化;
    /// </summary>
    class MapWatcher : IDictionaryObserver<CubicHexCoord, MapNode>
    {
        public MapWatcher(OLandformBuilder landformBuilder, OBuildingBuilder buildingBuilder, IObservableDictionary<CubicHexCoord, MapNode> observable)
        {
            this.landformBuilder = landformBuilder;
            this.buildingBuilder = buildingBuilder;
            Subscribe(observable);
        }

        IDisposable unsubscriber;
        readonly OLandformBuilder landformBuilder;
        readonly OBuildingBuilder buildingBuilder;

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
            UpdateLandformChunks(key, BakeTargets.All);
            UpdateBuilding(key, newValue.Building);
        }

        void IDictionaryObserver<CubicHexCoord, MapNode>.OnRemoved(CubicHexCoord key, MapNode originalValue)
        {
            UpdateLandformChunks(key, BakeTargets.All);
            UpdateBuilding(key, default(BuildingNode));
        }

        void IDictionaryObserver<CubicHexCoord, MapNode>.OnUpdated(CubicHexCoord key, MapNode originalValue, MapNode newValue)
        {
            BakeTargets targets = BakeTargets.None;
            if (originalValue.Landform != newValue.Landform)
            {
                targets |= BakeTargets.Landform;
            }
            if (originalValue.Road != newValue.Road)
            {
                targets |= BakeTargets.Road;
            }
            if (targets != BakeTargets.None)
            {
                UpdateLandformChunks(key, targets);
            }

            if (originalValue.Building != newValue.Building)
            {
                UpdateBuilding(key, newValue.Building);
            }
        }

        void IDictionaryObserver<CubicHexCoord, MapNode>.OnClear(IDictionary<CubicHexCoord, MapNode> dictionary)
        {
        }

        void UpdateLandformChunks(CubicHexCoord coord, BakeTargets targets)
        {
            var belongChunks = GetBakeChunks(coord);
            foreach (var belongChunk in belongChunks)
            {
                landformBuilder.UpdateAsync(belongChunk, targets);
            }
        }

        readonly List<RectCoord> chunkCoordList = new List<RectCoord>();

        IEnumerable<RectCoord> GetBakeChunks(CubicHexCoord coord)
        {
            chunkCoordList.Clear();
            foreach (var checkCoord in coord.GetNeighbours())
            {
                var chunkCoords = LandformChunkInfo.GetBelongChunks(checkCoord);

                foreach (var chunkCoord in chunkCoords)
                {
                    if (!chunkCoordList.Contains(chunkCoord))
                        chunkCoordList.Add(chunkCoord);
                }
            }
            return chunkCoordList;
        }

        void UpdateBuilding(CubicHexCoord position, BuildingNode node)
        {
            buildingBuilder.UpdateAt(position, node);
        }
    }
}
