using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块创建管理;
    /// </summary>
    public class LandformBuilder
    {
        public LandformBuilder(IWorldData worldData)
        {
            baker = LandformBaker.Initialize(worldData);
            chunkPool = new ChunkPool();
            sceneChunks = new Dictionary<RectCoord, IAsyncOperation<Chunk>>();
            readOnlySceneChunks = sceneChunks.AsReadOnlyDictionary();
        }

        readonly LandformBaker baker;
        readonly ChunkPool chunkPool;
        readonly Dictionary<RectCoord, IAsyncOperation<Chunk>> sceneChunks;
        readonly IReadOnlyDictionary<RectCoord, IAsyncOperation<Chunk>> readOnlySceneChunks;

        public IReadOnlyDictionary<RectCoord, IAsyncOperation<Chunk>> SceneChunks
        {
            get { return readOnlySceneChunks; }
        }

        public IAsyncOperation<Chunk> Create()
        {
            throw new NotImplementedException();
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        class BuildRequest : AsyncOperation<Chunk>, IBakeRequest
        {
            public BuildRequest(RectCoord chunkCoord, Chunk chunk)
            {
                ChunkCoord = chunkCoord;
                Result = chunk;
            }

            public RectCoord ChunkCoord { get; private set; }

            public Chunk Chunk
            {
                get { return Result; }
            }

            ChunkTexture IBakeRequest.Textures
            {
                get { return Chunk.Renderer; }
            }

            void IBakeRequest.OnCompleted()
            {
                OnCompleted(Chunk);
            }

            void IBakeRequest.OnFaulted(Exception ex)
            {
                OnFaulted(ex);
            }
        }

    }

}
