﻿using System;
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
        TerrainRenderer terrainChunk;

        public void Bake(IBakeRequest request)
        {
            Vector3 pos = TerrainChunk.ChunkGrid.GetCenter(request.ChunkCoord);
            TerrainRenderer mesh = GameObject.Instantiate(terrainChunk, pos, Quaternion.identity);
            mesh.gameObject.SetActive(true);

            var rt = BakeCamera.GetHeightTemporaryRender();
            BakeCamera.CameraRender(rt, new Grids.CubicHexCoord(), Color.black);

            var texture = BakeCamera.GetHeightTexture(rt);
            mesh.SetDiffuseMap(texture);
        }

    }

}