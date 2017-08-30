using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KouXiaGu.Grids;
using KouXiaGu.World;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 地形烘培
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class LandformBakeDrawingBoardRenderer : MonoBehaviour
    {
        LandformBakeDrawingBoardRenderer()
        {
        }

        [SerializeField]
        Shader diffuseShader = null;
        [SerializeField]
        Shader heightShader = null;

        MeshRenderer meshRenderer;
        Material diffuseMeterial;
        Material heightMeterial;

        void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            diffuseMeterial = new Material(diffuseShader);
            heightMeterial = new Material(heightShader);
        }

        public void DisplayDiffuse(RectCoord nodePos)
        {
            throw new NotImplementedException();
        }

        public void DisplayHeight(RectCoord nodePos)
        {
            throw new NotImplementedException();
        }
    }
}
