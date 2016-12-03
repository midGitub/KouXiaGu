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


        Camera bakingCamera;

        Queue<BakingNode> bakingQueue;

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
            bakingQueue = new Queue<BakingNode>();
        }

        void Start()
        {
            InitializeBakingCamera();

            bakingCoroutine = StartCoroutine(Baking());
        }

        /// <summary>
        /// 设置烘焙相机参数;
        /// </summary>
        void InitializeBakingCamera()
        {
            bakingCamera.orthographic = true;
            bakingCamera.orthographicSize = HexOuterRadius;
        }


        public void Enqueue(BakingNode bakingNode)
        {
            bakingQueue.Enqueue(bakingNode);
        }

        bool TryDequeue(out BakingNode bakingNode)
        {
            if (bakingQueue.Count == 0)
            {
                bakingNode = default(BakingNode);
                return false;
            }
            else
            {
                bakingNode = bakingQueue.Dequeue();
                return true;
            }
        }


        /// <summary>
        /// 漫反射贴图尺寸;
        /// </summary>
        const int DiffuseTextureSize = 500;

        public Renderer renderer;

        IEnumerator Baking()
        {
            while (true)
            {


                yield return null;
            }
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


        [ContextMenu("保存到")]
        void SaveToPng()
        {
            RenderTexture renderTexture = new RenderTexture(DiffuseTextureSize, DiffuseTextureSize, 24, RenderTextureFormat.ARGB32);
            bakingCamera.targetTexture = renderTexture;
            bakingCamera.Render();

            Texture2D texture = HexTextureCutOut(renderTexture, TextureFormat.RGB24, false);

            texture.SavePNG(Path.Combine(Application.dataPath, "1123"));

            texture.Compress(false);
            texture.filterMode = FilterMode.Bilinear;
            texture.Apply();

            renderer.material.SetTexture("_MainTex", texture);
        }


    }

}
