using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using JiongXiaGu.Grids;
using JiongXiaGu.World;
using JiongXiaGu.RectTerrain.Resources;

namespace JiongXiaGu.RectTerrain
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
        Shader displayInBakeDrawingBoardShader = null;
        MeshRenderer meshRenderer;
        Material meterial;
        public LandformResource landformResource { get; private set; }

        void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = meterial = new Material(displayInBakeDrawingBoardShader);
        }

        public void Initialize(Vector3 pos, Quaternion rotation, LandformResource resource)
        {
            transform.position = pos;
            transform.rotation = rotation;
            landformResource = resource;
        }

        public void Reset()
        {
            landformResource = null;
        }

        public void DisplayDiffuse()
        {
            if (landformResource == null)
                throw new ArgumentNullException("landformResource");

            meterial.SetTexture("_MainTex", landformResource.DiffuseTex);
            meterial.SetTexture("_BlendTex", landformResource.DiffuseBlendTex);
        }

        public void DisplayHeight()
        {
            if (landformResource == null)
                throw new ArgumentNullException("landformResource");

            meterial.SetTexture("_MainTex", landformResource.HeightTex);
            meterial.SetTexture("_BlendTex", landformResource.HeightBlendTex);
        }
    }
}
