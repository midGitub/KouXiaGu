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
            meshPool.Initialise();
            diffuseMaterial = new Material(diffuseShader);
            heightMaterial = new Material(heightShader);
        }

        [Serializable]
        class BakeMeshPool
        {
            BakeMeshPool()
            {
            }

            [SerializeField]
            MeshRenderer prefab;
            [SerializeField]
            int maxCapacity = 100;
            [SerializeField]
            CubicHexCoord createdCenter;

            GameObjectPool<MeshRenderer> objectPool;

            public void Initialise()
            {
                objectPool = new GameObjectPool<MeshRenderer>(prefab, maxCapacity);
            }

        }

    }

}
