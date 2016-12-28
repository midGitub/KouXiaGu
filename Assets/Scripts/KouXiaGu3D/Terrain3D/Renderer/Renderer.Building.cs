using System;
using System.Collections;
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
        /// 获取到对地形进行改变的参数;
        /// </summary>
        [Serializable]
        class BuildingDecorate
        {

            [SerializeField]
            MeshDisplay displayMeshPool;

            /// <summary>
            /// 高度微调;
            /// </summary>
            [SerializeField]
            Shader heightAdjustShader;

            Material heightAdjustMaterial;

            /// <summary>
            /// 高度调整图;
            /// </summary>
            public RenderTexture HeightAdjustRT { get; private set; }
            /// <summary>
            /// 漫反射调整图;
            /// </summary>
            public RenderTexture DiffuseAdjustRT { get; private set; }

            /// <summary>
            /// 最终的高度图;
            /// </summary>
            public RenderTexture HeightMap { get; private set; }
            /// <summary>
            /// 最终的漫反射图;
            /// </summary>
            public RenderTexture DiffuseMap { get; private set; }

            public void Awake()
            {
                displayMeshPool.Awake();

                heightAdjustMaterial = new Material(heightAdjustShader);
            }

            /// <summary>
            /// 烘焙需要的调整图;
            /// </summary>
            public IEnumerator Rander(IBakeRequest request, IEnumerable<BakingNode> bakingNodes)
            {
                //List<KeyValuePair<BakingNode, MeshRenderer>> displayMeshs = GetDisplayMeshs(request, bakingNodes);

                //SetBakingCamera(displayMeshPool);

                //RanderHeightAdjust(displayMeshs);

                //HeightAdjustRT.SavePNG(@"Assets\TestTex");

                yield break;
            }


            static readonly Color HeightRanderBackgroundColor = new Color(0.5f, 0.5f, 0.5f);

            void RanderHeightAdjust(IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> displayMeshs)
            {
                foreach (var pair in displayMeshs)
                {
                    BakingNode node = pair.Key;
                    MeshRenderer mesh = pair.Value;

                    if (mesh.material != null)
                        GameObject.Destroy(mesh.material);

                    mesh.material = heightAdjustMaterial;
                }

                HeightAdjustRT = RenderTexture.GetTemporary(Parameter.rHeightMapWidth, Parameter.rHeightMapHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
                CameraRender(HeightAdjustRT, HeightRanderBackgroundColor);
            }

            /// <summary>
            /// 混合到高度贴图和漫反射贴图;
            /// </summary>
            public IEnumerator Blend(Texture heightMap, Texture diffuseMap)
            {
                
                throw new NotImplementedException();
            }

            /// <summary>
            /// 释放所有渲染贴图类;
            /// </summary>
            public void ReleaseAll()
            {

            }

            /// <summary>
            /// 获取到需要显示到场景的内容和网格;
            /// </summary>
            public List<KeyValuePair<BakingNode, MeshRenderer>> GetDisplayMeshs(IBakeRequest request, IEnumerable<BakingNode> bakingNodes)
            {
                List<KeyValuePair<BakingNode, MeshRenderer>> list = new List<KeyValuePair<BakingNode, MeshRenderer>>();
                CubicHexCoord center = TerrainChunk.GetHexCenter(request.ChunkCoord);

                displayMeshPool.RecoveryActive();

                foreach (var bakingNode in bakingNodes)
                {
                    CubicHexCoord crood = bakingNode.Position;
                    var mesh = displayMeshPool.Dequeue(crood, center, 0);
                    list.Add(new KeyValuePair<BakingNode, MeshRenderer>(bakingNode, mesh));
                }

                return list;
            }

            /// <summary>
            /// 高度调整图烘焙;
            /// </summary>
            public RenderTexture HeightAdjustMapRender(Texture heightMap)
            {
                throw new NotImplementedException();
            }

        }

    }

}
