using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    public sealed partial class Renderer : UnitySington<Renderer>
    {

        /// <summary>
        /// 装饰类型使用的Shader;
        /// </summary>
        [Serializable]
        class DecorateShader
        {
            /// <summary>
            /// 高度调整;
            /// </summary>
            [SerializeField]
            Shader heightAdjustInMesh;

            Material heightAdjustMaterial;

            public void Awake()
            {
                heightAdjustMaterial = new Material(heightAdjustInMesh);
            }

        }

        /// <summary>
        /// 装饰类型抽象;
        /// </summary>
        interface IDecorate
        {

            /// <summary>
            /// 高度调整(局部调整);
            /// </summary>
            RenderTexture HeightAdjustRT { get; }
            /// <summary>
            /// 漫反射调整;
            /// </summary>
            RenderTexture DiffuseAdjustRT { get; }
            
            /// <summary>
            /// 进行烘焙;
            /// </summary>
            IEnumerator Rander(IBakeRequest request, IEnumerable<CubicHexCoord> bakingPoints);

            void ReleaseAll();
        }

    }


}

