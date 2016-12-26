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
        /// 用于烘焙放置节点内容的网格;
        /// </summary>
        [SerializeField]
        MeshDisplay widerRectMeshPool;

        [SerializeField]
        BakingParameter parameter = new BakingParameter(120, 0, 1);

        //[SerializeField]
        //MixerTex mixer;
        [SerializeField]
        HeightRenderer heightRenderer;
        [SerializeField]
        NormalMapper normalMapper;
        [SerializeField]
        DiffuseTex diffuser;

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
            parameter.Reset();
            widerRectMeshPool.Awake();
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
        /// 流程:高度->建筑,道路地形平整->法线->贴图->完成
        /// </summary>
        IEnumerator Baking()
        {
            CustomYieldInstruction bakingYieldInstruction = new WaitWhile(() => bakingQueue.Count == 0);

            IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes;
            IBakeRequest request = null;
            RenderTexture heightMapRT = null;
            RenderTexture normalMapRT = null;
            RenderTexture diffuseRT = null;
            Texture2D normalMap = null;
            Texture2D heightMap = null;
            Texture2D diffuse = null;

            while (true)
            {
                yield return bakingYieldInstruction;

                try
                {
                    request = bakingQueue.Dequeue();
                    bakingNodes = PrepareBaking(request);

                    heightMapRT = heightRenderer.Baking(bakingNodes);
                    normalMapRT = normalMapper.Rander(heightMapRT);
                    diffuseRT = diffuser.Baking(bakingNodes);

                    heightMap = heightRenderer.GetTexture(heightMapRT);
                    normalMap = normalMapper.GetTexture(normalMapRT);
                    diffuse = diffuser.GetTexture(diffuseRT);

                    request.OnComplete(diffuse, heightMap, normalMap);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("烘焙时出现错误:" + ex);

                    Destroy(diffuse);
                    Destroy(heightMap);
                    Destroy(normalMap);
                    Destroy(null);

                    if(request != null)
                        request.OnError(ex);
                }
                finally
                {
                    RenderTexture.ReleaseTemporary(heightMapRT);
                    RenderTexture.ReleaseTemporary(normalMapRT);
                    RenderTexture.ReleaseTemporary(diffuseRT);
                }
            }
        }

        static void Render(RenderTexture rt)
        {
            Camera bakingCamera = GetInstance.bakingCamera;

            bakingCamera.targetTexture = rt;
            bakingCamera.Render();
            bakingCamera.targetTexture = null;
        }


        Dictionary<CubicHexCoord, MeshRenderer> active;

        ///// <summary>
        ///// 烘焙前的准备,返回烘焙对应的网格;
        ///// </summary>
        //List<KeyValuePair<BakingNode, MeshRenderer>> PrepareBaking(IBakeRequest request)
        //{
        //    bakingCamera.transform.position = request.CameraPosition;

        //    IEnumerable<BakingNode> bakingNodes = request.BakingNodes;
        //    List<KeyValuePair<BakingNode, MeshRenderer>> list = new List<KeyValuePair<BakingNode, MeshRenderer>>();

        //    widerRectMeshPool.RecoveryActive();

        //    int indexY = -2;

        //    foreach (var node in bakingNodes)
        //    {
        //        Vector3 position = new Vector3(node.Position.x, indexY--, node.Position.z);
        //        var mesh = widerRectMeshPool.Dequeue(position, node.RotationY);
        //        list.Add(new KeyValuePair<BakingNode, MeshRenderer>(node, mesh));
        //    }

        //    return list;
        //}


        /// <summary>
        /// 烘焙前的准备,返回烘焙对应的网格;
        /// </summary>
        List<KeyValuePair<BakingNode, MeshRenderer>> PrepareBaking(IBakeRequest request)
        {
            bakingCamera.transform.position = new Vector3();

            IEnumerable<BakingNode> bakingNodes = request.BakingNodes;
            List<KeyValuePair<BakingNode, MeshRenderer>> list = new List<KeyValuePair<BakingNode, MeshRenderer>>();

            widerRectMeshPool.RecoveryActive();

            foreach (var node in bakingNodes)
            {
                var mesh = widerRectMeshPool.Dequeue(node.MapPosition, node.MapCenter, node.RotationY);
                list.Add(new KeyValuePair<BakingNode, MeshRenderer>(node, mesh));
            }

            return list;
        }


        /// <summary>
        /// 场景内的网格控制;
        /// </summary>
        [Serializable]
        class MeshDisplay
        {
            [SerializeField]
            Transform parent;

            [SerializeField]
            MeshRenderer ovenDisplayPrefab;

            /// <summary>
            /// 中心点,根据传入坐标位置转换到此中心点附近;
            /// </summary>
            [SerializeField]
            CubicHexCoord center;

            Queue<MeshRenderer> sleep;
            Dictionary<CubicHexCoord, MeshRenderer> active;

            public void Awake()
            {
                sleep = new Queue<MeshRenderer>();
                active = new Dictionary<CubicHexCoord, MeshRenderer>();
            }

            /// <summary>
            /// 布置到场景;
            /// </summary>
            public MeshRenderer Dequeue(CubicHexCoord coord, CubicHexCoord center, float rotationY)
            {
                MeshRenderer mesh;
                Quaternion rotation = Quaternion.Euler(0, rotationY, 0);
                Vector3 position = PositionConvert(coord, center);
                if (sleep.Count == 0)
                {
                    mesh = GameObject.Instantiate(ovenDisplayPrefab, position, rotation, parent) as MeshRenderer;
                    mesh.gameObject.SetActive(true);
                }
                else
                {
                    mesh = sleep.Dequeue();
                    mesh.transform.position = position;
                    mesh.transform.rotation = rotation;
                    mesh.gameObject.SetActive(true);
                }

                active.Add(coord, mesh);
                return mesh;
            }

            Vector3 PositionConvert(CubicHexCoord coord, CubicHexCoord center)
            {
                CubicHexCoord oCoord = coord - center;
                CubicHexCoord rCoord = oCoord + this.center;
                return GridConvert.Grid.GetPixel(rCoord, -active.Count);
            }

            /// <summary>
            /// 回收所有在场景中的网格;
            /// </summary>
            public void RecoveryActive()
            {
                foreach (var item in active.Values)
                {
                    Destroy(item);
                }
                active.Clear();
            }

            void Destroy(MeshRenderer mesh)
            {
                mesh.gameObject.SetActive(false);
                sleep.Enqueue(mesh);
            }

        }


    }

}
