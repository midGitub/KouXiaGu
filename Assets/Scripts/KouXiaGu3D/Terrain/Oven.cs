using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 烘焙贴图队列;
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Camera))]
    public class Oven : MonoBehaviour
    {
        Oven() { }

        Camera bakingCamera;

        [SerializeField]
        Material mixerMaterial;
        [SerializeField]
        Material heightMaterial;
        [SerializeField]
        Material shadowsAndHeightMaterial;
        [SerializeField]
        Material diffuseMaterial;

        public Renderer renderer;

        void Awake()
        {
            bakingCamera = GetComponent<Camera>();
        }




        const int TextureWidth = 200;
        const int TextureHeight = 200;

        [ContextMenu("保存到")]
        void SaveToPng()
        {
            RenderTexture renderTexture = new RenderTexture(TextureWidth, TextureHeight, 24, RenderTextureFormat.ARGB32);
            bakingCamera.targetTexture = renderTexture;
            bakingCamera.Render();

            RenderTexture.active = renderTexture;
            Texture2D texture = new Texture2D(TextureWidth, 173, TextureFormat.RGB24, false);

            texture.wrapMode = TextureWrapMode.Clamp;
            texture.ReadPixels(new Rect(0, 14f, TextureWidth, 173f), 0, 0, false);
            texture.Apply();
            texture.SavePNG(Path.Combine(Application.dataPath, "1123"));

            texture.Compress(false);
            texture.filterMode = FilterMode.Bilinear;
            texture.Apply();

            renderer.material.SetTexture("_MainTex", texture);
        }


    }

}
