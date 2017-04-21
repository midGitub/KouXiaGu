using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形控制;
    /// </summary>
    public class Landform
    {
        public static IAsyncOperation<Landform> Initialize(IWorldScene world)
        {
            return new AsyncInitializer(world);
        }

        class AsyncInitializer : AsyncOperation<Landform>
        {
            public AsyncInitializer(IWorldData worldData)
            {
                var instance = new Landform();
                instance.WorldData = worldData;
                instance.TerrainChunk = new LandformChunkManager();
                OnCompleted(instance);
            }
        }


        Landform()
        {
        }

        public IWorldData WorldData { get; private set; }
        public LandformChunkManager TerrainChunk { get; private set; }

        public IDictionary<CubicHexCoord, MapNode> Map
        {
            get { return WorldData.Map.Data; }
        }

    }

}
