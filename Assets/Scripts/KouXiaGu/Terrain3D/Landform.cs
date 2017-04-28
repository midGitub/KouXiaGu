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
    public class Landform : MonoBehaviour
    {
        Landform()
        {
        }

        public bool IsInitialized { get; private set; }
        public LandformBuilder Builder { get; private set; }
        public LandformScene Scene { get; private set; }

        public Landform Initialize(IWorldData worldData)
        {
            if (!IsInitialized)
            {
                Builder = new LandformBuilder(worldData);
                Scene = new LandformScene(Builder);
            }
            return this;
        }

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            RectCoord chunkCoord = ChunkInfo.ChunkGrid.GetCoord(position);
            IAsyncOperation<Chunk> chunk;
            if (Builder.SceneDisplayedChunks.TryGetValue(chunkCoord, out chunk))
            {
                Vector2 uv = ChunkInfo.ChunkGrid.GetUV(chunkCoord, position);
                return chunk.Result.Renderer.GetHeight(uv);
            }
            return 0;
        }

    }

}
