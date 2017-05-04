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

        void IDictionaryObserver<CubicHexCoord, MapNode>.OnAdded(CubicHexCoord key, MapNode newValue)
        {
            Debug.Log("OnAdded");
        }

        void IDictionaryObserver<CubicHexCoord, MapNode>.OnRemoved(CubicHexCoord key, MapNode originalValue)
        {
            Debug.Log("OnRemoved");
        }

        void IDictionaryObserver<CubicHexCoord, MapNode>.OnUpdated(CubicHexCoord key, MapNode originalValue, MapNode newValue)
        {
            Debug.Log("OnUpdated");
        }

        static readonly CubicHexCoord[] checkDirections = new CubicHexCoord[]
            {
                CubicHexCoord.Self,
                CubicHexCoord.DIR_North,
                CubicHexCoord.DIR_South,
                CubicHexCoord.DIR_Northwest,
                CubicHexCoord.DIR_Southeast,
            };

        readonly List<RectCoord> chunkCoordList = new List<RectCoord>();

        IEnumerable<RectCoord> GetBelongChunks(CubicHexCoord coord)
        {
            chunkCoordList.Clear();
            foreach (var direction in checkDirections)
            {
                CubicHexCoord checkCoord = coord + direction;
                RectCoord chunkCoord = GetBelongChunk(checkCoord);
                chunkCoordList.Add(chunkCoord);
            }
            return chunkCoordList;
        }

        RectCoord GetBelongChunk(CubicHexCoord coord)
        {
            return ChunkInfo.ChunkGrid.GetCoord(coord.GetTerrainPixel());
        }

    }

}
