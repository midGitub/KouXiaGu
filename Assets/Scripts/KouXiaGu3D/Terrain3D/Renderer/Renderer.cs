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
        /// 流程:
        /// 建筑高度平整图->建筑地表图->
        /// 地形高度图->地形地表图->
        /// 混合高度->混合地表->
        /// 高度生成法线图->完成
        /// </summary>
        IEnumerator Baking()
        {
            CustomYieldInstruction bakingYieldInstruction = new WaitWhile(() => bakingQueue.Count == 0);

            List<BakingNode> bakingNodes;
            List<KeyValuePair<BakingNode, MeshRenderer>> terrainDisplayMesh;
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
                    bakingNodes = GetBakingNodes(request);
                    terrainDisplayMesh = GetTerrainDisplayMesh(request, bakingNodes);

                    heightMapRT = heightRenderer.Baking(terrainDisplayMesh);
                    normalMapRT = normalMapper.Rander(heightMapRT);
                    diffuseRT = diffuser.Baking(terrainDisplayMesh);

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

        /// <summary>
        /// 获取到所有需要烘焙的地图节点;
        /// </summary>
        List<BakingNode> GetBakingNodes(IBakeRequest request)
        {
            List<BakingNode> bakingNodes = new List<BakingNode>();
            IEnumerable<CubicHexCoord> cover = TerrainChunk.GetChunkCover(request.ChunkCoord);
            TerrainNode mapNode;

            foreach (var point in cover)
            {
                if (request.Map.TryGetValue(point, out mapNode))
                {
                    BakingNode bakingNode = new BakingNode(point, mapNode);
                    bakingNodes.Add(bakingNode);
                }
            }
            return bakingNodes;
        }

        /// <summary>
        /// 获取到地形烘焙显示的网格;
        /// </summary>
        List<KeyValuePair<BakingNode, MeshRenderer>> GetTerrainDisplayMesh(IBakeRequest request, IEnumerable<BakingNode> bakingNodes)
        {
            List<KeyValuePair<BakingNode, MeshRenderer>> list = new List<KeyValuePair<BakingNode, MeshRenderer>>();
            CubicHexCoord center = TerrainChunk.GetHexCenter(request.ChunkCoord);

            widerRectMeshPool.RecoveryActive();

            foreach (var bakingNode in bakingNodes)
            {
                CubicHexCoord crood = bakingNode.Position;
                var mesh = widerRectMeshPool.Dequeue(crood, center, bakingNode.RotationY);
                list.Add(new KeyValuePair<BakingNode, MeshRenderer>(bakingNode, mesh));
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
            Queue<MeshRenderer> active;

            public void Awake()
            {
                sleep = new Queue<MeshRenderer>();
                active = new Queue<MeshRenderer>();
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

                active.Enqueue(mesh);
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
                while (active.Count != 0)
                {
                    var item = active.Dequeue();
                    Destroy(item);
                }
            }

            void Destroy(MeshRenderer mesh)
            {
                mesh.gameObject.SetActive(false);
                sleep.Enqueue(mesh);
            }

        }


    }

}
