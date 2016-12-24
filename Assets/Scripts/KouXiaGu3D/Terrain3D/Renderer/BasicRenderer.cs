using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using KouXiaGu.ImageEffects;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 基本贴图信息渲染,负责将传入的请求渲染出基本的高度图和地貌贴图;
    /// </summary>
    [DisallowMultipleComponent, CustomEditorTool]
    public sealed class BasicRenderer : UnitySingleton<BasicRenderer>
    {
        BasicRenderer() { }

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
        Shader mixer;
        [SerializeField]
        HeightRenderer heightRenderer;
        [SerializeField]
        internal NormalMapper normalMapper;
        [SerializeField]
        Shader diffuse;

        static Material mixerMaterial;
        static Material diffuseMaterial;

        static Coroutine bakingCoroutine;

        /// <summary>
        /// 将要进行烘焙的队列;
        /// </summary>
        static readonly LinkedList<BakeRequest> bakingQueue = new LinkedList<BakeRequest>();

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
            InitMaterial();

            StartCoroutine();
        }

        /// <summary>
        /// 加入到烘焙队列;
        /// </summary>
        public static void Enqueue(BakeRequest request)
        {
            bakingQueue.AddLast(request);
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

        void InitMaterial()
        {
            mixerMaterial = new Material(mixer);
            mixerMaterial.hideFlags = HideFlags.HideAndDontSave;

            diffuseMaterial = new Material(diffuse);
            diffuseMaterial.hideFlags = HideFlags.HideAndDontSave;
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
                    mixerRT = BakingMixer(bakingNodes);
                    heightMapRT = heightRenderer.BakingHeight(bakingNodes, mixerRT);
                    normalMapRT = BakingNormalMap(heightMapRT);
                    diffuseRT = BakingDiffuse(bakingNodes, mixerRT, heightMapRT);

                    normalMap = GetNormalMap(normalMapRT);
                    heightMap = heightRenderer.GetHeightMap(heightMapRT);
                    diffuse = GetDiffuseTexture(diffuseRT);

                    request.TextureComplete(diffuse, heightMap, normalMap);
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
        /// 对混合贴图进行烘焙;传入需要设置到的地貌节点;
        /// </summary>
        RenderTexture BakingMixer(IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes)
        {
            foreach (var pair in bakingNodes)
            {
                BakingNode node = pair.Key;
                MeshRenderer hexMesh = pair.Value;

                if (hexMesh.material != null)
                    GameObject.Destroy(hexMesh.material);

                hexMesh.material = mixerMaterial;
                hexMesh.material.mainTexture = node.MixerTexture;
            }

            RenderTexture mixerRT = RenderTexture.GetTemporary(parameter.rDiffuseTexWidth, parameter.rDiffuseTexHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
            Render(mixerRT);
            return mixerRT;
        }

        /// <summary>
        /// 根据高度图生成法线贴图;
        /// </summary>
        RenderTexture BakingNormalMap(Texture height)
        {
            return normalMapper.Rander(height);
        }

        /// <summary>
        /// 烘焙材质贴图;
        /// </summary>
        RenderTexture BakingDiffuse(IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes, Texture mixer, Texture height)
        {
            foreach (var pair in bakingNodes)
            {
                BakingNode node = pair.Key;
                MeshRenderer hexMesh = pair.Value;

                if (hexMesh.material != null)
                    GameObject.Destroy(hexMesh.material);

                hexMesh.material = diffuseMaterial;

                hexMesh.material.SetTexture("_MainTex", node.DiffuseTexture);
                hexMesh.material.SetTexture("_Mixer", node.MixerTexture);
                hexMesh.material.SetTexture("_Height", node.HeightTexture);
                hexMesh.material.SetFloat("_Centralization", 1.0f);
            }

            RenderTexture diffuseRT = RenderTexture.GetTemporary(parameter.rDiffuseTexWidth, parameter.rDiffuseTexHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
            Render(diffuseRT);
            return diffuseRT;
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

        Texture2D GetDiffuseTexture(RenderTexture renderTexture)
        {
            RenderTexture.active = renderTexture;
            Texture2D diffuse = new Texture2D(parameter.DiffuseTexWidth, parameter.DiffuseTexHeight, TextureFormat.RGB24, false);
            diffuse.ReadPixels(parameter.DiffuseReadPixel, 0, 0, false);
            diffuse.wrapMode = TextureWrapMode.Clamp;
            diffuse.Apply();

            return diffuse;
        }

        static void Render(RenderTexture rt)
        {
            Camera bakingCamera = GetInstance.bakingCamera;

            bakingCamera.targetTexture = rt;
            bakingCamera.Render();
            bakingCamera.targetTexture = null;
        }

        [Serializable]
        class HeightRenderer
        {
            HeightRenderer() { }

            [SerializeField]
            Shader heightShader;

            Material _heightMaterial;

            Material heightMaterial
            {
                get { return _heightMaterial ?? (_heightMaterial = new Material(heightShader)); }
            }

            public RenderTexture BakingHeight(IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes, Texture mixer)
            {
                foreach (var pair in bakingNodes)
                {
                    BakingNode node = pair.Key;
                    MeshRenderer hexMesh = pair.Value;

                    if (hexMesh.material != null)
                        GameObject.Destroy(hexMesh.material);

                    hexMesh.material = heightMaterial;
                    hexMesh.material.SetTexture("_MainTex", node.HeightTexture);
                    hexMesh.material.SetTexture("_Mixer", node.MixerTexture);
                    hexMesh.material.SetTexture("_GlobalMixer", mixer);
                }

                RenderTexture heightRT = RenderTexture.GetTemporary(Parameter.rHeightMapWidth, Parameter.rHeightMapHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
                Render(heightRT);

                var blueHeightRT = ImageEffect.BlurOptimized(heightRT, 3, 0, 1, ImageEffect.BlurType.StandardGauss);
                RenderTexture.ReleaseTemporary(heightRT);

                return blueHeightRT;
            }

            public Texture2D GetHeightMap(RenderTexture rt)
            {
                RenderTexture.active = rt;
                Texture2D heightMap = new Texture2D(Parameter.HeightMapWidth, Parameter.HeightMapHeight, TextureFormat.ARGB32, false);
                heightMap.ReadPixels(Parameter.HeightReadPixel, 0, 0, false);
                heightMap.wrapMode = TextureWrapMode.Clamp;
                heightMap.Apply();

                return heightMap;
            }


            public void Clear()
            {
                if (_heightMaterial != null)
                    Destroy(_heightMaterial);
            }

        }

    }


    /// <summary>
    /// 地形烘焙时的贴图大小参数;
    /// </summary>
    [Serializable]
    public struct BakingParameter
    {

        /// <summary>
        /// 烘焙时的边框比例(需要裁剪的部分比例);
        /// </summary>
        public static readonly float OutlineScale = 1f / 12f;

        /// <summary>
        /// 完整预览整个地图块的摄像机旋转角度;
        /// </summary>
        public static readonly Quaternion CameraRotation = Quaternion.Euler(90, 0, 0);

        /// <summary>
        /// 完整预览整个地图块的摄像机大小;
        /// </summary>
        public static readonly float CameraSize = 
            ((TerrainChunk.CHUNK_HEIGHT + (TerrainChunk.CHUNK_HEIGHT * OutlineScale)) / 2);

        /// <summary>
        /// 完整预览整个地图块的摄像机比例(W/H);
        /// </summary>
        public static readonly float CameraAspect = 
            (TerrainChunk.CHUNK_WIDTH + TerrainChunk.CHUNK_WIDTH * OutlineScale) / 
            (TerrainChunk.CHUNK_HEIGHT + TerrainChunk.CHUNK_HEIGHT * OutlineScale);

        [SerializeField, Range(50, 500)]
        float textureSize;

        [SerializeField, Range(0,3)]
        int diffuseTexDownsample;

        [SerializeField, Range(0, 3)]
        int heightMapDownsample;

        /// <summary>
        /// 贴图大小;
        /// </summary>
        public float TextureSize
        {
            get { return textureSize; }
            private set { textureSize = value; }
        }

        /// <summary>
        /// 图片裁剪后的尺寸;
        /// </summary>
        public int DiffuseTexWidth { get; private set; }
        public int DiffuseTexHeight { get; private set; }
        public int HeightMapWidth { get; private set; }
        public int HeightMapHeight { get; private set; }

        /// <summary>
        /// 烘焙时的尺寸;
        /// </summary>
        public int rDiffuseTexWidth { get; private set; }
        public int rDiffuseTexHeight { get; private set; }
        public int rHeightMapWidth { get; private set; }
        public int rHeightMapHeight { get; private set; }

        /// <summary>
        /// 裁剪区域;
        /// </summary>
        public Rect DiffuseReadPixel { get; private set; }
        public Rect HeightReadPixel { get; private set; }

        public BakingParameter(float textureSize, int diffuseTexDownsample, int heightMapDownsample) : this()
        {
            this.diffuseTexDownsample = diffuseTexDownsample;
            this.heightMapDownsample = heightMapDownsample;
            SetTextureSize(textureSize);
        }

        public void Reset()
        {
            SetTextureSize(this.textureSize);
        }

        void SetTextureSize(float size)
        {
            float chunkWidth = TerrainChunk.CHUNK_WIDTH * size;
            float chunkHeight = TerrainChunk.CHUNK_HEIGHT * size;

            this.DiffuseTexWidth = (int)(chunkWidth) >> diffuseTexDownsample;
            this.DiffuseTexHeight = (int)(chunkHeight) >> diffuseTexDownsample;
            this.HeightMapWidth = (int)(chunkWidth) >> heightMapDownsample;
            this.HeightMapHeight = (int)(chunkHeight) >> heightMapDownsample;

            this.rDiffuseTexWidth = (int)(DiffuseTexWidth + DiffuseTexWidth * OutlineScale);
            this.rDiffuseTexHeight = (int)(DiffuseTexHeight + DiffuseTexHeight * OutlineScale);
            this.rHeightMapWidth = (int)(HeightMapWidth + HeightMapWidth * OutlineScale);
            this.rHeightMapHeight = (int)(HeightMapHeight + HeightMapHeight * OutlineScale);

            this.DiffuseReadPixel = 
                new Rect(
                    DiffuseTexWidth * OutlineScale / 2,
                    DiffuseTexHeight * OutlineScale / 2, 
                    DiffuseTexWidth,
                    DiffuseTexHeight);

            this.HeightReadPixel = 
                new Rect(
                    HeightMapWidth * OutlineScale / 2,
                    HeightMapHeight * OutlineScale / 2,
                    HeightMapWidth,
                    HeightMapHeight);

            this.TextureSize = size;
        }

    }

}
