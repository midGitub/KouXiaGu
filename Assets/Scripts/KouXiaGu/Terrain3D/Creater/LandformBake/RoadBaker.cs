using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;
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


        public void Bake(IBakeRequest request, IEnumerable<CubicHexCoord> points)
        {
            PrepareScene(request, points);
            BakeDiffuse();
            BakeHeight();
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
                var meshs = Create(request.Data, display);
                inSceneMeshs.AddRange(meshs);
            }
        }

        void SetTargetCenter(IBakeRequest request)
        {
            this.targetCenter = OLandformChunk.ChunkGrid.GetCenter(request.ChunkCoord).GetTerrainCubic();
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
        IEnumerable<RoadMesh> Create(IDictionary<CubicHexCoord, MapNode> map, CubicHexCoord target)
        {
            var paths = map.FindPaths(target);

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
            return (coord + center).GetTerrainPixel(-inSceneMeshs.Count);
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

        void BakeDiffuse()
        {
            foreach (var meshRenderer in Renderers)
            {
                SetDiffuserMaterial(meshRenderer);
            }

            DiffuseRT = LandformBakeManager.GetDiffuseTemporaryRender();
            LandformBakeManager.CameraRender(DiffuseRT, Center, Transparent);
        }

        void SetDiffuserMaterial(MeshRenderer renderer)
        {
            renderer.sharedMaterial = diffuseMaterial;
        }



        void BakeHeight()
        {
            foreach (var meshRenderer in Renderers)
            {
                SetHeightMaterial(meshRenderer);
            }

            HeightRT = LandformBakeManager.GetHeightTemporaryRender();
            LandformBakeManager.CameraRender(HeightRT, Center);
        }

        void SetHeightMaterial(MeshRenderer renderer)
        {
            renderer.sharedMaterial = heightMaterial;
        }


        public void Dispose()
        {
            LandformBakeManager.ReleaseTemporary(DiffuseRT);
            LandformBakeManager.ReleaseTemporary(HeightRT);

            DiffuseRT = null;
            HeightRT = null;
        }

    }

}
