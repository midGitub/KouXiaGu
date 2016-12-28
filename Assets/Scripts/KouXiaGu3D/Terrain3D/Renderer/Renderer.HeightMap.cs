using System;
using System.Collections.Generic;
using KouXiaGu.ImageEffects;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public sealed partial class Renderer : UnitySington<Renderer>
    {

        /// <summary>
        /// 高度图烘焙;
        /// </summary>
        [Serializable]
        class HeightRenderer
        {
            HeightRenderer() { }

            [SerializeField]
            Shader heightShader;

            [SerializeField]
            bool isBlur = true;
            [SerializeField, Range(0, 10)]
            float blurSize = 3;
            /// <summary>
            /// 降低分辨率;
            /// </summary>
            const int downsample = 0;
            [SerializeField, Range(1, 6)]
            int blurIterations = 1;

            Material _heightMaterial;

            Material heightMaterial
            {
                get { return _heightMaterial ?? (_heightMaterial = new Material(heightShader)); }
            }

            /// <summary>
            /// 烘焙高度图;
            /// </summary>
            /// <param name="bakingNodes">烘焙的节点和对应的网格;</param>
            /// <returns>高度图结果;</returns>
            public RenderTexture Baking(IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes)
            {
                foreach (var pair in bakingNodes)
                {
                    BakingNode node = pair.Key;
                    MeshRenderer hexMesh = pair.Value;

                    if (hexMesh.material != null)
                        GameObject.Destroy(hexMesh.material);

                    hexMesh.material = heightMaterial;
                    hexMesh.material.SetTexture("_MainTex", node.Landform.HeightTex);
                    hexMesh.material.SetTexture("_BlendTex", node.Landform.HeightBlendTex);
                }

                RenderTexture heightRT = RenderTexture.GetTemporary(Parameter.rHeightMapWidth, Parameter.rHeightMapHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
                CameraRender(heightRT);

                if (isBlur)
                {
                    var blueHeightRT = ImageEffect.BlurOptimized(heightRT, blurSize, downsample, blurIterations, ImageEffect.BlurType.StandardGauss);
                    RenderTexture.ReleaseTemporary(heightRT);
                    heightRT = blueHeightRT;
                }

                return heightRT;
            }

            /// <summary>
            /// 获取到高度图;
            /// </summary>
            public Texture2D GetTexture(RenderTexture rt)
            {
                RenderTexture.active = rt;
                Texture2D heightMap = new Texture2D(Parameter.HeightMapWidth, Parameter.HeightMapHeight, TextureFormat.ARGB32, false);
                heightMap.ReadPixels(Parameter.HeightReadPixel, 0, 0, false);
                heightMap.wrapMode = TextureWrapMode.Clamp;
                heightMap.Apply();

                return heightMap;
            }

            public void Clear()
            {
                if (_heightMaterial != null)
                    Destroy(_heightMaterial);
            }

        }


    }

}
