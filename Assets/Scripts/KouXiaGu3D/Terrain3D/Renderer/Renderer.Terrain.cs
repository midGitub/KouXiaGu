using System;
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
        /// 地形烘焙;
        /// </summary>
        [Serializable]
        public class TerrainRender : IDisposable
        {
            TerrainRender() { }

            [SerializeField]
            MeshDisplay displayMeshPool;

            [SerializeField]
            Shader heightShader;

            [SerializeField]
            Shader diffuseShader;

            public RenderTexture DiffuseAdjustRT { get; private set; }

            public RenderTexture HeightAdjustRT { get; private set; }

            public void Awake()
            {
                displayMeshPool.Awake();
            }

            public void Rander(IBakeRequest request, IEnumerable<CubicHexCoord> bakingPoints)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                RenderTexture.ReleaseTemporary(DiffuseAdjustRT);
                RenderTexture.ReleaseTemporary(HeightAdjustRT);

                DiffuseAdjustRT = null;
                HeightAdjustRT = null;
            }

        }

    }

}
