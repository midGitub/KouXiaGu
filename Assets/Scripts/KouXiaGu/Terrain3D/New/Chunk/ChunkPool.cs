using System;
using System.Collections.Generic;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块池;
    /// </summary>
    public class ChunkPool : ObjectPool<Chunk>
    {
        public ChunkPool()
        {
        }

        public Chunk Get(Vector3 position, ChunkTexture textures)
        {
            Chunk chunk = Get();
            Set(chunk, position, textures);
            return chunk;
        }        

        void Set(Chunk chunk, Vector3 position, ChunkTexture textures)
        {
            chunk.transform.position = position;
            chunk.Texture.SetTextures(textures);
        }

        public override Chunk Instantiate()
        {
            Chunk chunk = Chunk.Create();
            return chunk;
        }

        public override void Reset(Chunk chunk)
        {
            chunk.Clear();
        }

        public override void Destroy(Chunk chunk)
        {
            chunk.Destroy();
        }
    }

}
