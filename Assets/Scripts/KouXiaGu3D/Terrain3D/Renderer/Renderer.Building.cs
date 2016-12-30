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
            /// 高度调整;
            /// </summary>
            [SerializeField]
            Shader heightAdjustInMesh;

            Material heightAdjustMaterial;

            /// <summary>
            /// 高度调整;
            /// </summary>
            public RenderTexture HeightAdjustRT { get; private set; }
            /// <summary>
            /// 区域平滑;
            /// </summary>
            public RenderTexture RegionSmoothRT { get; private set; }
            /// <summary>
            /// 漫反射调整;
            /// </summary>
            public RenderTexture DiffuseAdjustRT { get; private set; }

            public void Awake()
            {
                displayMeshPool.Awake();

                heightAdjustMaterial = new Material(heightAdjustInMesh);
            }

            /// <summary>
            /// 烘焙需要的调整图;
            /// 地形提升图 -> 平滑区域图 -> 漫反射贴图; 
            /// </summary>
            public IEnumerator Rander(IBakeRequest request, IEnumerable<CubicHexCoord> bakingPoints)
            {
                //List<KeyValuePair<BakingNode, MeshRenderer>> displayMeshs = GetDisplayMeshs(request, bakingNodes);

                //SetBakingCamera(displayMeshPool);

                //RanderHeightAdjust(displayMeshs);

                //HeightAdjustRT.SavePNG(@"Assets\TestTex");

                yield break;
            }

            //IEnumerable<KeyValuePair<RoadNode, MeshRenderer>



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
                CameraRender(HeightAdjustRT);
            }

            /// <summary>
            /// 准备好烘焙高度平整的场景;
            /// </summary>
            IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> PrepareMeshInScene(IBakeRequest request, IEnumerable<BakingNode> bakingNodes)
            {


                yield break;
            }

            /// <summary>
            /// 准备建筑物内容;
            /// </summary>
            void PrepareBuildingMesh(CubicHexCoord center, IEnumerable<BakingNode> bakingNodes)
            {

            }

            /// <summary>
            /// 将道路布置到场景
            /// </summary>
            void PrepareRoadMesh(CubicHexCoord center, IEnumerable<BakingNode> bakingNodes)
            {

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
