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

        public static Landform Initialize(IWorldData worldData)
        {
            return null;
        }


        Landform()
        {
        }

        IWorldData worldData;
        ChunkSceneManager chunkManager;

        public ChunkSceneManager ChunkManager
        {
            get { return chunkManager; }
        }

        void Awake()
        {
            chunkManager = new ChunkSceneManager();
        }

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            RectCoord chunkCoord = ChunkInfo.ChunkGrid.GetCoord(position);
            Chunk chunk;
            if (ChunkManager.InSceneChunks.TryGetValue(chunkCoord, out chunk))
            {
                Vector2 uv = ChunkInfo.ChunkGrid.GetUV(chunkCoord, position);
                return chunk.Renderer.GetHeight(uv);
            }
            return 0;
        }

    }

}
