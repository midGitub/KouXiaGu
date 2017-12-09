using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using JiongXiaGu.Grids;
using JiongXiaGu.Unity;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形烘培
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class LandformBakeDrawingBoardRenderer : MonoBehaviour
    {
        private LandformBakeDrawingBoardRenderer()
        {
        }

        [SerializeField]
        private Shader displayInBakeDrawingBoardShader = null;
        private MeshRenderer meshRenderer;
        private Material meterial;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = meterial = new Material(displayInBakeDrawingBoardShader);
        }

        public void SetValue(Vector3 pos, Quaternion rotation)
        {
            transform.position = pos;
            transform.rotation = rotation;
        }

        public void DisplayDiffuse(LandformRes landformResource)
        {
            if (landformResource == null)
                throw new ArgumentNullException("landformResource");

            meterial.SetTexture("_MainTex", landformResource.DiffuseTex);
            meterial.SetTexture("_BlendTex", landformResource.DiffuseBlendTex);
        }

        public void DisplayHeight(LandformRes landformResource)
        {
            if (landformResource == null)
                throw new ArgumentNullException("landformResource");

            meterial.SetTexture("_MainTex", landformResource.HeightTex);
            meterial.SetTexture("_BlendTex", landformResource.HeightBlendTex);
        }
    }
}
