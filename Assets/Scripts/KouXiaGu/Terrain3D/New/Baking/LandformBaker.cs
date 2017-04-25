using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [Serializable]
    public class LandformBaker
    {
        /// <summary>
        /// 透明的黑色颜色;
        /// </summary>
        public static readonly Color BlackTransparent = new Color(0, 0, 0, 0);

        /// <summary>
        /// 地平线颜色;
        /// </summary>
        public static readonly Color Horizon = new Color(0.5f, 0.5f, 0.5f, 1);



        LandformBaker()
        {
        }

        [SerializeField]
        BakeLandform landform;

        public BakeLandform Landform
        {
            get { return landform; }
        }

        IEnumerator Bake(IWorldData worldData, CubicHexCoord chunkCenter, ChunkTexture texture)
        {
            yield return landform.BakeCoroutine(worldData, chunkCenter);
            var heightMapRT = landform.HeightRT;
            var diffuseMapRT = landform.DiffuseRT;

            var diffuseMap = LandformBakeManager.GetDiffuseTexture(diffuseMapRT);
            var heightMap = LandformBakeManager.GetHeightTexture(diffuseMapRT);

            texture.SetDiffuseMap(diffuseMap);
            texture.SetHeightMap(heightMap);

            landform.Reset();
            yield break;
        }

        public void Reset()
        {
            landform.ResetAll();
        }

    }

}
