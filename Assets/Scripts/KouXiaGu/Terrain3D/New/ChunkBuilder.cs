using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World.Map;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    class ChunkBuilder : AsyncOperation<Chunk>
    {
        public static ChunkBuilder Create(RectCoord chunkCoord, ChunkManager chunk)
        {
            return new ChunkBuilder(chunkCoord);
        }


        ChunkBuilder(RectCoord chunkCoord)
        {
            ChunkCoord = chunkCoord;
        }

        public RectCoord ChunkCoord { get; private set; }

        /// <summary>
        /// 取消创建;
        /// </summary>
        public void Cancel()
        {
            if (IsCompleted)
                throw new ArgumentException();

            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            ChunkBuilder item = obj as ChunkBuilder;
            if (item == null)
                return false;
            return item.ChunkCoord == ChunkCoord;
        }

        public override int GetHashCode()
        {
            return ChunkCoord.GetHashCode();
        }
    }


    //public class ChunkBuildManager
    //{
    //    public ChunkBuildManager()
    //    {
    //        buildingChunks = new Dictionary<RectCoord, ChunkBuilder>();
    //        TerrainChunk = new ChunkManager();
    //    }

    //    Dictionary<RectCoord, ChunkBuilder> buildingChunks;
    //    public ChunkManager TerrainChunk { get; private set; }

    //    public IAsyncOperation<Chunk> Create(RectCoord chunkCoord)
    //    {
    //        ChunkBuilder builder;

    //        if (!buildingChunks.TryGetValue(chunkCoord, out builder))
    //        {
    //            //builder = ChunkBuilder.Create(chunkCoord);
    //            buildingChunks.Add(chunkCoord, builder);
    //            throw new NotImplementedException();
    //        }

    //        return builder;
    //    }

    //    class MapObserver : IDictionaryObserver<CubicHexCoord, MapNode>
    //    {
    //        void IDictionaryObserver<CubicHexCoord, MapNode>.OnAdded(CubicHexCoord key, MapNode newValue)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        void IDictionaryObserver<CubicHexCoord, MapNode>.OnRemoved(CubicHexCoord key, MapNode originalValue)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        void IDictionaryObserver<CubicHexCoord, MapNode>.OnUpdated(CubicHexCoord key, MapNode originalValue, MapNode newValue)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }
    //}

}
