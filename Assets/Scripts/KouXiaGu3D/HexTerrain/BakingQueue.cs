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
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Camera))]
    public sealed class BakingQueue : UnitySingleton<BakingQueue>
    {
        BakingQueue() { }

        [SerializeField]
        Material mixerMaterial;
        [SerializeField]
        Material heightMaterial;
        //[SerializeField]
        //Material shadowsAndHeightMaterial;
        [SerializeField]
        Material diffuseMaterial;

        [SerializeField]
        GameObject ovenDisplayMesh;
        [SerializeField]
        GameObject gameDisplayMesh;

        Camera bakingCamera;
        Queue<BakingRequest> bakingQueue;
        Dictionary<int, MeshRenderer> aroundHexMesh;
        Coroutine bakingCoroutine;


        RenderTexture mixerRT;
        RenderTexture heightRT;
        RenderTexture diffuseRT;

        //贴图尺寸;
        int mixerTextureSize = 500;


        /// <summary>
        /// 定义的六边形半径;
        /// </summary>
        public float HexOuterRadius
        {
            get { return HexGrids.OuterRadius; }
        }


        void Awake()
        {
            bakingCamera = GetComponent<Camera>();
            bakingQueue = new Queue<BakingRequest>();
        }

        void Start()
        {
            InitBakingCamera();
            InitBakingMesh();
            InitRenderTextures();

            bakingCoroutine = StartCoroutine(Baking());
        }

        /// <summary>
        /// 设置烘焙相机参数;
        /// </summary>
        void InitBakingCamera()
        {
            bakingCamera.orthographic = true;
            bakingCamera.orthographicSize = HexOuterRadius;
            bakingCamera.transform.position = new Vector3(HexGrids.OriginPixelPoint.x,5f, HexGrids.OriginPixelPoint.z);
        }

        /// <summary>
        /// 设置六边形的网格到场景;
        /// </summary>
        void InitBakingMesh()
        {
            aroundHexMesh = new Dictionary<int, MeshRenderer>();
            foreach (var pixelPair in HexGrids.GetNeighboursAndSelf(HexGrids.Origin))
            {
                GameObject hexRendererObject = GameObject.Instantiate(ovenDisplayMesh, transform, false) as GameObject;
                hexRendererObject.SetActive(true);
                hexRendererObject.transform.position = HexGrids.OffsetToPixel(pixelPair.Value);
                MeshRenderer hexRenderer = hexRendererObject.GetComponent<MeshRenderer>();
                aroundHexMesh.Add((int)pixelPair.Key, hexRenderer);
            }
        }

        /// <summary>
        /// 初始化渲染纹理;
        /// </summary>
        void InitRenderTextures()
        {
            mixerRT = new RenderTexture(mixerTextureSize, mixerTextureSize, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);

            heightRT = new RenderTexture(mixerTextureSize >> 1, mixerTextureSize >> 1, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            heightRT.wrapMode = TextureWrapMode.Clamp;

            diffuseRT = new RenderTexture(mixerTextureSize, mixerTextureSize, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            diffuseRT.wrapMode = TextureWrapMode.Clamp;
        }

        /// <summary>
        /// 添加需要渲染的节点;
        /// </summary>
        public void Enqueue(BakingRequest bakingRequest)
        {
            bakingQueue.Enqueue(bakingRequest);
        }

        IEnumerator Baking()
        {
            CustomYieldInstruction bakingYieldInstruction = new WaitWhile(() => bakingQueue.Count == 0);

            while (true)
            {
                yield return bakingYieldInstruction;

                BakingRequest bakingNode = bakingQueue.Dequeue();
                IEnumerable<KeyValuePair<HexDirection, Landform>> bakingRange = bakingNode.GetBakingRange();

                List<KeyValuePair<HexDirection, Landform>> bakingLandforms = BakingRangeSetting(bakingRange);

                BakingMixer(bakingLandforms);
                BakingHeight(bakingLandforms);
                BakingDiffuse(bakingLandforms);
            }
        }

        /// <summary>
        /// 对烘焙范围进行设置,关闭或开启烘焙的方向;并返回需要进行烘焙的内容;
        /// </summary>
        List<KeyValuePair<HexDirection, Landform>> BakingRangeSetting(IEnumerable<KeyValuePair<HexDirection, Landform>> baking)
        {
            var bakingLandform = new List<KeyValuePair<HexDirection, Landform>>();
            foreach (var pair in baking)
            {
                MeshRenderer hexMesh = GetHexMesh(pair.Key);
                if (pair.Value == null)
                {
                    hexMesh.gameObject.SetActive(false);
                }
                else
                {
                    hexMesh.gameObject.SetActive(true);
                    bakingLandform.Add(pair);
                }
            }
            return bakingLandform;
        }

        /// <summary>
        /// 对混合贴图进行烘焙;传入需要设置到的地貌节点;
        /// </summary>
        void BakingMixer(IEnumerable<KeyValuePair<HexDirection, Landform>> baking)
        {
            foreach (var pair in baking)
            {
                Landform landform = pair.Value;
                MeshRenderer hexMesh = GetHexMesh(pair.Key);
                if (hexMesh.material != null)
                    GameObject.Destroy(hexMesh.material);

                hexMesh.material = mixerMaterial;
                hexMesh.material.mainTexture = landform.MixerTexture;
            }

            mixerRT.Release();
            bakingCamera.targetTexture = mixerRT;
            bakingCamera.Render();
        }

        void BakingHeight(IEnumerable<KeyValuePair<HexDirection, Landform>> baking)
        {
            foreach (var pair in baking)
            {
                Landform landform = pair.Value;
                MeshRenderer hexMesh = GetHexMesh(pair.Key);
                if (hexMesh.material != null)
                    GameObject.Destroy(hexMesh.material);

                hexMesh.material = heightMaterial;
                hexMesh.material.SetTexture("_MainTex", landform.HeightTexture);
                hexMesh.material.SetTexture("_Mixer", landform.MixerTexture);
                hexMesh.material.SetTexture("_GlobalMixer", mixerRT);
            }

            heightRT.Release();
            bakingCamera.targetTexture = heightRT;
            bakingCamera.Render();
        }

        void BakingDiffuse(IEnumerable<KeyValuePair<HexDirection, Landform>> baking)
        {
            foreach (var pair in baking)
            {
                Landform landform = pair.Value;
                MeshRenderer hexMesh = GetHexMesh(pair.Key);
                if (hexMesh.material != null)
                    GameObject.Destroy(hexMesh.material);

                hexMesh.material = diffuseMaterial;

                hexMesh.material.SetTexture("_MainTex", landform.DiffuseTexture);
                hexMesh.material.SetTexture("_Mixer", landform.MixerTexture);
                hexMesh.material.SetTexture("_Height", landform.HeightTexture);
                hexMesh.material.SetTexture("_GlobalMixer", mixerRT);
                //hexMesh.material.SetTexture("_ShadowsAndHeight", shadowsAndHeightRT);
                //hexMesh.material.SetFloat("_Sea", pair.Key.terrainType.source.seaType ? 1f : 0f);
                hexMesh.material.SetFloat("_Centralization", 1.0f);
            }

            diffuseRT.Release();
            bakingCamera.targetTexture = diffuseRT;
            bakingCamera.Render();
        }


        MeshRenderer GetHexMesh(HexDirection direction)
        {
            return aroundHexMesh[(int)direction];
        }

        /// <summary>
        /// 剪切到平顶的六边形贴图的大小,传入贴图必须为矩形的;
        /// </summary>
        Texture2D HexTextureCutOut(RenderTexture renderTexture, TextureFormat textureFormat, bool mipmap)
        {
            Hexagon hexagon = new Hexagon(renderTexture.width / 2);
            double textureHeight = hexagon.InnerDiameters;
            float y = (float)((renderTexture.width - textureHeight) / 2);

            RenderTexture.active = renderTexture;
            Texture2D texture = new Texture2D(renderTexture.width, (int)textureHeight, textureFormat, mipmap);
            texture.ReadPixels(new Rect(0, y, renderTexture.width, (float)textureHeight), 0, 0, mipmap);
            texture.Apply();

            return texture;
        }


        //[ContextMenu("保存到")]
        //void SaveToPng()
        //{
        //    RenderTexture renderTexture = new RenderTexture(DiffuseTextureSize, DiffuseTextureSize, 24, RenderTextureFormat.ARGB32);
        //    bakingCamera.targetTexture = renderTexture;
        //    bakingCamera.Render();

        //    Texture2D texture = HexTextureCutOut(renderTexture, TextureFormat.RGB24, false);

        //    texture.SavePNG(Path.Combine(Application.dataPath, "1123"));

        //    texture.Compress(false);
        //    texture.filterMode = FilterMode.Bilinear;
        //    texture.Apply();

        //    renderer.material.SetTexture("_MainTex", texture);
        //}


    }

}
