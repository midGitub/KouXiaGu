using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain
{


    public interface ILandform
    {

        /// <summary>
        /// 地形唯一标示;
        /// </summary>
        int ID { get; }

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        Texture DiffuseTexture { get; }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        Texture HeightTexture { get; }

        /// <summary>
        /// 混合贴图;
        /// </summary>
        Texture MixerTexture { get; }

    }

}
