
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
    /// 烘培地形
    /// </summary>
    [DisallowMultipleComponent]
    [Obsolete]
    public sealed partial class OBaker : OSceneSington<OBaker>
    {
        static OBaker()
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
        public static LinkedList<IBakeRequest> Requested
        {
            get { return bakeRequestQueue; }
        }

        /// <summary>
        /// 是否已经初始化?
        /// </summary>
        public static bool IsInitialised { get; private set; }


        public static void Initialize()
        {
            if (!IsInitialised)
            {
                OBaker instance = GetInstance;

                instance.road.Initialise();
                instance.landform.Initialise();

                instance.decorateBlend.Awake();

                IsInitialised = true;
            }
        }


        OBaker()
        {
        }

        [SerializeField]
        NormalMapper normalMapper;

        [SerializeField]
        OLandformBaker landform;

        [SerializeField]
        RoadBaker road;

        [SerializeField]
        DecorateBlend decorateBlend;

#if TEST_BAKE
        [SerializeField]
        TestBaker testBaker;
#endif

        public bool IsRunning
        {
            get { return enabled; }
            set { enabled = value; }
        }

        protected override void OnDestroy()
        {
            bakeRequestQueue.Clear();
            IsInitialised = false;
            base.OnDestroy();
        }

        void Update()
        {
            if (CanUpdate())
            {
                IBakeRequest request = bakeRequestQueue.Dequeue();
                Bake(request);
            }
        }

        bool CanUpdate()
        {
            return bakeRequestQueue.Count > 0;
        }

        /// <summary>
        /// 在协程内队列中进行烘焙;
        /// 流程:
        /// 建筑高度平整图->建筑地表图->
        /// 地形高度图->地形地表图->
        /// 混合高度->混合地表->
        /// 高度生成法线图->完成
        /// </summary>
        void Bake(IBakeRequest request)
        {
            RenderTexture normalMapRT = null;
            RenderTexture heightMapRT = null;
            RenderTexture diffuseMapRT = null;
            TerrainTexPack tex = null;

            try
            {
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
                tex.heightMap = LandformBakeManager.GetHeightTexture(heightMapRT);
                tex.normalMap = normalMapper.GetTexture(normalMapRT);
                tex.diffuseMap = LandformBakeManager.GetDiffuseTexture(diffuseMapRT);

                request.OnComplete(tex);
                tex = null;
#endif
            }
            catch (Exception ex)
            {
                if (tex != null)
                    tex.Destroy();

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
            return ChunkPartitioner.GetLandform(request.ChunkCoord);
        }

    }

}
