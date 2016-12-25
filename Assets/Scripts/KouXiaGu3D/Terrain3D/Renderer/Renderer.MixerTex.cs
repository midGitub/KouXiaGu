using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    public sealed partial class Renderer : UnitySingleton<Renderer>
    {

        [Serializable]
        class MixerTex
        {
            [SerializeField]
            Shader mixer;

            Material _mixerMaterial;

            Material mixerMaterial
            {
                get { return _mixerMaterial ?? (_mixerMaterial = new Material(mixer)); }
            }

            /// <summary>
            /// 烘焙混合图;
            /// </summary>
            public RenderTexture Baking(IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes)
            {
                foreach (var pair in bakingNodes)
                {
                    BakingNode node = pair.Key;
                    MeshRenderer hexMesh = pair.Value;

                    if (hexMesh.material != null)
                        GameObject.Destroy(hexMesh.material);

                    hexMesh.material = mixerMaterial;
                    hexMesh.material.mainTexture = node.MixerTex;
                }

                RenderTexture mixerRT = RenderTexture.GetTemporary(Parameter.rDiffuseTexWidth, Parameter.rDiffuseTexHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
                Render(mixerRT);
                return mixerRT;
            }

        }


    }

}
