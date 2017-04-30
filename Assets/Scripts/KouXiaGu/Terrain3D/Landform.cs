﻿using System;
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

        /// <summary>
        /// 初始化,并且在等待地形初始化完毕;
        /// </summary>
        public static IAsyncOperation<Landform> InitializeAsync(IWorldData worldData)
        {
            return new AsyncInitializer(worldData);
        }

        class AsyncInitializer : AsyncOperation<Landform>
        {
            public AsyncInitializer(IWorldData worldData)
            {
                try
                {
                    Result = SceneObject.GetObject<Landform>();
                    Result.Initialize(worldData);
                    OnCompleted();
                }
                catch (Exception ex)
                {
                    OnFaulted(ex);
                }
            }

            public override bool IsCompleted
            {
                get { return isFaulted || Result.Baker.IsEmpty; }
            }

        }


        Landform()
        {
        }

        public bool IsInitialized { get; private set; }
        public LandformBuilder Builder { get; private set; }
        public LandformBaker Baker { get; private set; }
        public LandformScene Scene { get; private set; }

        Landform Initialize(IWorldData worldData)
        {
            if (!IsInitialized)
            {
                Builder = new LandformBuilder(worldData);
                Baker = Builder.Baker;
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
