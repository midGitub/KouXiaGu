using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public interface ISingtonBaker : IDisposable
    {
        IDisposable GetAndLock(out LandformBaker baker);
    }


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
        IEnumerator<ChunkTexture> Bake()
        {
            yield break;
        }

        public void Reset()
        {
            landform.ReleaseAll();
        }
    }

}
