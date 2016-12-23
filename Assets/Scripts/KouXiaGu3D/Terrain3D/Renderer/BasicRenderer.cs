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

        [SerializeField, HideInInspector]
        BakingParameter prm = new BakingParameter(150);

        [SerializeField]
        Shader mixer;
        [SerializeField]
        Shader height;
        [SerializeField]
        Shader heightToAlpha;
        [SerializeField]
        Shader diffuse;
        [SerializeField]
        Shader blur;

        static Material mixerMaterial;
        static Material heightMaterial;
        static Material heightToAlphaMaterial;
        static Material diffuseMaterial;
        static Material blurMaterial;

        static Coroutine bakingCoroutine;

        /// <summary>
        /// 将要进行烘焙的队列(对外只提供查询,以允许移除;);
        /// </summary>
        static readonly Queue<BakeRequest> bakingQueue = new Queue<BakeRequest>();

        public static bool IsRunning
        {
            get { return bakingCoroutine != null; }
        }

        public static Queue<BakeRequest> BakingRequests
        {
            get { return bakingQueue; }
        }

        [ExposeProperty]
        public float TextureSize
        {
            get { return prm.TextureSize; }
            set { prm = new BakingParameter(value); }
        }

        public static void Clear()
        {
            bakingQueue.Clear();
        }

        void Awake()
        {
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
            bakingQueue.Enqueue(request);
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

            heightMaterial = new Material(height);
            heightMaterial.hideFlags = HideFlags.HideAndDontSave;

            heightToAlphaMaterial = new Material(heightToAlpha);
            heightToAlphaMaterial.hideFlags = HideFlags.HideAndDontSave;

            diffuseMaterial = new Material(diffuse);
            diffuseMaterial.hideFlags = HideFlags.HideAndDontSave;

            blurMaterial = new Material(blur);
            blurMaterial.hideFlags = HideFlags.HideAndDontSave;
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
                RenderTexture heightRT = null;
                RenderTexture alphaHeightRT = null;
                RenderTexture diffuseRT = null;
                Texture2D height;
                Texture2D diffuse;

                try
                {
                    mixerRT = BakingMixer(bakingNodes);
                    heightRT = BakingHeight(bakingNodes, mixerRT);
                    alphaHeightRT = BakingHeightToAlpha(heightRT);
                    diffuseRT = BakingDiffuse(bakingNodes, mixerRT, alphaHeightRT);

                    height = GetHeightTexture(alphaHeightRT);
                    diffuse = GetDiffuseTexture(diffuseRT);

                    request.TextureComplete(diffuse, height);

                }
                finally
                {
                    RenderTexture.ReleaseTemporary(mixerRT);
                    RenderTexture.ReleaseTemporary(heightRT);
                    RenderTexture.ReleaseTemporary(alphaHeightRT);
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

            RenderTexture mixerRT = RenderTexture.GetTemporary(prm.rDiffuseMapWidth, prm.rDiffuseMapHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
            Render(mixerRT);
            return mixerRT;
        }

        /// <summary>
        /// 混合高度贴图;
        /// </summary>
        RenderTexture BakingHeight(IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes, Texture mixer)
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

            RenderTexture heightRT = RenderTexture.GetTemporary(prm.rHeightMapWidth, prm.rHeightMapHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
            Render(heightRT);
            return heightRT;
        }

        /// <summary>
        /// 将高度图从灰度通道转到阿尔法通道上;
        /// </summary>
        RenderTexture BakingHeightToAlpha(Texture height)
        {
            RenderTexture alphaHeightRT = RenderTexture.GetTemporary(height.width, height.height, 0, RenderTextureFormat.ARGB32);

            height.filterMode = FilterMode.Bilinear;
            alphaHeightRT.filterMode = FilterMode.Bilinear;

            Graphics.Blit(height, alphaHeightRT, heightToAlphaMaterial, 0);
            return alphaHeightRT;
        }

        RenderTexture BakingDiffuse(IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes, Texture globalMixer, Texture globalHeight)
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
                //hexMesh.material.SetTexture("_GlobalMixer", globalMixer);
                //hexMesh.material.SetTexture("_ShadowsAndHeight", globalHeight);
                //hexMesh.material.SetFloat("_Sea", 0f);
                hexMesh.material.SetFloat("_Centralization", 1.0f);
            }

            RenderTexture diffuseRT = RenderTexture.GetTemporary(prm.rDiffuseMapWidth, prm.rDiffuseMapHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
            Render(diffuseRT);
            return diffuseRT;
        }

        Texture2D GetHeightTexture(RenderTexture renderTexture)
        {
            RenderTexture.active = renderTexture;
            Texture2D height_ARGB32 = new Texture2D(prm.HeightMapWidth, prm.HeightMapHeight, TextureFormat.ARGB32, false);
            height_ARGB32.ReadPixels(prm.HeightReadPixel, 0, 0, false);
            height_ARGB32.wrapMode = TextureWrapMode.Clamp;
            height_ARGB32.Apply();

            Texture2D height_Alpha8 = new Texture2D(prm.HeightMapWidth, prm.HeightMapHeight, TextureFormat.Alpha8, false);
            height_Alpha8.wrapMode = TextureWrapMode.Clamp;
            Color32[] data = height_ARGB32.GetPixels32();

            height_Alpha8.SetPixels32(data);
            height_Alpha8.Apply();

            GameObject.Destroy(height_ARGB32);

            return height_Alpha8;
        }

        Texture2D GetDiffuseTexture(RenderTexture renderTexture)
        {
            RenderTexture.active = renderTexture;
            Texture2D diffuse = new Texture2D(prm.DiffuseMapWidth, prm.DiffuseMapHeight, TextureFormat.RGB24, false);
            diffuse.ReadPixels(prm.DiffuseReadPixel, 0, 0, false);
            diffuse.wrapMode = TextureWrapMode.Clamp;
            diffuse.Apply();

            return diffuse;
        }

        void Render(RenderTexture rt)
        {
            bakingCamera.targetTexture = rt;
            bakingCamera.Render();
            bakingCamera.targetTexture = null;
        }

        public class Queue<T> : IEnumerable<T>
        {
            readonly LinkedList<T> linkedList = new LinkedList<T>();

            public int Count
            {
                get { return linkedList.Count; }
            }

            public T Dequeue()
            {
                T item = linkedList.First.Value;
                linkedList.Remove(linkedList.First);
                return item;
            }

            public void Enqueue(T item)
            {
                linkedList.AddLast(item);
            }

            public bool Remove(T item)
            {
                return linkedList.Remove(item);
            }

            public bool Remove(Func<T, bool> func)
            {
                var current = linkedList.First;

                while (current != null)
                {
                    if (func(current.Value))
                    {
                        linkedList.Remove(current);
                        return true;
                    }
                    current = current.Next;
                }
                return false;
            }

            public bool Contains(T item)
            {
                return linkedList.Contains(item);
            }

            public void Clear()
            {
                linkedList.Clear();
            }

            public IEnumerator<T> GetEnumerator()
            {
                return ((IEnumerable<T>)this.linkedList).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<T>)this.linkedList).GetEnumerator();
            }
        }

    }


    /// <summary>
    /// 地形烘焙时的贴图大小参数;
    /// </summary>
    [Serializable]
    public struct BakingParameter
    {

        ///// <summary>
        ///// 烘焙时得到的结果边框(需要裁剪的部分,单位 像素);
        ///// </summary>
        //public static readonly float Outline = (TerrainData.CHUNK_HEIGHT / 12);

        /// <summary>
        /// 完整预览整个地图块的摄像机旋转角度;
        /// </summary>
        public static readonly Quaternion CameraRotation = Quaternion.Euler(90, 0, 0);

        /// <summary>
        /// 完整预览整个地图块的摄像机大小;
        /// </summary>
        public static readonly float CameraSize = ((TerrainData.CHUNK_HEIGHT) / 2);

        /// <summary>
        /// 完整预览整个地图块的摄像机比例(W/H);
        /// </summary>
        public static readonly float CameraAspect = ((TerrainData.CHUNK_WIDTH )) / (TerrainData.CHUNK_HEIGHT);

        [SerializeField]
        float textureSize;

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
        public int DiffuseMapWidth { get; private set; }
        public int DiffuseMapHeight { get; private set; }
        public int HeightMapWidth { get; private set; }
        public int HeightMapHeight { get; private set; }

        /// <summary>
        /// 烘焙时的尺寸;
        /// </summary>
        public int rDiffuseMapWidth { get; private set; }
        public int rDiffuseMapHeight { get; private set; }
        public int rHeightMapWidth { get; private set; }
        public int rHeightMapHeight { get; private set; }

        /// <summary>
        /// 裁剪区域;
        /// </summary>
        public Rect DiffuseReadPixel { get; private set; }
        public Rect HeightReadPixel { get; private set; }

        public BakingParameter(float textureSize) : this()
        {
            SetTextureSize(textureSize);
        }

        void SetTextureSize(float size)
        {
            this.DiffuseMapWidth = (int)(TerrainData.CHUNK_WIDTH * size);
            this.DiffuseMapHeight = (int)(TerrainData.CHUNK_HEIGHT * size);
            this.HeightMapWidth = (int)(TerrainData.CHUNK_WIDTH * size);
            this.HeightMapHeight = (int)(TerrainData.CHUNK_HEIGHT * size);

            this.rDiffuseMapWidth = (int)(TerrainData.CHUNK_WIDTH * size);
            this.rDiffuseMapHeight = (int)(TerrainData.CHUNK_HEIGHT * size);
            this.rHeightMapWidth = (int)(TerrainData.CHUNK_WIDTH * size);
            this.rHeightMapHeight = (int)(TerrainData.CHUNK_HEIGHT * size);

            this.DiffuseReadPixel = new Rect(0, 0, DiffuseMapWidth, DiffuseMapHeight);
            this.HeightReadPixel = new Rect(0, 0, HeightMapWidth, HeightMapHeight);

            this.TextureSize = size;
        }

    }

}
