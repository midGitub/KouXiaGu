using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 基本贴图信息渲染,负责将传入的请求渲染出基本的高度图和地貌贴图;
    /// </summary>
    [DisallowMultipleComponent, CustomEditor]
    public sealed class BasicRenderer : UnitySingleton<BasicRenderer>
    {
        BasicRenderer() { }

        /// <summary>
        /// 负责渲染地形的摄像机;
        /// </summary>
        [SerializeField]
        Camera bakingCamera;
        /// <summary>
        /// 在烘焙过程中放置在场景内的物体;
        /// </summary>
        [SerializeField]
        RenderDisplayMeshPool ovenDisplayMeshPool;

        [SerializeField, HideInInspector]
        BakingParameter parameter = BakingParameter.Default;

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

        Material mixerMaterial;
        Material heightMaterial;
        Material heightToAlphaMaterial;
        Material diffuseMaterial;
        Material blurMaterial;

        Queue<RenderRequest> bakingQueue;
        Coroutine bakingCoroutine;

        public bool IsRunning
        {
            get { return bakingCoroutine != null; }
        }

        [ExposeProperty]
        public float TextureSize
        {
            get { return parameter.TextureSize; }
            set { parameter = new BakingParameter(value); }
        }


        void Awake()
        {
            ovenDisplayMeshPool.Awake();
            bakingQueue = new Queue<RenderRequest>();
        }

        void Start()
        {
            InitBakingCamera();
            InitMaterial();

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
        /// 加入到烘焙队列;
        /// </summary>
        public void Enqueue(RenderRequest request)
        {
            bakingQueue.Enqueue(request);
        }

        /// <summary>
        /// 初始化烘焙相机参数;
        /// </summary>
        void InitBakingCamera()
        {
            bakingCamera.aspect = BakingParameter.CameraAspect;
            bakingCamera.orthographicSize = BakingParameter.CameraSize;
            bakingCamera.transform.rotation = BakingParameter.CameraRotation;
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

                try
                {
                    RenderRequest request = bakingQueue.Dequeue();
                    Baking(request);
                }
                catch (Exception e)
                {
                    Debug.LogError("地形烘焙错误:" + e);
                }
            }
        }

        /// <summary>
        /// 立即烘焙这个请求;
        /// </summary>
        public void Baking(RenderRequest request)
        {
            IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes = PrepareBaking(request);

            RenderTexture mixerRT;
            RenderTexture heightRT;
            RenderTexture alphaHeightRT;
            RenderTexture diffuseRT;

            mixerRT = BakingMixer(bakingNodes);
            heightRT = BakingHeight(bakingNodes, mixerRT);
            //BlurTexture(heightRT, 1, 1, 1);
            alphaHeightRT = BakingHeightToAlpha(heightRT);
            diffuseRT = BakingDiffuse(bakingNodes, mixerRT, alphaHeightRT);

            Texture2D height = GetHeightTexture(alphaHeightRT);
            Texture2D diffuse = GetDiffuseTexture(diffuseRT);

            request.OnComplete(diffuse, height);

            RenderTexture.ReleaseTemporary(mixerRT);
            RenderTexture.ReleaseTemporary(heightRT);
            RenderTexture.ReleaseTemporary(alphaHeightRT);
            RenderTexture.ReleaseTemporary(diffuseRT);
        }


        /// <summary>
        /// 烘焙前的准备,返回烘焙对应的网格;
        /// </summary>
        List<KeyValuePair<BakingNode, MeshRenderer>> PrepareBaking(RenderRequest request)
        {
            bakingCamera.transform.position = request.CameraPosition;

            IEnumerable<BakingNode> bakingNodes = request.GetBakingNodes();
            List<KeyValuePair<BakingNode, MeshRenderer>> list = new List<KeyValuePair<BakingNode, MeshRenderer>>();

            ovenDisplayMeshPool.RecoveryActive();

            foreach (var node in bakingNodes)
            {
                if (node.NotBoundary)
                {
                    var mesh = ovenDisplayMeshPool.Dequeue(node.Position, node.RotationY);

                    list.Add(new KeyValuePair<BakingNode, MeshRenderer>(node, mesh));
                }
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

            RenderTexture mixerRT = RenderTexture.GetTemporary(parameter.DiffuseMapWidth, parameter.DiffuseMapHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
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

            RenderTexture heightRT = RenderTexture.GetTemporary(parameter.HeightMapWidth, parameter.HeightMapHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
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
                hexMesh.material.SetTexture("_GlobalMixer", mixer);
                hexMesh.material.SetTexture("_ShadowsAndHeight", height);
                hexMesh.material.SetFloat("_Sea", 0f);
                hexMesh.material.SetFloat("_Centralization", 1.0f);
            }

            RenderTexture diffuseRT = RenderTexture.GetTemporary(parameter.DiffuseMapWidth, parameter.DiffuseMapHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
            Render(diffuseRT);
            return diffuseRT;
        }



        Texture2D GetHeightTexture(RenderTexture renderTexture)
        {
            RenderTexture.active = renderTexture;
            Texture2D height_ARGB32 = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            height_ARGB32.ReadPixels(new Rect(0, 0, renderTexture.width, (float)renderTexture.height), 0, 0, false);
            height_ARGB32.wrapMode = TextureWrapMode.Clamp;
            height_ARGB32.Apply();

            Texture2D height_Alpha8 = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.Alpha8, false);
            height_Alpha8.wrapMode = TextureWrapMode.Clamp;
            Color32[] data = height_ARGB32.GetPixels32();

            height_Alpha8.SetPixels32(data);
            height_Alpha8.Apply();

            return height_Alpha8;
        }

        Texture2D GetDiffuseTexture(RenderTexture renderTexture)
        {
            RenderTexture.active = renderTexture;
            Texture2D diffuse = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            diffuse.ReadPixels(new Rect(0, 0, renderTexture.width, (float)renderTexture.height), 0, 0, false);
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

        /// <summary>
        /// 模糊贴图;
        /// </summary>
        /// <param name="rendertexture">原贴图;</param>
        /// <param name="destTexture">目标贴图;</param>
        /// <param name="downSampleNum">[降采样次数]向下采样的次数。此值越大,则采样间隔越大,需要处理的像素点越少,运行速度越快</param>
        /// <param name="iterations">[模糊扩散度]进行模糊时，相邻像素点的间隔。此值越大相邻像素间隔越远，图像越模糊。但过大的值会导致失真</param>
        /// <param name="spreadSize">[迭代次数]此值越大,则模糊操作的迭代次数越多，模糊效果越好，但消耗越大</param>
        void BlurTexture(RenderTexture rendertexture, int downSampleNum, int iterations, int spreadSize)
        {
            float widthMod = 1.0f / (1.0f * (1 << downSampleNum));
            blurMaterial.SetFloat("_DownSampleValue", spreadSize * widthMod);
            rendertexture.filterMode = FilterMode.Bilinear;

            int renderWidth = rendertexture.width >> downSampleNum;
            int renderHeight = rendertexture.height >> downSampleNum;

            RenderTexture renderBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, rendertexture.format);
            renderBuffer.filterMode = FilterMode.Bilinear;

            Graphics.Blit(rendertexture, renderBuffer, blurMaterial, 0);

            //根据BlurIterations（迭代次数），来进行指定次数的迭代操作  
            for (int i = 0; i < iterations; i++)
            {
                //迭代偏移量参数  
                float iterationOffs = (i * 1.0f);
                blurMaterial.SetFloat("_DownSampleValue", spreadSize * widthMod + iterationOffs);

                //处理Shader的通道1，垂直方向模糊处理 || Pass1,for vertical blur  
                RenderTexture tempBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, rendertexture.format);
                Graphics.Blit(renderBuffer, tempBuffer, blurMaterial, 1);
                RenderTexture.ReleaseTemporary(renderBuffer);
                renderBuffer = tempBuffer;

                // 处理Shader的通道2，竖直方向模糊处理 || Pass2,for horizontal blur  
                tempBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, rendertexture.format);
                Graphics.Blit(renderBuffer, tempBuffer, blurMaterial, 2);

                RenderTexture.ReleaseTemporary(renderBuffer);
                renderBuffer = tempBuffer;
            }

            //拷贝最终的renderBuffer到目标纹理，并绘制所有通道的纹理到屏幕  
            Graphics.Blit(renderBuffer, rendertexture);
            RenderTexture.ReleaseTemporary(renderBuffer);
        }


        //void BlurTexture(RenderTexture texture, int downSample, int size, int interations)
        //{
        //    float widthMod = 1.0f / (1.0f * (1 << downSample));

        //    Material material = new Material(blurMaterial);
        //    material.SetVector("_Parameter", new Vector4(size * widthMod, -size * widthMod, 0.0f, 0.0f));
        //    texture.filterMode = FilterMode.Bilinear;


        //    int rtW = texture.width >> downSample;
        //    int rtH = texture.height >> downSample;

        //    // downsample
        //    RenderTexture rt = new RenderTexture(rtW, rtH, 0, texture.format);
        //    //RenderTexture rt = RenderTargetManager.GetNewTexture(rtW, rtH, 0, texture.format);
        //    rt.filterMode = FilterMode.Bilinear;

        //    Graphics.Blit(texture, rt, material, 0);

        //    for (int i = 0; i < interations; i++)
        //    {
        //        float iterationOffs = (i * 1.0f);
        //        material.SetVector("_Parameter", new Vector4(size * widthMod + iterationOffs, -size * widthMod - iterationOffs, 0.0f, 0.0f));

        //        // vertical blur
        //        RenderTexture rt2 = new RenderTexture(rtW, rtH, 0, texture.format);
        //        //RenderTexture rt2 = RenderTargetManager.GetNewTexture(rtW, rtH, 0, texture.format);
        //        rt2.filterMode = FilterMode.Bilinear;

        //        Graphics.Blit(rt, rt2, material, 1);
        //        rt.Release();
        //        //RenderTargetManager.ReleaseTexture(rt);
        //        rt = rt2;

        //        // horizontal blur
        //        rt2 = new RenderTexture(rtW, rtH, 0, texture.format);
        //        //rt2 = RenderTargetManager.GetNewTexture(rtW, rtH, 0, texture.format);
        //        rt2.filterMode = FilterMode.Bilinear;

        //        Graphics.Blit(rt, rt2, material, 2);
        //        rt.Release();
        //        //RenderTargetManager.ReleaseTexture(rt);
        //        rt = rt2;
        //    }

        //    GameObject.Destroy(material);

        //    Graphics.Blit(rt, texture);

        //    rt.Release();
        //    //RenderTargetManager.ReleaseTexture(rt);
        //}

    }


    /// <summary>
    /// 地形烘焙时的贴图大小参数;
    /// </summary>
    [Serializable]
    public struct BakingParameter
    {

        /// <summary>
        /// 完整预览整个地图块的摄像机旋转角度;
        /// </summary>
        public static readonly Quaternion CameraRotation = Quaternion.Euler(90, 0, 0);

        /// <summary>
        /// 完整预览整个地图块的摄像机大小;
        /// </summary>
        public static readonly float CameraSize = TerrainBlock.BlockHeight / 2;

        /// <summary>
        /// 完整预览整个地图块的摄像机比例(W/H);
        /// </summary>
        public static readonly float CameraAspect = TerrainBlock.BlockWidth / TerrainBlock.BlockHeight;

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

        public int DiffuseMapWidth { get; private set; }
        public int DiffuseMapHeight { get; private set; }
        public int HeightMapWidth { get; private set; }
        public int HeightMapHeight { get; private set; }


        public BakingParameter(float textureSize) : this()
        {
            SetTextureSize(textureSize);
        }

        void SetTextureSize(float size)
        {
            this.DiffuseMapWidth = (int)(TerrainBlock.BlockWidth * size);
            this.DiffuseMapHeight = (int)(TerrainBlock.BlockHeight * size);
            this.HeightMapWidth = (int)(TerrainBlock.BlockWidth * size);
            this.HeightMapHeight = (int)(TerrainBlock.BlockHeight * size);
            this.TextureSize = size;
        }


        readonly static BakingParameter defaultParameter = new BakingParameter(150);

        /// <summary>
        /// 默认的参数;
        /// </summary>
        public static BakingParameter Default
        {
            get { return defaultParameter; }
        }

    }

}
