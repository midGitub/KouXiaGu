using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路烘焙;
    /// </summary>
    [Serializable]
    public class RoadBaker : ReusableObjectPool<RoadMesh>, IDisposable
    {

        [SerializeField]
        RoadMesh prefab;

        [SerializeField, Range(4, 60)]
        int segmentPoints = 16;

        [SerializeField, Range(0.01f, 2)]
        float roadWidth = 0.07f;

        /// <summary>
        /// 中心点,根据传入坐标位置转换到此中心点附近;
        /// </summary>
        [SerializeField]
        CubicHexCoord center;

        /// <summary>
        /// 目标中心点;
        /// </summary>
        CubicHexCoord targetCenter;

        /// <summary>
        /// 在场景中的网格;
        /// </summary>
        List<RoadMesh> inSceneMeshs;

        [SerializeField]
        Shader diffuseShader;

        [SerializeField]
        Shader heightShader;

        Material diffuseMaterial;
        Material heightMaterial;

        public RenderTexture DiffuseRT { get; private set; }
        public RenderTexture HeightRT { get; private set; }

        static BakingParameter Parameter
        {
            get { return TerrainBaker.Parameter; }
        }

        IEnumerable<MeshRenderer> Renderers
        {
            get { return inSceneMeshs.Select(item => item.MeshRenderer); }
        }

        CubicHexCoord Center
        {
            get { return center; }
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialise()
        {
            inSceneMeshs = new List<RoadMesh>();

            var res = RoadRes.initializedInstances[1];
            if (diffuseMaterial == null)
            {
                diffuseMaterial = new Material(diffuseShader);

                diffuseMaterial.SetTexture("_MainTex", res.DiffuseTex);
                diffuseMaterial.SetTexture("_BlendTex", res.DiffuseBlendTex);
            }
            if (heightMaterial == null)
            {
                heightMaterial = new Material(heightShader);

                heightMaterial.SetTexture("_MainTex", res.HeightAdjustTex);
            }
        }

        public void OnDestroy()
        {
            Clear();
        }


        public IEnumerator Bake(IBakeRequest request, IEnumerable<CubicHexCoord> points)
        {
            PrepareScene(request, points);
            DiffuseRT = BakeDiffuse();
            HeightRT = BakeHeight();
            yield break;
        }


        /// <summary>
        /// 准备场景;
        /// </summary>
        public void PrepareScene(IBakeRequest request, IEnumerable<CubicHexCoord> displays)
        {
            SetTargetCenter(request);
            ClearInSceneMeshs();

            foreach (var display in displays)
            {
                var meshs = Create(request.Data.Road, display);
                inSceneMeshs.AddRange(meshs);
            }
        }

        void SetTargetCenter(IBakeRequest request)
        {
            this.targetCenter = TerrainMesh.ChunkGrid.GetCenter(request.ChunkCoord).GetTerrainCubic();
        }

        void ClearInSceneMeshs()
        {
            foreach (var roadMesh in inSceneMeshs)
            {
                Release(roadMesh);
            }
            inSceneMeshs.Clear();
        }

        /// <summary>
        /// 创建目标点之上的道路;
        /// </summary>
        IEnumerable<RoadMesh> Create(Road road, CubicHexCoord target)
        {
            var paths = road.FindPaths(target);

            foreach (var path in paths)
            {
                Vector3[] pixelPath = PointConvertPixel(path);

                RoadMesh roadMesh = Get();
                roadMesh.SetPath(pixelPath, segmentPoints, roadWidth);

                yield return roadMesh;
            }
        }

        /// <summary>
        /// 转换目标点到相应位置;
        /// </summary>
        Vector3[] PointConvertPixel(CubicHexCoord[] path)
        {
            Vector3[] newPath = new Vector3[path.Length];

            for (int i = 0; i < path.Length; i++)
            {
                newPath[i] = PointConvertPixel(path[i]);
            }

            return newPath;
        }

        /// <summary>
        /// 转换目标点到相应位置;
        /// </summary>
        Vector3 PointConvertPixel(CubicHexCoord target)
        {
            CubicHexCoord coord = target - targetCenter;
            return (coord + center).GetTerrainPixel();
        }

        protected override RoadMesh Create()
        {
            var item = GameObject.Instantiate(prefab, Parent);
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

        void SetDiffuserMaterial(MeshRenderer renderer)
        {
            renderer.sharedMaterial = diffuseMaterial;
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

        void SetHeightMaterial(MeshRenderer renderer)
        {
            renderer.sharedMaterial = heightMaterial;
        }


        public void Dispose()
        {
            RenderTexture.ReleaseTemporary(DiffuseRT);
            RenderTexture.ReleaseTemporary(HeightRT);

            DiffuseRT = null;
            HeightRT = null;
        }

    }

}
