using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    public sealed partial class Renderer : UnitySington<Renderer>
    {

        /// <summary>
        /// 漫反射贴图烘焙;
        /// </summary>
        [Serializable]
        class DiffuseTex
        {

            [SerializeField]
            Shader diffuse;

            Material _diffuseMaterial;

            Material diffuseMaterial
            {
                get { return _diffuseMaterial ?? (_diffuseMaterial = new Material(diffuse)); }
            }

            /// <summary>
            /// 烘焙材质贴图;
            /// </summary>
            public RenderTexture Baking(IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes)
            {
                foreach (var pair in bakingNodes)
                {
                    BakingNode node = pair.Key;
                    MeshRenderer hexMesh = pair.Value;

                    if (hexMesh.material != null)
                        GameObject.Destroy(hexMesh.material);

                    hexMesh.material = diffuseMaterial;

                    hexMesh.material.SetTexture("_MainTex", node.Landform.DiffuseTex);
                    hexMesh.material.SetTexture("_BlendTex", node.Landform.DiffuseBlendTex);
                }

                RenderTexture diffuseRT = RenderTexture.GetTemporary(Parameter.rDiffuseTexWidth, Parameter.rDiffuseTexHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
                CameraRender(diffuseRT);
                return diffuseRT;
            }

            public Texture2D GetTexture(RenderTexture renderTexture)
            {
                RenderTexture.active = renderTexture;
                Texture2D diffuse = new Texture2D(Parameter.DiffuseTexWidth, Parameter.DiffuseTexHeight, TextureFormat.RGB24, false);
                diffuse.ReadPixels(Parameter.DiffuseReadPixel, 0, 0, false);
                diffuse.wrapMode = TextureWrapMode.Clamp;
                diffuse.Apply();

                return diffuse;
            }

        }

    }

}
