using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu
{

    /// <summary>
    /// 颜色拓展;
    /// </summary>
    public static class ColorHelper
    {

        public static Color New(int r, int g, int b)
        {
            return new Color(ToColorSingle(r), ToColorSingle(g), ToColorSingle(b));
        }

        public static Color New(int r, int g, int b, int a)
        {
            return new Color(ToColorSingle(r), ToColorSingle(g), ToColorSingle(b), ToColorSingle(a));
        }

        /// <summary>
        /// 将十六进制颜色转换成颜色实例;
        /// </summary>
        /// <param name="hex">十六进制颜色文本,如"#FFFFFFFF"</param>
        public static Color HexToColor(string hex)
        {
            if (hex.Length == 9)
            {
                char[] charArray = hex.ToCharArray();

                if (charArray[0] != '#')
                {
                    goto Exception;
                }

                float r = HexStringToInt32(charArray, 1, 2).ToColorSingle();
                float g = HexStringToInt32(charArray, 3, 2).ToColorSingle();
                float b = HexStringToInt32(charArray, 5, 2).ToColorSingle();
                float a = HexStringToInt32(charArray, 7, 2).ToColorSingle();
                return new Color(r, g, b, a);
            }
            else if (hex.Length == 8)
            {
                char[] charArray = hex.ToCharArray();
                float r = HexStringToInt32(charArray, 0, 2).ToColorSingle();
                float g = HexStringToInt32(charArray, 2, 2).ToColorSingle();
                float b = HexStringToInt32(charArray, 4, 2).ToColorSingle();
                float a = HexStringToInt32(charArray, 6, 2).ToColorSingle();
                return new Color(r, g, b, a);
            }

            Exception:
            throw new ArgumentException(string.Format("不是合法的十六进制颜色值:[{0}];", hex));
        }

        private static int HexStringToInt32(char[] charArray, int startIndex, int length)
        {
            string hexStr = new string(charArray, startIndex, length);
            return Convert.ToInt32(hexStr, 16);
        }

        /// <summary>
        /// 转换成十六进制表示形式;
        /// </summary>
        public static string ColorToHex(this Color color)
        {
            string str = "#";
            str += Int32ToHexString2(color.r.ToColorInt32());
            str += Int32ToHexString2(color.g.ToColorInt32());
            str += Int32ToHexString2(color.b.ToColorInt32());
            str += Int32ToHexString2(color.a.ToColorInt32());
            return str;
        }

        static string Int32ToHexString2(int i)
        {
            return string.Format("{0:X2}", i);
        }

        static int ToColorInt32(this float f)
        {
            return (int)(f * 255f);
        }

        static float ToColorSingle(this int i)
        {
            return i / 255f;
        }

    }

    /// <summary>
    /// 随机颜色;
    /// </summary>
    public static class RandomColor
    {

        static float Value01
        {
            get { return UnityEngine.Random.value; }
        }

        public static Color Next()
        {
            return Next(Value01);
        }

        public static Color Next(float alpha)
        {
            return new Color(Value01, Value01, Value01, alpha);
        }

        public static float Sample(this System.Random random)
        {
            return (float)random.NextDouble();
        }

        /// <summary>
        /// 根据种子获取到颜色;
        /// </summary>
        public static Color Get(int seed)
        {
            var random = new System.Random(seed);
            return new Color(random.Sample(), random.Sample(), random.Sample(), random.Sample());
        }

        public static Color Get(int seed, float alpha)
        {
            var random = new System.Random(seed);
            return new Color(random.Sample(), random.Sample(), random.Sample(), alpha);
        }
    }

}
