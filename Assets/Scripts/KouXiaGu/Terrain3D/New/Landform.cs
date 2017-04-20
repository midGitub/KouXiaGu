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
        public static IAsyncOperation<Landform> Initialize(IWorld world)
        {
            return new AsyncInitializer(world);
        }

        class AsyncInitializer : AsyncOperation<Landform>
        {
            public AsyncInitializer(IWorld world)
            {
                var instance = new Landform();
                instance.World = world;
                instance.TerrainChunk = new TerrainChunkManager();
                OnCompleted(instance);
            }
        }


        Landform()
        {
        }

        public IWorld World { get; private set; }
        public TerrainChunkManager TerrainChunk { get; private set; }

        public IDictionary<CubicHexCoord, MapNode> Map
        {
            get { return World.Map.Data; }
        }

    }

}
