using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public sealed partial class Renderer : UnitySingleton<Renderer>
    {

        /// <summary>
        /// 在地形上加入道路;
        /// </summary>
        [Serializable]
        class RoadDecorate
        {

            public RenderTexture Render(RenderTexture heightMap)
            {
                throw new NotImplementedException();
            }

        }

    }

}
