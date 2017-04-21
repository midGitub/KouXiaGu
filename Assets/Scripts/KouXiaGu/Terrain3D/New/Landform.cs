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
                instance.LandformChunk = new ChunkManager();
                OnCompleted(instance);
            }
        }


        Landform()
        {
        }

        public IWorldData WorldData { get; private set; }
        public ChunkManager LandformChunk { get; private set; }

        public IDictionary<CubicHexCoord, MapNode> Map
        {
            get { return WorldData.Map.Data; }
        }

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            RectCoord chunkCoord = ChunkInfo.ChunkGrid.GetCoord(position);
            Chunk chunk;
            if (LandformChunk.ActivatedChunks.TryGetValue(chunkCoord, out chunk))
            {
                Vector2 uv = ChunkInfo.ChunkGrid.GetUV(chunkCoord, position);
                return chunk.Texture.GetHeight(uv);
            }
            return 0;
        }

    }

}
