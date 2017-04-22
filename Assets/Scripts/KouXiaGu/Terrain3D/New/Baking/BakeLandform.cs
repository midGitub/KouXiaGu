using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘焙;
    /// </summary>
    [Serializable]
    class BakeLandform
    {
        BakeLandform()
        {
        }

        [SerializeField]
        BakeMeshPool meshPool;
        [SerializeField]
        Shader diffuseShader;
        [SerializeField]
        Shader heightShader;
        Material diffuseMaterial;
        Material heightMaterial;

        public void Initialise()
        {
            diffuseMaterial = new Material(diffuseShader);
            heightMaterial = new Material(heightShader);
        }

        [Serializable]
        class BakeMeshPool
        {
            [SerializeField]
            MeshRenderer prefab;
            [SerializeField]
            int maxCapacity = 100;

            /// <summary>
            /// 中心点,根据传入坐标转换到此中心点;
            /// </summary>
            [SerializeField]
            CubicHexCoord createdCenter;

            public void Initialise()
            {

            }
        }

    }

}
