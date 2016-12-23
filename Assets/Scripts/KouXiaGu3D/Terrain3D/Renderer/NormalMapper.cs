using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 法线图转换
    /// </summary>
    public class NormalMapper : MonoBehaviour
    {

        public float strength;
        public Texture2D texture;
        public Material material;

        public Texture2D temp;

        [ContextMenu("NromalMapping")]
        void Test_NormalMap()
        {
            temp = NormalMapFromGrayscale(texture, strength);
            temp.SavePNG(@"123");
        }

        [ContextMenu("灰度图")]
        void Test_Grayscale()
        {
            temp = Grayscale(texture);
            temp.SavePNG(@"123");
        }

        [ContextMenu("Shader")]
        void Test_Shader()
        {
            RenderTexture alphaHeightRT = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32);

            texture.filterMode = FilterMode.Bilinear;
            alphaHeightRT.filterMode = FilterMode.Bilinear;

            Graphics.Blit(texture, alphaHeightRT, material, 0);

            var rt = ImageEffect.GaussianBlur(alphaHeightRT, 1, 1, 1);

            rt.SavePNG(@"123");
            RenderTexture.ReleaseTemporary(alphaHeightRT);
            RenderTexture.ReleaseTemporary(rt);
        }

        void Test_NormalMap2()
        {
            RenderTexture alphaHeightRT = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(texture, alphaHeightRT, material, 0);
        }

        Texture2D NormalMapFromGrayscale(Texture2D source, float strength)
        {
            Texture2D normalTexture = new Texture2D(source.width, source.height, TextureFormat.ARGB32, true);

            int delta = 10;
            float sobel = 1f;

            for (int y = 0; y < normalTexture.height; y++)
            {
                for (int x = 0; x < normalTexture.width; x++)
                {
                    float topLeft = source.GetPixel(x - delta, y + delta).r;
                    float top = source.GetPixel(x, y + delta).r;
                    float topRight = source.GetPixel(x + delta, y + delta).r;
                    float left = source.GetPixel(x - delta, y).r;
                    float self = source.GetPixel(x, y).r;
                    float right = source.GetPixel(x + delta, y).r;
                    float bottomLeft = source.GetPixel(x - delta, y - delta).r;
                    float bottom = source.GetPixel(x, y - delta).r;
                    float bottomRight = source.GetPixel(x + delta, y - delta).r;

                    double dX = (topRight + sobel * right + bottomRight) - (topLeft + sobel * left + bottomLeft);
                    double dY = (bottomLeft + sobel * bottom + bottomRight) - (topLeft + sobel * top + topRight);
                    double dZ = 1;

                    Color color = (new Color((float)dX + 1f, (float)dY + 1f, (float)dZ + 1f)) * 0.5f;
                    color.a = 1;
                    normalTexture.SetPixel(x, y, color);
                }
            }
            normalTexture.Apply();

            return normalTexture;
        }


        Texture2D NormalMap(Texture2D source, float strength)
        {
            Texture2D normalTexture;
            float xLeft;
            float xRight;
            float yUp;
            float yDown;
            float yDelta;
            float xDelta;

            normalTexture = new Texture2D(source.width, source.height, TextureFormat.ARGB32, true);

            for (int y = 0; y < normalTexture.height; y++)
            {
                for (int x = 0; x < normalTexture.width; x++)
                {
                    xLeft = source.GetPixel(Mathf.Clamp(x - 5, 0, normalTexture.width), y).r * strength;
                    xRight = source.GetPixel(Mathf.Clamp(x + 5, 0, normalTexture.width), y).r * strength;
                    yUp = source.GetPixel(x, Mathf.Clamp(y + 5, 0, normalTexture.height)).r * strength;
                    yDown = source.GetPixel(x, Mathf.Clamp(y - 5, 0, normalTexture.height)).r * strength;
                    xDelta = ((xLeft - xRight) + 1) * 0.5f;
                    yDelta = ((yUp - yDown) + 1) * 0.5f;
                    normalTexture.SetPixel(x, y, new Color(xDelta, yDelta, 1.0f * strength, 1.0f));
                }
            }
            normalTexture.Apply();

            return normalTexture;
        }

        Texture2D Grayscale(Texture2D source)
        {
            Texture2D grayscaleTexture = new Texture2D(source.width, source.height, TextureFormat.ARGB32, true);

            for (int y = 0; y < grayscaleTexture.height; y++)
            {
                for (int x = 0; x < grayscaleTexture.width; x++)
                {
                    float grayscale = source.GetPixel(x, y).grayscale;
                    grayscaleTexture.SetPixel(x, y, new Color(grayscale, grayscale, grayscale));
                }
            }
            grayscaleTexture.Apply();

            return grayscaleTexture;
        }

    }

}
