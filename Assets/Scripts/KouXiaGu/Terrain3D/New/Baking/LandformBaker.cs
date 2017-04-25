using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [Serializable]
    public class LandformBaker
    {
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
        IEnumerator Bake(CubicHexCoord chunkCenter, ChunkTexture texture)
        {
            yield break;
        }

        public void Reset()
        {
            landform.ReleaseAll();
        }
    }

}
