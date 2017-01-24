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
        LandformRenderer terrainChunk;

        public void Bake(IBakeRequest request)
        {
            Vector3 pos = LandformChunk.ChunkGrid.GetCenter(request.ChunkCoord);
            LandformRenderer mesh = GameObject.Instantiate(terrainChunk, pos, Quaternion.identity);
            mesh.gameObject.SetActive(true);

            //var rt = BakeCamera.GetHeightTemporaryRender();
            //BakeCamera.CameraRender(rt, new Grids.CubicHexCoord(), Color.black);

            //var texture = BakeCamera.GetHeightTexture(rt);
            //mesh.SetDiffuseMap(texture);

            var rt = BakeCamera.GetDiffuseTemporaryRender();
            BakeCamera.CameraRender(rt, new Grids.CubicHexCoord(), Color.black);

            var texture = BakeCamera.GetDiffuseTexture(rt);
            mesh.SetDiffuseMap(texture);
        }

    }

}
