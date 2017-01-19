using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [Serializable]
    public class LandformBaker : GameObjectPool<MeshRenderer>, IDisposable
    {

        [SerializeField]
        MeshRenderer prefab;

        /// <summary>
        /// 中心点,根据传入坐标位置转换到此中心点附近;
        /// </summary>
        [SerializeField]
        CubicHexCoord center;

        /// <summary>
        /// 目标中心点;
        /// </summary>
        CubicHexCoord targetCenter;

        List<Pack> inSceneMeshs;


        [SerializeField]
        Shader diffuseShader;

        [SerializeField]
        Shader heightShader;

        Material diffuseMaterial;
        Material heightMaterial;

        public RenderTexture DiffuseRT { get; private set; }
        public RenderTexture HeightRT { get; private set; }

        IEnumerable<Pack> Renderers
        {
            get { return inSceneMeshs; }
        }

        CubicHexCoord Center
        {
            get { return center; }
        }

        BakingParameter Parameter
        {
            get { return TerrainBaker.Parameter; }
        }


        public void Initialise()
        {
            inSceneMeshs = new List<Pack>();

            diffuseMaterial = new Material(diffuseShader);
            heightMaterial = new Material(heightShader);
        }


        public void Bake(IBakeRequest request, IEnumerable<CubicHexCoord> points)
        {
            PrepareScene(request, points);
            DiffuseRT = BakeDiffuse();
            HeightRT = BakeHeight();
        }


        public void PrepareScene(IBakeRequest request, IEnumerable<CubicHexCoord> displays)
        {
            SetTargetCenter(request);
            ClearInSceneMeshs();

            foreach (var display in displays)
            {
                LandformNode info;
                if (request.Data.Landform.TryGetValue(display, out info))
                {
                    var renderer = Get(display, info.Angle);
                    LandformRes res = GetLandform(info.ID);
                    inSceneMeshs.Add(new Pack(res, renderer));
                }
            }
        }

        void SetTargetCenter(IBakeRequest request)
        {
            this.targetCenter = TerrainChunk.ChunkGrid.GetCenter(request.ChunkCoord).GetTerrainCubic();
        }

        void ClearInSceneMeshs()
        {
            foreach (var roadMesh in inSceneMeshs)
            {
                Release(roadMesh.Rednerer);
            }
            inSceneMeshs.Clear();
        }

        MeshRenderer Get(CubicHexCoord coord, float angle)
        {
            MeshRenderer item = Get();
            item.transform.position = PositionConvert(coord);
            item.transform.rotation = Quaternion.Euler(0, angle, 0);
            return item;
        }

        Vector3 PositionConvert(CubicHexCoord terget)
        {
            CubicHexCoord coord = terget - targetCenter;
            return (coord + this.center).GetTerrainPixel();
        }

        /// <summary>
        /// 获取到地貌信息;
        /// </summary>
        LandformRes GetLandform(int id)
        {
            try
            {
                return LandformRes.initializedInstances[id];
            }
            catch (KeyNotFoundException ex)
            {
                //Debug.LogError("缺少材质资源;ID: " + id + ex.ToString());
                throw new LackOfResourcesException("缺少材质资源;ID: " + id, ex);
            }
        }

        protected override MeshRenderer Create()
        {
            var item = GameObject.Instantiate(prefab, Parent);
            item.gameObject.SetActive(true);
            return item;
        }


        /// <summary>
        /// 完全透明颜色;
        /// </summary>
        static readonly Color Transparent = new Color(0, 0, 0, 0);

        RenderTexture BakeDiffuse()
        {
            foreach (var meshRenderer in Renderers)
            {
                SetDiffuserMaterial(meshRenderer);
            }

            RenderTexture rt = RenderTexture.GetTemporary(Parameter.rDiffuseTexWidth, Parameter.rDiffuseTexHeight, 24);
            TerrainBaker.CameraRender(rt, Center, Transparent);
            return rt;
        }

        void SetDiffuserMaterial(Pack renderer)
        {
            LandformRes res = renderer.Res;

            diffuseMaterial.SetTexture("_MainTex", res.DiffuseTex);
            diffuseMaterial.SetTexture("_BlendTex", res.DiffuseBlendTex);

            renderer.Rednerer.material = diffuseMaterial;
        }


        RenderTexture BakeHeight()
        {
            foreach (var meshRenderer in Renderers)
            {
                SetHeightMaterial(meshRenderer);
            }

            RenderTexture rt = RenderTexture.GetTemporary(Parameter.rHeightMapWidth, Parameter.rHeightMapHeight, 24);
            TerrainBaker.CameraRender(rt, Center);
            return rt;
        }

        void SetHeightMaterial(Pack renderer)
        {
            LandformRes res = renderer.Res;

            heightMaterial.SetTexture("_MainTex", res.HeightTex);
            heightMaterial.SetTexture("_BlendTex", res.HeightBlendTex);

            renderer.Rednerer.material = heightMaterial;
        }


        public void Dispose()
        {
            RenderTexture.ReleaseTemporary(DiffuseRT);
            RenderTexture.ReleaseTemporary(HeightRT);

            DiffuseRT = null;
            HeightRT = null;
        }

        struct Pack
        {
            public Pack(LandformRes res, MeshRenderer rednerer)
            {
                this.Res = res;
                this.Rednerer = rednerer;
            }

            public LandformRes Res { get; private set; }
            public MeshRenderer Rednerer { get; private set; }
        }

    }

}
