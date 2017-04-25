using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        /// <summary>
        /// 
        /// </summary>
        IEnumerator Bake(IWorldData worldData, CubicHexCoord chunkCenter, ChunkTexture texture)
        {
            yield return landform.BakeCoroutine(worldData, chunkCenter);
        }

        public void Reset()
        {
            landform.ReleaseAll();
        }
    }

}
