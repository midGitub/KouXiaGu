
//#define TEST_BAKE
//#define TEST_LANDFORM_BAKER

using System;
using System.Collections.Generic;
using UnityEngine;
using KouXiaGu.Grids;
using System.Collections;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 负责将传入的请求渲染出基本的
    /// 高度图\地貌贴图\法线图;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class TerrainBaker : SceneSington<TerrainBaker>
    {
        static TerrainBaker()
        {
            IsInitialised = false;
        }

        /// <summary>
        /// 将要进行烘焙的队列;
        /// </summary>
        static readonly LinkedList<IBakeRequest> bakeRequestQueue = new LinkedList<IBakeRequest>();

        /// <summary>
        /// 烘焙请求队列;
        /// </summary>
        public static LinkedList<IBakeRequest> BakingRequests
        {
            get { return bakeRequestQueue; }
        }

        /// <summary>
        /// 是否已经初始化?
        /// </summary>
        public static bool IsInitialised { get; private set; }

        /// <summary>
        /// 初始化;
        /// </summary>
        public static void Initialize()
        {
            if (!IsInitialised)
            {
                TerrainBaker instance = GetInstance;

                instance.road.Initialise();
                instance.landform.Initialise();

                instance.decorateBlend.Awake();

                instance.StartCoroutine(instance.Bake());

                IsInitialised = true;
            }
        }


        TerrainBaker() { }

        [SerializeField]
        NormalMapper normalMapper;

        [SerializeField]
        LandformBaker landform;

        [SerializeField]
        RoadBaker road;

        [SerializeField]
        DecorateBlend decorateBlend;

#if TEST_BAKE
        [SerializeField]
        TestBaker testBaker;
#endif

        protected override void OnDestroy()
        {
            bakeRequestQueue.Clear();
            IsInitialised = false;
            base.OnDestroy();
        }

        /// <summary>
        /// 在协程内队列中进行烘焙;
        /// 流程:
        /// 建筑高度平整图->建筑地表图->
        /// 地形高度图->地形地表图->
        /// 混合高度->混合地表->
        /// 高度生成法线图->完成
        /// </summary>
        IEnumerator Bake()
        {
            CustomYieldInstruction bakingYieldInstruction = new WaitWhile(() => bakeRequestQueue.Count == 0);

            IBakeRequest request = null;
            RenderTexture normalMapRT = null;
            RenderTexture heightMapRT = null;
            RenderTexture diffuseMapRT = null;

            TerrainTexPack tex = null;

            while (true)
            {
                yield return bakingYieldInstruction;

                try
                {
                    request = bakeRequestQueue.Dequeue();
                    var cover = GetOverlaye(request);

#if TEST_BAKE
                    testBaker.Bake(request);
#else

                    landform.Bake(request, cover);
                    road.Bake(request, cover);

#if TEST_LANDFORM_BAKER
                    heightMapRT = landform.HeightRT;
                    diffuseMapRT = landform.DiffuseRT;
#else
                    heightMapRT = decorateBlend.BlendHeight(landform.HeightRT, road.HeightRT);
                    diffuseMapRT = decorateBlend.BlendDiffuse(landform.DiffuseRT, road.DiffuseRT);
#endif
                    normalMapRT = normalMapper.Rander(heightMapRT);

                    tex = new TerrainTexPack();
                    tex.heightMap = BakeCamera.GetHeightTexture(heightMapRT);
                    tex.normalMap = normalMapper.GetTexture(normalMapRT);
                    tex.diffuseMap = BakeCamera.GetDiffuseTexture(diffuseMapRT);

                    request.OnComplete(tex);
                    tex = null;
#endif
                }
                catch (Exception ex)
                {
                    if (tex != null)
                        tex.Destroy();

                    if (request != null)
                        request.OnError(ex);
                }
                finally
                {
                    landform.Dispose();
                    road.Dispose();

                    ReleaseRenderTexture(ref normalMapRT);

#if !TEST_LANDFORM_BAKER
                    ReleaseRenderTexture(ref heightMapRT);
                    ReleaseRenderTexture(ref diffuseMapRT);
#endif
                }
            }
        }

        /// <summary>
        /// 释放临时的 RenderTexture,并且置为 null;
        /// </summary>
        void ReleaseRenderTexture(ref RenderTexture rt)
        {
            if (rt != null)
            {
                RenderTexture.ReleaseTemporary(rt);
                rt = null;
            }
        }

        /// <summary>
        /// 获取到覆盖到的坐标;
        /// </summary>
        IEnumerable<CubicHexCoord> GetOverlaye(IBakeRequest request)
        {
            return TerrainOverlayer.GetLandform(request.ChunkCoord);
        }

    }

}
