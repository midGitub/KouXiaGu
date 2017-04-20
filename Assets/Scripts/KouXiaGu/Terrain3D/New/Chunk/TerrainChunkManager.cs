using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形块管理;
    /// </summary>
    public class TerrainChunkManager
    {
        public TerrainChunkManager()
        {
        }

        
        public void Create(Vector3 position, TerrainChunkTexture textures)
        {
            TerrainChunk chunk = TerrainChunk.Create(textures);
            chunk.transform.position = position;
        }

    }


    public class TerrainChunkPool : ObjectPool<TerrainChunk>
    {
        protected override void Destroy(TerrainChunk item)
        {
            throw new NotImplementedException();
        }

        protected override TerrainChunk Instantiate()
        {
            throw new NotImplementedException();
        }

        protected override void Reset(TerrainChunk item)
        {
            throw new NotImplementedException();
        }
    }

}
