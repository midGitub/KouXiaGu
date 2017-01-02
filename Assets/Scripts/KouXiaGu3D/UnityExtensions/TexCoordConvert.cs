using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{


    public static class TexCoordConvert
    {

        /// <summary>
        /// UV坐标转换为像素坐标;
        /// </summary>
        public static Pixel ToPixel(UV uv, int width, int height)
        {
            int x = (int)(width * uv.X);
            int y = (int)(height * uv.Y);
            return new Pixel(x, y);
        }

        /// <summary>
        /// 像素坐标转换为UV坐标;
        /// </summary>
        public static UV ToUV(Pixel pixel, int width, int height)
        {
            float x = width / (float)pixel.X;
            float y = height / (float)pixel.Y;
            return new UV(x, y);
        }

    }

}
