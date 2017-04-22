using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘培;
    /// </summary>
    [Serializable]
    public class LandformBaker
    {

        [SerializeField]
        BakeLandform landform;

        [SerializeField]
        RoadBaker road;

        [SerializeField]
        DecorateBlend decorateBlend;

        public IEnumerator GetBakeCoroutine(RectCoord chunkCoord)
        {
            throw new NotImplementedException();
        }

    }

}
