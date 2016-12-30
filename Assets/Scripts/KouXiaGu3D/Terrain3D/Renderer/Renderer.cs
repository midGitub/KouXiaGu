using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using KouXiaGu.Grids;
using System.Collections;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 基本贴图信息渲染,负责将传入的请求渲染出基本的高度图和地貌贴图;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class Renderer : UnitySington<Renderer>
    {
        Renderer() { }

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
        RenderTerrain terrainRender;

        [SerializeField]
        DecorateRoad roadDescorate;

        static Coroutine bakingCoroutine;

        /// <summary>
        /// 将要进行烘焙的队列;
        /// </summary>
        static readonly LinkedList<IBakeRequest> bakingQueue = new LinkedList<IBakeRequest>();

        /// <summary>
        /// 是否运行中?
        /// </summary>
        public static bool IsRunning
        {
            get { return bakingCoroutine != null; }
        }

        /// <summary>
        /// 烘焙时的参数;
        /// </summary>
        public static BakingParameter Parameter
        {
            get { return GetInstance.parameter; }
            set { GetInstance.parameter = value; }
        }

        static Camera BakingCamera
        {
            get { return GetInstance.bakingCamera; }
        }

        /// <summary>
        /// 烘焙请求队列;
        /// </summary>
        public static LinkedList<IBakeRequest> BakingRequests
        {
            get { return bakingQueue; }
        }

        public static void Clear()
        {
            bakingQueue.Clear();
        }

        void Awake()
        {
            terrainRender.Awake();
            roadDescorate.Awake();
        }

        void Start()
        {
            InitBakingCamera();
            StartCoroutine();
        }

        void OnValidate()
        {
            parameter.Reset();
        }

        /// <summary>
        /// 开始烘焙协程;
        /// </summary>
        public void StartCoroutine()
        {
            if (!IsRunning)
            {
                bakingCoroutine = StartCoroutine(Baking());
            }
        }

        /// <summary>
        /// 停止烘焙的协程,清空请求队列;
        /// </summary>
        public void StopCoroutine()
        {
            StopCoroutine(bakingCoroutine);
            bakingQueue.Clear();
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
        IEnumerator Baking()
        {
            CustomYieldInstruction bakingYieldInstruction = new WaitWhile(() => bakingQueue.Count == 0);

            IBakeRequest request = null;
            RenderTexture normalMapRT = null;

            Texture2D normalMap = null;
            Texture2D heightMap = null;
            Texture2D diffuse = null;

            while (true)
            {
                yield return bakingYieldInstruction;
                try
                {
                    request = bakingQueue.Dequeue();
                    var cover = GetCover(request);

                    terrainRender.Render(request, cover);
                    normalMapRT = normalMapper.Rander(terrainRender.HeightRT);

                    heightMap = terrainRender.GetHeightTexture();
                    normalMap = normalMapper.GetTexture(normalMapRT);
                    diffuse = terrainRender.GetDiffuseTexture();

                    request.OnComplete(diffuse, heightMap, normalMap);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("烘焙时出现错误:" + ex);

                    Destroy(diffuse);
                    Destroy(heightMap);
                    Destroy(normalMap);

                    if(request != null)
                        request.OnError(ex);
                }
                finally
                {
                    terrainRender.Dispose();
                    RenderTexture.ReleaseTemporary(normalMapRT);
                }
            }
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
        /// 使用摄像机烘焙;
        /// </summary>
        public static void CameraRender(RenderTexture rt, MeshDisplay display)
        {
            SetBakingCamera(display);
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
        /// 设置烘焙相机到对应位置;
        /// </summary>
        static void SetBakingCamera(MeshDisplay display)
        {
            Camera bakingCamera = GetInstance.bakingCamera;
            Vector3 cameraPoint = GridConvert.Grid.GetPixel(display.Center, 5);
            bakingCamera.transform.position = cameraPoint;
        }

        /// <summary>
        /// 获取到覆盖到的坐标;
        /// </summary>
        IEnumerable<CubicHexCoord> GetCover(IBakeRequest request)
        {
            return TerrainChunk.GetChunkCover(request.ChunkCoord);
        }

    }

}
