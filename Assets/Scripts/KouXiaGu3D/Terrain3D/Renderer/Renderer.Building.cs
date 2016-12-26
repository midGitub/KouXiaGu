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
        /// 建筑物(道路 建筑地形 之类的),
        /// 平整地形;
        /// </summary>
        class BuildingDecorate
        {

            /// <summary>
            /// 建筑混合高度平整图;
            /// </summary>
            public RenderTexture HeightMapRender()
            {
                throw new NotImplementedException();
            }

        }

    }

}
