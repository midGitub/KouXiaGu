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

        public BakeCoordTransform _coordTransform;
        public BakeMeshPool _meshPool;
        public Shader _diffuseShader;
        public Shader _heightShader;
        Material diffuseMaterial;
        Material heightMaterial;

        public void Initialise()
        {
            _meshPool.Initialise();
            diffuseMaterial = new Material(_diffuseShader);
            heightMaterial = new Material(_heightShader);
        }

        [Serializable]
        public class BakeMeshPool
        {
            BakeMeshPool()
            {
            }

            public MeshRenderer prefab;
            public int maxCapacity = 100;
            GameObjectPool<MeshRenderer> objectPool;

            public void Initialise()
            {
                objectPool = new GameObjectPool<MeshRenderer>(prefab, maxCapacity);
            }

        }

    }

}
