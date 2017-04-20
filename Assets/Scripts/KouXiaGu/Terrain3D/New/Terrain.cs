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
    public class Terrain
    {

        public static IAsyncOperation<Terrain> Initialize(IWorld world)
        {
            return new AsyncInitializer();
        }

        class AsyncInitializer : AsyncOperation<Terrain>
        {
            public AsyncInitializer()
            {
                throw new NotImplementedException();
            }
        }


        Terrain()
        {
        }

        public IDictionary<CubicHexCoord, MapNode> Map { get; private set; }
        public TerrainChunkManager TerrainChunk { get; private set; }

        public static float GetHeight(Vector3 pos)
        {
            throw new NotImplementedException();
        }

    }

}
