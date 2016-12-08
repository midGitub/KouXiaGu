using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Collections;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 烘焙贴图队列;
    /// 负责将传入的请求渲染出高度图和地貌贴图输出;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class BakingQueue : UnitySingleton<BakingQueue>
    {
        BakingQueue() { }

        /// <summary>
        /// 负责渲染地形的摄像机;
        /// </summary>
        [SerializeField]
        Camera bakingCamera;
        [SerializeField]
        OvenDisplayMeshQueue ovenDisplayMeshPool;

        [SerializeField]
        Material mixerMaterial;
        [SerializeField]
        Material heightMaterial;
        [SerializeField]
        Material blurMaterial;
        [SerializeField]
        Material shadowsAndHeightMaterial;
        [SerializeField]
        Material diffuseMaterial;

        BakingParameter parameter = BakingParameter.Default;

        RenderTexture mixerRT;
        RenderTexture heightRT;
        RenderTexture heightRTOffset1;
        RenderTexture heightRTOffset2;
        RenderTexture shadowsAndHeightRT;
        RenderTexture diffuseRT;

        Queue<BakingRequest> bakingQueue;

        [SerializeField]
        MeshRenderer test_Mesh;

        void Awake()
        {
            ovenDisplayMeshPool.Awake();
            bakingQueue = new Queue<BakingRequest>();
        }

        void Start()
        {
            InitBakingCamera();

            StartCoroutine(Baking());
        }

        /// <summary>
        /// 加入到烘焙队列;
        /// </summary>
        public void Enqueue(BakingRequest request)
        {
            bakingQueue.Enqueue(request);
        }

        /// <summary>
        /// 初始化烘焙相机参数;
        /// </summary>
        void InitBakingCamera()
        {
            bakingCamera.aspect = BakingBlock.CameraAspect;
            bakingCamera.orthographicSize = BakingBlock.CameraSize;
            bakingCamera.transform.rotation = BakingBlock.CameraRotation;
        }

        public string TestPath
        {
            get { return Path.Combine(Application.dataPath, "TestTexture"); }
        }

        IEnumerator Baking()
        {
            CustomYieldInstruction bakingYieldInstruction = new WaitWhile(() => bakingQueue.Count == 0);

            while (true)
            {
                yield return bakingYieldInstruction;


                BakingRequest request = bakingQueue.Dequeue();
                IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes = PrepareBaking(request);

                BakingMixer(bakingNodes);

                BakingHeight(bakingNodes, ref heightRT);
                BlurTexture(heightRT, 1, 1, 1);

                //heightRT.SavePNG();

                BakingHeight(bakingNodes, ref heightRTOffset1, true);
                BlurTexture(heightRTOffset1, 1, 1, 1);

                BakingHeight(bakingNodes, ref heightRTOffset2, true);
                BlurTexture(heightRTOffset2, 1, 1, 1);

                ProduceShadowsAndHeightTexture(heightRT, heightRTOffset1, heightRTOffset2);

                BakingDiffuse(bakingNodes);

                Texture2D height = GetHeightTexture(shadowsAndHeightRT);
                Texture2D diffuse = GetDiffuseTexture(diffuseRT);

                //diffuse.SavePNG(TestPath);

                MeshRenderer tt = Instantiate(test_Mesh, BakingBlock.BlockCoordToPixelCenter(request.BlockCoord), Quaternion.identity) as MeshRenderer;
                tt.gameObject.SetActive(true);
                tt.material.SetTexture("_HeightTex", height);
                tt.material.SetTexture("_MainTex", diffuse);
            }
        }


        /// <summary>
        /// 烘焙前的准备,返回烘焙对应的网格;
        /// </summary>
        List<KeyValuePair<BakingNode, MeshRenderer>> PrepareBaking(BakingRequest request)
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
        void BakingMixer(IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes)
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

            RenderTexture.ReleaseTemporary(mixerRT);
            mixerRT = RenderTexture.GetTemporary(parameter.DiffuseMapWidth, parameter.DiffuseMapHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
            Render(mixerRT);
        }

        /// <summary>
        /// 混合高度贴图;
        /// </summary>
        void BakingHeight(IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes, ref RenderTexture heightRT, bool bakingOnly = false)
        {
            if (!bakingOnly)
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
                    hexMesh.material.SetTexture("_GlobalMixer", mixerRT);
                }
            }

            RenderTexture.ReleaseTemporary(heightRT);
            heightRT = RenderTexture.GetTemporary(parameter.DiffuseMapWidth, parameter.DiffuseMapHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
            Render(heightRT);
        }


        void BakingDiffuse(IEnumerable<KeyValuePair<BakingNode, MeshRenderer>> bakingNodes)
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
                hexMesh.material.SetTexture("_GlobalMixer", mixerRT);
                hexMesh.material.SetTexture("_ShadowsAndHeight", shadowsAndHeightRT);
                hexMesh.material.SetFloat("_Sea", 0f);
                hexMesh.material.SetFloat("_Centralization", 1.0f);
            }

            RenderTexture.ReleaseTemporary(diffuseRT);
            diffuseRT = RenderTexture.GetTemporary(parameter.DiffuseMapWidth, parameter.DiffuseMapHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
            Render(diffuseRT);
        }



        Texture2D GetHeightTexture(RenderTexture renderTexture)
        {
            RenderTexture.active = renderTexture;
            Texture2D height_ARGB32 = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            height_ARGB32.ReadPixels(new Rect(0, 0, renderTexture.width, (float)renderTexture.height), 0, 0, false);
            height_ARGB32.wrapMode = TextureWrapMode.Clamp;
            height_ARGB32.Apply();

            //RenderTexture.active = null;
            Texture2D height_Alpha8 = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.Alpha8, false);
            //height_Alpha8.ReadPixels(new Rect(0, 0, renderTexture.width, (float)renderTexture.height), 0, 0, false);
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


        void ProduceShadowsAndHeightTexture(RenderTexture terrainHeight,
                                          RenderTexture terrainHeightOff1,
                                          RenderTexture terrainHeightOff2)
        {
            shadowsAndHeightMaterial.SetTexture("_Height", terrainHeight);
            shadowsAndHeightMaterial.SetTexture("_Height1", terrainHeightOff1);
            shadowsAndHeightMaterial.SetTexture("_Height2", terrainHeightOff2);

            RenderTexture.ReleaseTemporary(shadowsAndHeightRT);
            shadowsAndHeightRT = RenderTexture.GetTemporary(terrainHeight.width, terrainHeight.height, 0, RenderTextureFormat.ARGB32);

            terrainHeight.filterMode = FilterMode.Bilinear;
            terrainHeightOff1.filterMode = FilterMode.Bilinear;
            terrainHeightOff1.filterMode = FilterMode.Bilinear;
            shadowsAndHeightRT.filterMode = FilterMode.Bilinear;

            Graphics.Blit(null, shadowsAndHeightRT, shadowsAndHeightMaterial, 0);

            shadowsAndHeightMaterial.SetTexture("_Height", null);
            shadowsAndHeightMaterial.SetTexture("_Height1", null);
            shadowsAndHeightMaterial.SetTexture("_Height2", null);
        }

        void BlurTexture(RenderTexture texture, int downSample, int size, int interations)
        {
            float widthMod = 1.0f / (1.0f * (1 << downSample));

            Material material = new Material(blurMaterial);
            material.SetVector("_Parameter", new Vector4(size * widthMod, -size * widthMod, 0.0f, 0.0f));
            texture.filterMode = FilterMode.Bilinear;


            int rtW = texture.width >> downSample;
            int rtH = texture.height >> downSample;

            // downsample
            RenderTexture rt = new RenderTexture(rtW, rtH, 0, texture.format);
            //RenderTexture rt = RenderTargetManager.GetNewTexture(rtW, rtH, 0, texture.format);
            rt.filterMode = FilterMode.Bilinear;

            Graphics.Blit(texture, rt, material, 0);

            for (int i = 0; i < interations; i++)
            {
                float iterationOffs = (i * 1.0f);
                material.SetVector("_Parameter", new Vector4(size * widthMod + iterationOffs, -size * widthMod - iterationOffs, 0.0f, 0.0f));

                // vertical blur
                RenderTexture rt2 = new RenderTexture(rtW, rtH, 0, texture.format);
                //RenderTexture rt2 = RenderTargetManager.GetNewTexture(rtW, rtH, 0, texture.format);
                rt2.filterMode = FilterMode.Bilinear;

                Graphics.Blit(rt, rt2, material, 1);
                rt.Release();
                //RenderTargetManager.ReleaseTexture(rt);
                rt = rt2;

                // horizontal blur
                rt2 = new RenderTexture(rtW, rtH, 0, texture.format);
                //rt2 = RenderTargetManager.GetNewTexture(rtW, rtH, 0, texture.format);
                rt2.filterMode = FilterMode.Bilinear;

                Graphics.Blit(rt, rt2, material, 2);
                rt.Release();
                //RenderTargetManager.ReleaseTexture(rt);
                rt = rt2;
            }

            GameObject.Destroy(material);

            Graphics.Blit(rt, texture);

            rt.Release();
            //RenderTargetManager.ReleaseTexture(rt);
        }




        /// <summary>
        /// 在烘焙时显示在场景内的网格;
        /// </summary>
        [Serializable]
        class OvenDisplayMeshQueue
        {
            OvenDisplayMeshQueue() { }

            [SerializeField]
            Transform parent;

            [SerializeField]
            MeshRenderer ovenDisplayPrefab;

            Queue<MeshRenderer> sleep;
            Queue<MeshRenderer> active;

            float rotationX;

            /// <summary>
            /// 激活在场景的物体;
            /// </summary>
            public IEnumerable<MeshRenderer> Active
            {
                get { return active; }
            }

            public void Awake()
            {
                sleep = new Queue<MeshRenderer>();
                active = new Queue<MeshRenderer>();
                rotationX = ovenDisplayPrefab.transform.rotation.eulerAngles.x;
            }

            /// <summary>
            /// 回收所有激活的物体(将所有激活的物体设为睡眠模式);
            /// </summary>
            public void RecoveryActive()
            {
                while (active.Count != 0)
                {
                    var item = active.Dequeue();
                    Destroy(item);
                }
            }

            /// <summary>
            /// 获取到一个网格物体;
            /// </summary>
            public MeshRenderer Dequeue(Vector3 position, float rotationY)
            {
                MeshRenderer mesh;
                Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
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

            void Destroy(MeshRenderer mesh)
            {
                mesh.gameObject.SetActive(false);
                sleep.Enqueue(mesh);
            }

        }

    }


    /// <summary>
    /// 烘焙的参数;
    /// </summary>
    public struct BakingParameter
    {

        public BakingParameter(float textureSize) : this()
        {
            SetTextureSize(textureSize);
        }

        public float textureSize { get; private set; }

        void SetTextureSize(float size)
        {
            this.DiffuseMapWidth = (int)(BakingBlock.BlockWidth * size);
            this.DiffuseMapHeight = (int)(BakingBlock.BlockHeight * size);
            this.HeightMapWidth = (int)(BakingBlock.BlockWidth * size);
            this.HeightMapHeight = (int)(BakingBlock.BlockHeight * size);
            this.textureSize = size;
        }

        public int DiffuseMapWidth { get; private set; }
        public int DiffuseMapHeight { get; private set; }
        public int HeightMapWidth { get; private set; }
        public int HeightMapHeight { get; private set; }


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
