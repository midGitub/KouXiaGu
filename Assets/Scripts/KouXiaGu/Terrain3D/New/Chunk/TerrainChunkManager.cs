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

}
