using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{


    public static class Texture2DExtensions
    {

        /// <summary>
        /// UV坐标转换为像素坐标;
        /// </summary>
        public static Pixel UVToPixel(this Texture2D texture, UV uv)
        {
            return TexCoordConvert.ToPixel(uv, texture.width, texture.height);
        }

        /// <summary>
        /// 像素坐标转换为UV坐标;
        /// </summary>
        public static UV PixelToUV(this Texture2D texture, Pixel pixel)
        {
            return TexCoordConvert.ToUV(pixel, texture.width, texture.height);
        }


        public static void SetPixel(this Texture2D texture, UV uv, Color color)
        {
            Pixel coord = texture.UVToPixel(uv);
            texture.SetPixel(coord, color);
        }

        public static void SetPixel(this Texture2D texture, Pixel pixel, Color color)
        {
            texture.SetPixel(pixel.X, pixel.Y, color);
        }


        public static Color GetPixel(this Texture2D texture, UV uv)
        {
            Pixel coord = texture.UVToPixel(uv);
            return texture.GetPixel(coord);
        }

        public static Color GetPixel(this Texture2D texture, Pixel pixel)
        {
            return texture.GetPixel(pixel.X, pixel.Y);
        }


    }

}
