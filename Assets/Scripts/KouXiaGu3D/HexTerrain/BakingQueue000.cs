//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;
//using UnityEngine;
//using System.Collections;

//namespace KouXiaGu.HexTerrain
//{

//    /// <summary>
//    /// 烘焙贴图队列;
//    /// </summary>
//    [DisallowMultipleComponent, RequireComponent(typeof(Camera))]
//    public sealed class BakingQueue : UnitySingleton<BakingQueue>
//    {
//        BakingQueue() { }

//        [SerializeField]
//        Material mixerMaterial;
//        [SerializeField]
//        Material heightMaterial;
//        [SerializeField]
//        Material blurMaterial;
//        [SerializeField]
//        Material shadowsAndHeightMaterial;
//        [SerializeField]
//        Material diffuseMaterial;

//        [SerializeField]
//        GameObject ovenDisplayMesh;
//        [SerializeField]
//        GameObject gameDisplayMesh;

//        Camera bakingCamera;
//        Queue<BakingRequest> bakingQueue;
//        Dictionary<int, MeshRenderer> aroundHexMesh;
//        Coroutine bakingCoroutine;


//        RenderTexture mixerRT;
//        RenderTexture heightRT;
//        RenderTexture heightRTOffset1;
//        RenderTexture heightRTOffset2;
//        RenderTexture shadowsAndHeightRT;
//        RenderTexture diffuseRT;

//        //贴图尺寸;
//        int mixerTextureSize = 500;


//        /// <summary>
//        /// 定义的六边形半径;
//        /// </summary>
//        float HexOuterRadius
//        {
//            get { return HexGrids.OuterRadius; }
//        }


//        /// <summary>
//        /// 添加需要渲染的节点;
//        /// </summary>
//        public void Enqueue(BakingRequest bakingRequest)
//        {
//            bakingQueue.Enqueue(bakingRequest);
//        }


//        void Awake()
//        {
//            bakingCamera = GetComponent<Camera>();
//            bakingQueue = new Queue<BakingRequest>();
//        }

//        void Start()
//        {
//            InitBakingCamera();
//            InitBakingMesh();
//            InitRenderTextures();

//            bakingCoroutine = StartCoroutine(Baking());
//        }

//        /// <summary>
//        /// 设置烘焙相机参数;
//        /// </summary>
//        void InitBakingCamera()
//        {
//            bakingCamera.orthographic = true;
//            bakingCamera.orthographicSize = HexOuterRadius;
//            bakingCamera.transform.position = new Vector3(HexGrids.OriginPixelPoint.x, 5f, HexGrids.OriginPixelPoint.z);
//        }

//        /// <summary>
//        /// 设置可渲染的六边形的网格到场景;
//        /// </summary>
//        void InitBakingMesh()
//        {
//            aroundHexMesh = new Dictionary<int, MeshRenderer>();
//            foreach (var pixelPair in HexGrids.GetNeighboursAndSelf(HexGrids.Origin))
//            {
//                GameObject hexRendererObject = GameObject.Instantiate(ovenDisplayMesh, transform, false) as GameObject;
//                hexRendererObject.name = pixelPair.Key.ToString();
//                hexRendererObject.SetActive(true);
//                hexRendererObject.transform.position = HexGrids.OffsetToPixel(pixelPair.Value);
//                MeshRenderer hexRenderer = hexRendererObject.GetComponent<MeshRenderer>();
//                aroundHexMesh.Add((int)pixelPair.Key, hexRenderer);
//            }
//        }

//        /// <summary>
//        /// 初始化渲染纹理;
//        /// </summary>
//        void InitRenderTextures()
//        {
//            mixerRT = new RenderTexture(mixerTextureSize, mixerTextureSize, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);

//            heightRT = new RenderTexture(mixerTextureSize >> 1, mixerTextureSize >> 1, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
//            heightRT.wrapMode = TextureWrapMode.Clamp;

//            heightRTOffset1 = new RenderTexture(mixerTextureSize >> 1, mixerTextureSize >> 1, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
//            heightRTOffset1.wrapMode = TextureWrapMode.Clamp;

//            heightRTOffset2 = new RenderTexture(mixerTextureSize >> 1, mixerTextureSize >> 1, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
//            heightRTOffset1.wrapMode = TextureWrapMode.Clamp;

//            diffuseRT = new RenderTexture(mixerTextureSize, mixerTextureSize, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
//            diffuseRT.wrapMode = TextureWrapMode.Clamp;
//        }

//        [SerializeField]
//        float Test_Wait = 0f;
//        string Test_Path { get { return @"123"; } }

//        IEnumerator Baking()
//        {
//            CustomYieldInstruction bakingYieldInstruction = new WaitWhile(() => bakingQueue.Count == 0);

//            while (true)
//            {
//                yield return bakingYieldInstruction;

//                BakingRequest bakingRequest = bakingQueue.Dequeue();
//                List<KeyValuePair<HexDirection, Landform>> bakingLandforms = BakingRangeSetting(bakingRequest);
//                BakingMixer(bakingLandforms);
//                yield return new WaitForSeconds(Test_Wait);

//                BakingHeight(bakingLandforms);
//                BlurTexture(heightRT, 1, 1, 1);

//                //BakeTo(heightRTOffset1);
//                //BlurTexture(heightRTOffset1, 1, 1, 1);
//                //BakeTo(heightRTOffset2);
//                //BlurTexture(heightRTOffset2, 1, 1, 1);
//                //shadowsAndHeightRT = ProduceShadowsAndHeightTexture(heightRT, heightRTOffset1, heightRTOffset2);
//                //yield return new WaitForSeconds(Test_Wait);

//                BakingDiffuse(bakingLandforms);
//                yield return new WaitForSeconds(Test_Wait);

//                Texture2D height_Alpha8 = GetHeightTexture(heightRT);

//                // Copy Diffuse to Texture2D            
//                Texture2D diffuse = HexTexture2DCutOut(diffuseRT, TextureFormat.RGB24, false);
//                diffuse.wrapMode = TextureWrapMode.Clamp;
//                diffuse.Apply();

//                //diffuseRT.SavePNG(Test_Path);
//                diffuse.SavePNG(Test_Path);
//                height_Alpha8.SavePNG(Test_Path);

//                Vector3 gameDisplayMeshPoint = HexGrids.OffsetToPixel(bakingRequest.MapPoint);
//                Instantiate(gameDisplayMeshPoint, diffuse, height_Alpha8);

//            }
//        }

//        /// <summary>
//        /// 对烘焙范围进行设置,关闭或开启烘焙的方向;并返回需要进行烘焙的内容;
//        /// </summary>
//        List<KeyValuePair<HexDirection, Landform>> BakingRangeSetting(BakingRequest bakingRequest)
//        {
//            var baking = bakingRequest.GetBakingRange();
//            var bakingLandform = new List<KeyValuePair<HexDirection, Landform>>();
//            foreach (var pair in baking)
//            {
//                MeshRenderer hexMesh = GetHexMesh(pair.Direction);
//                if (pair.Item == null)
//                {
//                    hexMesh.gameObject.SetActive(false);
//                }
//                else
//                {
//                    hexMesh.gameObject.SetActive(true);
//                    hexMesh.transform.position = new Vector3(
//                        hexMesh.transform.position.x,
//                        pair.Point.y,
//                        hexMesh.transform.position.z);
//                    bakingLandform.Add(new KeyValuePair<HexDirection, Landform>(pair.Direction, pair.Item));
//                }
//            }
//            return bakingLandform;
//        }

//        /// <summary>
//        /// 对混合贴图进行烘焙;传入需要设置到的地貌节点;
//        /// </summary>
//        void BakingMixer(IEnumerable<KeyValuePair<HexDirection, Landform>> baking)
//        {
//            foreach (var pair in baking)
//            {
//                Landform landform = pair.Value;
//                MeshRenderer hexMesh = GetHexMesh(pair.Key);
//                if (hexMesh.material != null)
//                    GameObject.Destroy(hexMesh.material);

//                hexMesh.material = mixerMaterial;
//                hexMesh.material.mainTexture = landform.MixerTexture;
//            }

//            mixerRT.Release();
//            bakingCamera.targetTexture = mixerRT;
//            bakingCamera.Render();
//            bakingCamera.targetTexture = null;
//        }

//        void BakingHeight(IEnumerable<KeyValuePair<HexDirection, Landform>> baking)
//        {
//            foreach (var pair in baking)
//            {
//                Landform landform = pair.Value;
//                MeshRenderer hexMesh = GetHexMesh(pair.Key);
//                if (hexMesh.material != null)
//                    GameObject.Destroy(hexMesh.material);

//                hexMesh.material = heightMaterial;
//                hexMesh.material.SetTexture("_MainTex", landform.HeightTexture);
//                hexMesh.material.SetTexture("_Mixer", landform.MixerTexture);
//                hexMesh.material.SetTexture("_GlobalMixer", mixerRT);
//            }

//            heightRT.Release();
//            bakingCamera.targetTexture = heightRT;
//            bakingCamera.Render();
//            bakingCamera.targetTexture = null;
//        }

//        Texture2D GetHeightTexture(RenderTexture renderTexture)
//        {
//            // Copy Height to Texture2D (ARGB) because we cant render directly to Alpha8. 
//            Texture2D height_ARGB32 = HexTexture2DCutOut(renderTexture, TextureFormat.ARGB32, false);
//            height_ARGB32.wrapMode = TextureWrapMode.Clamp;
//            height_ARGB32.Apply();

//            Texture2D height_Alpha8 = HexTexture2D(renderTexture.width, TextureFormat.Alpha8, false);
//            height_Alpha8.wrapMode = TextureWrapMode.Clamp;
//            Color32[] data = height_ARGB32.GetPixels32();

//            height_Alpha8.SetPixels32(data);
//            height_Alpha8.Apply();

//            return height_ARGB32;
//        }


//        void BakingDiffuse(IEnumerable<KeyValuePair<HexDirection, Landform>> baking)
//        {
//            foreach (var pair in baking)
//            {
//                Landform landform = pair.Value;
//                MeshRenderer hexMesh = GetHexMesh(pair.Key);
//                if (hexMesh.material != null)
//                    GameObject.Destroy(hexMesh.material);

//                hexMesh.material = diffuseMaterial;

//                hexMesh.material.SetTexture("_MainTex", landform.DiffuseTexture);
//                hexMesh.material.SetTexture("_Mixer", landform.MixerTexture);
//                hexMesh.material.SetTexture("_Height", landform.HeightTexture);
//                hexMesh.material.SetTexture("_GlobalMixer", mixerRT);
//                hexMesh.material.SetTexture("_ShadowsAndHeight", shadowsAndHeightRT);
//                hexMesh.material.SetFloat("_Sea", 0f);
//                hexMesh.material.SetFloat("_Sea", landform.ID == 30 ? 1f : 0f);
//                hexMesh.material.SetFloat("_Centralization", 1.0f);
//            }

//            diffuseRT.Release();
//            bakingCamera.targetTexture = diffuseRT;
//            bakingCamera.Render();
//            bakingCamera.targetTexture = null;
//        }


//        RenderTexture ProduceShadowsAndHeightTexture(RenderTexture terrainHeight,
//                                          RenderTexture terrainHeightOff1,
//                                          RenderTexture terrainHeightOff2)
//        {
//            shadowsAndHeightMaterial.SetTexture("_Height", terrainHeight);
//            shadowsAndHeightMaterial.SetTexture("_Height1", terrainHeightOff1);
//            shadowsAndHeightMaterial.SetTexture("_Height2", terrainHeightOff2);


//            RenderTexture rt = new RenderTexture(terrainHeight.width, terrainHeight.height, 0, RenderTextureFormat.ARGB32);
//            //simpler baking for older GPUs, in case they have problem with bilinear filtering in render targets

//            terrainHeight.filterMode = FilterMode.Bilinear;
//            terrainHeightOff1.filterMode = FilterMode.Bilinear;
//            terrainHeightOff1.filterMode = FilterMode.Bilinear;
//            rt.filterMode = FilterMode.Bilinear;

//            Graphics.Blit(null, rt, shadowsAndHeightMaterial, 0);

//            shadowsAndHeightMaterial.SetTexture("_Height", null);
//            shadowsAndHeightMaterial.SetTexture("_Height1", null);
//            shadowsAndHeightMaterial.SetTexture("_Height2", null);
//            return rt;
//        }


//        void BakeTo(RenderTexture target)
//        {
//            target.Release();
//            bakingCamera.targetTexture = target;
//            bakingCamera.Render();
//        }

//        void Instantiate(Vector3 position, Texture2D diffuse, Texture2D height)
//        {
//            GameObject displayMesh = Instantiate(gameDisplayMesh) as GameObject;
//            displayMesh.transform.position = position;
//            displayMesh.gameObject.SetActive(true);
//            MeshRenderer mr = displayMesh.GetComponent<MeshRenderer>();
//            mr.material.SetTexture("_MainTex", diffuse);
//            mr.material.SetTexture("_HeightTex", height);
//        }

//        MeshRenderer GetHexMesh(HexDirection direction)
//        {
//            return aroundHexMesh[(int)direction];
//        }

//        Texture2D HexTexture2D(int width, TextureFormat textureFormat, bool mipmap)
//        {
//            Hexagon hexagon = new Hexagon(width / 2);
//            double textureHeight = hexagon.InnerDiameters;
//            Texture2D texture = new Texture2D(width, (int)textureHeight, textureFormat, mipmap);
//            return texture;
//        }

//        /// <summary>
//        /// 剪切到平顶的六边形贴图的大小,传入贴图必须为矩形的;
//        /// </summary>
//        Texture2D HexTexture2DCutOut(RenderTexture renderTexture, TextureFormat textureFormat, bool mipmap)
//        {
//            Hexagon hexagon = new Hexagon(renderTexture.width / 2);
//            double textureHeight = hexagon.InnerDiameters;
//            float y = (float)((renderTexture.width - textureHeight) / 2);

//            RenderTexture.active = renderTexture;
//            Texture2D texture = new Texture2D(renderTexture.width, (int)textureHeight, textureFormat, mipmap);
//            texture.ReadPixels(new Rect(0, y, renderTexture.width, (float)textureHeight), 0, 0, mipmap);
//            texture.Apply();

//            return texture;
//        }


///// <summary>
///// 模糊贴图;
///// </summary>
///// <param name="rendertexture">原贴图;</param>
///// <param name="destTexture">目标贴图;</param>
///// <param name="downSampleNum">[降采样次数]向下采样的次数。此值越大,则采样间隔越大,需要处理的像素点越少,运行速度越快</param>
///// <param name="iterations">[模糊扩散度]进行模糊时，相邻像素点的间隔。此值越大相邻像素间隔越远，图像越模糊。但过大的值会导致失真</param>
///// <param name="spreadSize">[迭代次数]此值越大,则模糊操作的迭代次数越多，模糊效果越好，但消耗越大</param>
//void BlurTexture(RenderTexture rendertexture, int downSampleNum, int iterations, int spreadSize)
//{
//    float widthMod = 1.0f / (1.0f * (1 << downSampleNum));
//    blurMaterial.SetFloat("_Parameter", spreadSize * widthMod);
//    rendertexture.filterMode = FilterMode.Bilinear;

//    int renderWidth = rendertexture.width >> downSampleNum;
//    int renderHeight = rendertexture.height >> downSampleNum;

//    RenderTexture renderBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, rendertexture.format);
//    renderBuffer.filterMode = FilterMode.Bilinear;

//    Graphics.Blit(rendertexture, renderBuffer, blurMaterial, 0);

//    //根据BlurIterations（迭代次数），来进行指定次数的迭代操作  
//    for (int i = 0; i < iterations; i++)
//    {
//        //迭代偏移量参数  
//        float iterationOffs = (i * 1.0f);
//        blurMaterial.SetFloat("_Parameter", spreadSize * widthMod + iterationOffs);

//        //处理Shader的通道1，垂直方向模糊处理 || Pass1,for vertical blur  
//        RenderTexture tempBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, rendertexture.format);
//        Graphics.Blit(renderBuffer, tempBuffer, blurMaterial, 1);
//        RenderTexture.ReleaseTemporary(renderBuffer);
//        renderBuffer = tempBuffer;

//        // 处理Shader的通道2，竖直方向模糊处理 || Pass2,for horizontal blur  
//        tempBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, rendertexture.format);
//        Graphics.Blit(renderBuffer, tempBuffer, blurMaterial, 2);

//        RenderTexture.ReleaseTemporary(renderBuffer);
//        renderBuffer = tempBuffer;
//    }

//    //拷贝最终的renderBuffer到目标纹理，并绘制所有通道的纹理到屏幕  
//    Graphics.Blit(renderBuffer, rendertexture);
//    RenderTexture.ReleaseTemporary(renderBuffer);
//}


//        /// <summary>
//        /// Simple function which adds a bit of the blur to texture. used mostly by the height textures to avoid artifacts
//        /// </summary>
//        /// <param name="texture"></param>
//        /// <param name="downSample"></param>
//        /// <param name="size"></param>
//        /// <param name="interations"></param>
//        /// <returns></returns>
//        void BlurTexture(RenderTexture texture, int downSample, int size, int interations)
//        {
//            float widthMod = 1.0f / (1.0f * (1 << downSample));

//            Material material = new Material(blurMaterial);
//            material.SetVector("_Parameter", new Vector4(size * widthMod, -size * widthMod, 0.0f, 0.0f));
//            texture.filterMode = FilterMode.Bilinear;


//            int rtW = texture.width >> downSample;
//            int rtH = texture.height >> downSample;

//            // downsample
//            RenderTexture rt = new RenderTexture(rtW, rtH, 0, texture.format);
//            //RenderTexture rt = RenderTargetManager.GetNewTexture(rtW, rtH, 0, texture.format);
//            rt.filterMode = FilterMode.Bilinear;

//            Graphics.Blit(texture, rt, material, 0);

//            for (int i = 0; i < interations; i++)
//            {
//                float iterationOffs = (i * 1.0f);
//                material.SetVector("_Parameter", new Vector4(size * widthMod + iterationOffs, -size * widthMod - iterationOffs, 0.0f, 0.0f));

//                // vertical blur
//                RenderTexture rt2 = new RenderTexture(rtW, rtH, 0, texture.format);
//                //RenderTexture rt2 = RenderTargetManager.GetNewTexture(rtW, rtH, 0, texture.format);
//                rt2.filterMode = FilterMode.Bilinear;

//                Graphics.Blit(rt, rt2, material, 1);
//                rt.Release();
//                //RenderTargetManager.ReleaseTexture(rt);
//                rt = rt2;

//                // horizontal blur
//                rt2 = new RenderTexture(rtW, rtH, 0, texture.format);
//                //rt2 = RenderTargetManager.GetNewTexture(rtW, rtH, 0, texture.format);
//                rt2.filterMode = FilterMode.Bilinear;

//                Graphics.Blit(rt, rt2, material, 2);
//                rt.Release();
//                //RenderTargetManager.ReleaseTexture(rt);
//                rt = rt2;
//            }

//            GameObject.Destroy(material);

//            Graphics.Blit(rt, texture);

//            rt.Release();
//            //RenderTargetManager.ReleaseTexture(rt);
//        }

//        //[ContextMenu("保存到")]
//        //void SaveToPng()
//        //{
//        //    RenderTexture renderTexture = new RenderTexture(DiffuseTextureSize, DiffuseTextureSize, 24, RenderTextureFormat.ARGB32);
//        //    bakingCamera.targetTexture = renderTexture;
//        //    bakingCamera.Render();

//        //    Texture2D texture = HexTextureCutOut(renderTexture, TextureFormat.RGB24, false);

//        //    texture.SavePNG(Path.Combine(Application.dataPath, "1123"));

//        //    texture.Compress(false);
//        //    texture.filterMode = FilterMode.Bilinear;
//        //    texture.Apply();

//        //    renderer.material.SetTexture("_MainTex", texture);
//        //}


//    }

//}
