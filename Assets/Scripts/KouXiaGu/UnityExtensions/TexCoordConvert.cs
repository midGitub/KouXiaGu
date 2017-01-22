using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{


    public static class TexCoordConvert
    {

        /// <summary>
        /// UV坐标转换为像素坐标;
        /// </summary>
        public static Pixel ToPixel(Vector2 uv, int width, int height)
        {
            int x = (int)(width * uv.x);
            int y = (int)(height * uv.y);
            return new Pixel(x, y);
        }

        /// <summary>
        /// 像素坐标转换为UV坐标;
        /// </summary>
        public static Vector2 ToUV(Pixel pixel, int width, int height)
        {
            float x = width / (float)pixel.X;
            float y = height / (float)pixel.Y;
            return new Vector2(x, y);
        }

    }

}
