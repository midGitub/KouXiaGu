using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 基本贴图信息渲染,负责将传入的请求渲染出基本的高度图和地貌贴图;
    /// </summary>
    [DisallowMultipleComponent, CustomEditorTool]
    public sealed partial class Renderer : UnitySingleton<Renderer>
    {
        Renderer() { }

        /// <summary>
        /// 负责渲染的摄像机;
        /// </summary>
        [SerializeField]
        Camera bakingCamera;
        /// <summary>
        /// 在烘焙过程中放置在场景内的物体;
        /// </summary>
        [SerializeField]
        RenderDisplayMeshPool ovenDisplayMeshPool;

        [SerializeField]
        BakingParameter parameter = new BakingParameter(120, 0, 1);

        [SerializeField]
        MixerTex mixer;
        [SerializeField]
        HeightRenderer heightRenderer;
        [SerializeField]
        internal NormalMapper normalMapper;
        [SerializeField]
        DiffuseTex diffuser;

        static Coroutine bakingCoroutine;

        /// <summary>
        /// 将要进行烘焙的队列;
        /// </summary>
        static readonly LinkedList<BakeRequest> bakingQueue = new LinkedList<BakeRequest>();

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

        /// <summary>
        /// 烘焙请求队列;
        /// </summary>
        public static LinkedList<BakeRequest> BakingRequests
        {
            get { return bakingQueue; }
        }

        public static void Clear()
        {
            bakingQueue.Clear();
        }

        void Awake()
        {
            parameter.Reset();
            ovenDisplayMeshPool.Awake();
        }

        void Start()
        {
            InitBakingCamera();
            StartCoroutine();
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
        /// </summary>
        /// <returns></returns>
        IEnumerator Baking()
        {
            CustomYieldInstruction bakingYieldInstruction = new WaitWhile(() => bakingQueue.Count == 0);

            while (true)
            {
                yield return bakingYieldInstruction;

                var request = bakingQueue.Dequeue();

                IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes = PrepareBaking(request);

                RenderTexture mixerRT = null;
                RenderTexture heightMapRT = null;
                RenderTexture normalMapRT = null;
                RenderTexture diffuseRT = null;
                Texture2D normalMap;
                Texture2D heightMap;
                Texture2D diffuse;

                try
                {
                    mixerRT = mixer.BakingMixer(bakingNodes);
                    heightMapRT = heightRenderer.BakingHeight(bakingNodes, mixerRT);
                    normalMapRT = normalMapper.Rander(heightMapRT);
                    diffuseRT = diffuser.BakingDiffuse(bakingNodes, mixerRT, heightMapRT);

                    normalMap = GetNormalMap(normalMapRT);
                    heightMap = heightRenderer.GetHeightMap(heightMapRT);
                    diffuse = diffuser.GetDiffuseTexture(diffuseRT);

                    request.TextureComplete(diffuse, heightMap, normalMap);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("烘焙时出现错误:" + ex);
                }
                finally
                {
                    RenderTexture.ReleaseTemporary(mixerRT);
                    RenderTexture.ReleaseTemporary(heightMapRT);
                    RenderTexture.ReleaseTemporary(normalMapRT);
                    RenderTexture.ReleaseTemporary(diffuseRT);
                }
            }
        }

        /// <summary>
        /// 烘焙前的准备,返回烘焙对应的网格;
        /// </summary>
        List<KeyValuePair<BakingNode, MeshRenderer>> PrepareBaking(BakeRequest request)
        {
            bakingCamera.transform.position = request.CameraPosition;

            IEnumerable<BakingNode> bakingNodes = request.BakingNodes;
            List<KeyValuePair<BakingNode, MeshRenderer>> list = new List<KeyValuePair<BakingNode, MeshRenderer>>();

            ovenDisplayMeshPool.RecoveryActive();

            int indexY = -2;

            foreach (var node in bakingNodes)
            {
                Vector3 position = new Vector3(node.Position.x, indexY--, node.Position.z);
                var mesh = ovenDisplayMeshPool.Dequeue(position, node.RotationY);
                list.Add(new KeyValuePair<BakingNode, MeshRenderer>(node, mesh));
            }

            return list;
        }

        /// <summary>
        /// 获取到法线贴图;
        /// </summary>
        Texture2D GetNormalMap(RenderTexture rt)
        {
            RenderTexture.active = rt;
            Texture2D normalMap = new Texture2D(parameter.HeightMapWidth, parameter.HeightMapHeight, TextureFormat.ARGB32, false);
            normalMap.ReadPixels(parameter.HeightReadPixel, 0, 0, false);
            normalMap.wrapMode = TextureWrapMode.Clamp;
            normalMap.Apply();
            
            return normalMap;
        }

        static void Render(RenderTexture rt)
        {
            Camera bakingCamera = GetInstance.bakingCamera;

            bakingCamera.targetTexture = rt;
            bakingCamera.Render();
            bakingCamera.targetTexture = null;
        }

    }

}
