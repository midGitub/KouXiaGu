using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 用于测试烘培;
    /// </summary>
    [Serializable]
    public class TestBaker
    {

        [SerializeField]
        MeshRenderer terrainChunk;

        public void Bake(IBakeRequest request)
        {
            Vector3 pos = TerrainChunk.ChunkGrid.GetCenter(request.ChunkCoord);
            GameObject.Instantiate(terrainChunk, pos, Quaternion.identity);
        }

    }

}
