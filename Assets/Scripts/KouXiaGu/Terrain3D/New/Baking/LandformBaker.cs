using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void Reset()
        {
            landform.Reset();
        }

    }

}
