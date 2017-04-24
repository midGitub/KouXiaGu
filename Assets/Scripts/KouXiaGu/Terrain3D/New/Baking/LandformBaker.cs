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

        public void Reset()
        {
            landform.Reset();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}
