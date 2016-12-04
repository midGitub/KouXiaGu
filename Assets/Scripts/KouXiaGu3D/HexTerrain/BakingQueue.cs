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
    public class BakingQueue : MonoBehaviour
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
        GameObject hexMesh;

        Camera bakingCamera;
        Queue<BakingRequest> bakingQueue;
        Dictionary<int, MeshRenderer> aroundHexMesh;
        Coroutine bakingCoroutine;

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
            InitializeBakingCamera();
            InitializeBakingMesh();

            bakingCoroutine = StartCoroutine(Baking());
        }

        /// <summary>
        /// 设置烘焙相机参数;
        /// </summary>
        void InitializeBakingCamera()
        {
            bakingCamera.orthographic = true;
            bakingCamera.orthographicSize = HexOuterRadius;
            bakingCamera.transform.position = new Vector3(HexGrids.OriginPixelPoint.x,5f, HexGrids.OriginPixelPoint.z);
        }

        /// <summary>
        /// 设置六边形的网格到场景;
        /// </summary>
        void InitializeBakingMesh()
        {
            aroundHexMesh = new Dictionary<int, MeshRenderer>();
            foreach (var pixelPair in HexGrids.GetNeighboursAndSelf(HexGrids.Origin))
            {
                GameObject hexRendererObject = GameObject.Instantiate(hexMesh, transform, false) as GameObject;
                hexRendererObject.SetActive(true);
                hexRendererObject.transform.position = HexGrids.OffsetToPixel(pixelPair.Value);
                MeshRenderer hexRenderer = hexRendererObject.GetComponent<MeshRenderer>();
                aroundHexMesh.Add((int)pixelPair.Key, hexRenderer);
            }
        }


        public void Enqueue(BakingRequest bakingNode)
        {
            bakingQueue.Enqueue(bakingNode);
        }


        /// <summary>
        /// 漫反射贴图尺寸;
        /// </summary>
        const int DiffuseTextureSize = 500;

        IEnumerator Baking()
        {
            CustomYieldInstruction bakingYieldInstruction = new WaitWhile(() => bakingQueue.Count == 0);

            while (true)
            {
                yield return bakingYieldInstruction;

                BakingRequest bakingNode = bakingQueue.Dequeue();
                KeyValuePair<HexDirection, Landform>[] bakingRange = bakingNode.BakingRange.ToArray();

                BakingRangeSetting(bakingRange);



            }
        }

        /// <summary>
        /// 对烘焙范围进行设置,关闭或开启烘焙的方向;
        /// </summary>
        void BakingRangeSetting(IEnumerable<KeyValuePair<HexDirection, Landform>> baking)
        {
            foreach (var pair in baking)
            {
                MeshRenderer hexMesh = aroundHexMesh[(int)pair.Key];
                hexMesh.gameObject.SetActive(pair.Value != null);
            }
        }

        /// <summary>
        /// 对混合贴图进行烘焙;
        /// </summary>
        void BakingMixer(IEnumerable<KeyValuePair<HexDirection, Landform>> baking)
        {

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
