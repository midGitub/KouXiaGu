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
        /// 烘焙参数;
        /// </summary>
        public static BakingParameter Parameter
        {
            get { return GetInstance.parameter; }
            internal set { GetInstance.parameter = value; }
        }

        /// <summary>
        /// 烘培使用的相机;
        /// </summary>
        static Camera BakingCamera
        {
            get { return GetInstance.bakingCamera; }
        }

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

                instance.InitBakingCamera();
                instance.StartCoroutine(instance.Bake());

                IsInitialised = true;
            }
        }


        TerrainBaker() { }

        /// <summary>
        /// 负责渲染的摄像机;
        /// </summary>
        [SerializeField]
        Camera bakingCamera;

        [SerializeField]
        BakingParameter parameter = new BakingParameter(120, 0, 1);

        [SerializeField]
        NormalMapper normalMapper;

        [SerializeField]
        LandformBaker landform;

        [SerializeField]
        RoadBaker road;

        [SerializeField]
        DecorateBlend decorateBlend;


        void OnValidate()
        {
            parameter.Reset();
        }

        protected override void OnDestroy()
        {
            bakeRequestQueue.Clear();
            IsInitialised = false;
            base.OnDestroy();
        }


        /// <summary>
        /// 初始化烘焙相机参数;
        /// </summary>
        [ContextMenu("初始化相机")]
        void InitBakingCamera()
        {
            bakingCamera.aspect = BakingParameter.CameraAspect;
            bakingCamera.orthographicSize = BakingParameter.CameraSize;
            bakingCamera.transform.rotation = BakingParameter.CameraRotation;
            bakingCamera.clearFlags = CameraClearFlags.SolidColor;  //必须设置为纯色,否则摄像机渲染贴图会有(残图?);

            bakingCamera.backgroundColor = Color.black;
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
                    var cover = GetCover(request);

                    landform.Bake(request, cover);
                    road.Bake(request, cover);

                    heightMapRT = decorateBlend.BlendHeight(landform.HeightRT, road.HeightRT);
                    diffuseMapRT = decorateBlend.BlendDiffuse(landform.DiffuseRT, road.DiffuseRT);
                    normalMapRT = normalMapper.Rander(heightMapRT);

                    tex = new TerrainTexPack();
                    tex.heightMap = GetHeightTexture(heightMapRT);
                    tex.normalMap = normalMapper.GetTexture(normalMapRT);
                    tex.diffuseMap = GetDiffuseTexture(diffuseMapRT);

                    request.OnComplete(tex);
                    tex = null;
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
                    ReleaseRenderTexture(ref heightMapRT);
                    ReleaseRenderTexture(ref diffuseMapRT);
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
        /// 使用摄像机指定背景颜色烘焙;
        /// </summary>
        public static void CameraRender(RenderTexture rt, CubicHexCoord cameraPoint, Color backgroundColor)
        {
            Color current = BakingCamera.backgroundColor;
            BakingCamera.backgroundColor = backgroundColor;

            CameraRender(rt, cameraPoint);

            BakingCamera.backgroundColor = current;
        }

        /// <summary>
        /// 使用摄像机烘焙;
        /// </summary>
        public static void CameraRender(RenderTexture rt, CubicHexCoord cameraPoint)
        {
            Vector3 cameraPixelPoint = cameraPoint.GetTerrainPixel(5f);
            CameraRender(rt, cameraPixelPoint);
        }

        /// <summary>
        /// 使用摄像机指定背景颜色烘焙;
        /// </summary>
        public static void CameraRender(RenderTexture rt, Vector3 cameraPoint, Color backgroundColor)
        {
            Color current = BakingCamera.backgroundColor;
            BakingCamera.backgroundColor = backgroundColor;

            CameraRender(rt, cameraPoint);

            BakingCamera.backgroundColor = current;
        }

        /// <summary>
        /// 使用摄像机烘焙;
        /// </summary>
        public static void CameraRender(RenderTexture rt, Vector3 cameraPoint)
        {
            Camera bakingCamera = GetInstance.bakingCamera;
            bakingCamera.transform.position = cameraPoint;
            CameraRender(rt);
        }

        /// <summary>
        /// 使用摄像机指定背景颜色烘焙;
        /// </summary>
        public static void CameraRender(RenderTexture rt, Color backgroundColor)
        {
            Color current = BakingCamera.backgroundColor;
            BakingCamera.backgroundColor = backgroundColor;

            CameraRender(rt);

            BakingCamera.backgroundColor = current;
        }

        /// <summary>
        /// 使用摄像机烘焙;
        /// </summary>
        public static void CameraRender(RenderTexture rt)
        {
            BakingCamera.targetTexture = rt;
            BakingCamera.Render();
            BakingCamera.targetTexture = null;
        }


        /// <summary>
        /// 获取到覆盖到的坐标;
        /// </summary>
        IEnumerable<CubicHexCoord> GetCover(IBakeRequest request)
        {
            return TerrainChunk.GetChunkCover(request.ChunkCoord);
        }

        public static Texture2D GetHeightTexture(RenderTexture rt)
        {
            RenderTexture.active = rt;
            Texture2D heightMap = new Texture2D((int)Parameter.HeightMapWidth, (int)Parameter.HeightMapHeight, TextureFormat.RGB24, false);
            heightMap.ReadPixels(Parameter.HeightReadPixel, 0, 0, false);
            heightMap.wrapMode = TextureWrapMode.Clamp;
            heightMap.Apply();
            return heightMap;
        }

        public static Texture2D GetDiffuseTexture(RenderTexture rt)
        {
            RenderTexture.active = rt;
            Texture2D diffuseTex = new Texture2D((int)Parameter.DiffuseTexWidth, (int)Parameter.DiffuseTexHeight, TextureFormat.RGB24, false);
            diffuseTex.ReadPixels(Parameter.DiffuseReadPixel, 0, 0, false);
            diffuseTex.wrapMode = TextureWrapMode.Clamp;
            diffuseTex.Apply();
            return diffuseTex;
        }

    }

}
