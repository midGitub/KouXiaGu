using System;
using System.Collections.Generic;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    public sealed partial class Renderer : UnitySington<Renderer>
    {

        /// <summary>
        /// 装饰类型抽象;
        /// </summary>
        abstract class RenderBase<T> : IDisposable
        {

            [SerializeField]
            MeshDisplay displayMeshPool;

            [SerializeField]
            Shader heightShader;

            [SerializeField]
            Shader diffuseShader;

            Material heightMaterial;
            Material diffuseMaterial;

            /// <summary>
            /// 暂存地形块中心点;
            /// </summary>
            CubicHexCoord tempCenter;

            public RenderTexture DiffuseAdjustRT { get; private set; }
            public RenderTexture HeightAdjustRT { get; private set; }

            protected BakingParameter Parameter
            {
                get { return Renderer.Parameter; }
            }

            /// <summary>
            /// 获取到放置到场景的对应 网格和材质;
            /// </summary>
            protected abstract List<KeyValuePair<T, MeshRenderer>> InitMeshs(IDictionary<CubicHexCoord, TerrainNode> map, IEnumerable<CubicHexCoord> coords);

            /// <summary>
            /// 高度烘焙时设置参数;
            /// </summary>
            protected abstract void SetHeightParameter(Material material, T res);

            /// <summary>
            /// 设置漫反射时的参数;
            /// </summary>
            protected abstract void SetDiffuseParameter(Material material, T res);

            /// <summary>
            /// 回收已经激活在场景的网格;
            /// </summary>
            protected void RecoveryActive()
            {
                displayMeshPool.RecoveryActive();
            }

            /// <summary>
            /// 获取到一个 网格;
            /// </summary>
            protected MeshRenderer DequeueMesh(CubicHexCoord coord, float angle)
            {
                return displayMeshPool.Dequeue(coord, tempCenter, angle);
            }


            public virtual void Awake()
            {
                displayMeshPool.Awake();

                heightMaterial = new Material(heightShader);
                diffuseMaterial = new Material(diffuseShader);
            }

            /// <summary>
            /// 烘焙对应高度调整图和漫反射调整图;
            /// </summary>
            public void Rander(IBakeRequest request, IEnumerable<CubicHexCoord> bakingPoints)
            {
                tempCenter = TerrainChunk.GetHexCenter(request.ChunkCoord);

                List<KeyValuePair<T, MeshRenderer>> meshs = InitMeshs(request.Map, bakingPoints);

                HeightAdjustRT = RenderHeight(meshs);
                DiffuseAdjustRT = RenderDiffuse(meshs);
            }

            /// <summary>
            /// 烘焙高度;
            /// </summary>
            RenderTexture RenderHeight(IEnumerable<KeyValuePair<T, MeshRenderer>> meshs)
            {
                foreach (var pair in meshs)
                {
                    T res = pair.Key;
                    MeshRenderer mesh = pair.Value;

                    if (mesh.material != null)
                        GameObject.Destroy(mesh.material);

                    mesh.material = heightMaterial;
                    SetHeightParameter(mesh.material, res);
                }

                RenderTexture rt = RenderTexture.GetTemporary(Parameter.rHeightMapWidth, Parameter.rHeightMapHeight, 24);
                CameraRender(rt);
                return rt;
            }

            /// <summary>
            /// 烘焙漫反射贴图;
            /// </summary>
            RenderTexture RenderDiffuse(IEnumerable<KeyValuePair<T, MeshRenderer>> meshs)
            {
                foreach (var pair in meshs)
                {
                    T res = pair.Key;
                    MeshRenderer mesh = pair.Value;

                    if (mesh.material != null)
                        GameObject.Destroy(mesh.material);

                    mesh.material = diffuseMaterial;
                    SetDiffuseParameter(mesh.material, res);
                }

                RenderTexture rt = RenderTexture.GetTemporary(Parameter.rDiffuseTexWidth, Parameter.rDiffuseTexHeight, 24);
                CameraRender(rt);
                return rt;
            }

            /// <summary>
            /// 释放资源引用;
            /// </summary>
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

