using System;
using System.Collections.Generic;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 需要摄像机烘焙贴图的抽象类;
    /// </summary>
    public abstract class RenderBase<T> : IDisposable
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

        public RenderTexture DiffuseRT { get; private set; }
        public RenderTexture HeightRT { get; private set; }

        protected static BakingParameter Parameter
        {
            get { return TerrainBaker.Parameter; }
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
        public void Render(IBakeRequest request, IEnumerable<CubicHexCoord> bakingPoints)
        {
            tempCenter = TerrainMesh.GetHexCenter(request.ChunkCoord);

            List<KeyValuePair<T, MeshRenderer>> meshs = InitMeshs(request.Data.Data, bakingPoints);

            HeightRT = RenderHeight(meshs);
            DiffuseRT = RenderDiffuse(meshs);
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
            CameraRenderHeight(rt, displayMeshPool);
            return rt;
        }

        /// <summary>
        /// 使用摄像机烘焙高度图;
        /// </summary>
        protected virtual void CameraRenderHeight(RenderTexture rt, MeshDisplay display)
        {
            TerrainBaker.CameraRender(rt, displayMeshPool);
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
            CameraRenderDiffuse(rt, displayMeshPool);
            return rt;
        }

        /// <summary>
        /// 使用摄像机烘焙漫反射图;
        /// </summary>
        protected virtual void CameraRenderDiffuse(RenderTexture rt, MeshDisplay display)
        {
            TerrainBaker.CameraRender(rt, displayMeshPool);
        }

        public Texture2D GetHeightTexture()
        {
            return TerrainBaker.GetHeightTexture(HeightRT);
        }

        public Texture2D GetDiffuseTexture()
        {
            return TerrainBaker.GetDiffuseTexture(DiffuseRT);
        }

        /// <summary>
        /// 释放资源引用;
        /// </summary>
        public void Dispose()
        {
            RenderTexture.ReleaseTemporary(DiffuseRT);
            RenderTexture.ReleaseTemporary(HeightRT);

            DiffuseRT = null;
            HeightRT = null;
        }

    }

}

