﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 颜色拓展;
    /// </summary>
    public static class ColorExtensions
    {

        public static Color HexToColor(string hex)
        {
            if (!hex.StartsWith("#"))
                throw new ArgumentException();

            char[] charArray = hex.ToCharArray();
            float r = HexStringToInt32(charArray, 1, 2).ToColorSingle();
            float g = HexStringToInt32(charArray, 3, 2).ToColorSingle();
            float b = HexStringToInt32(charArray, 5, 2).ToColorSingle();
            float a = HexStringToInt32(charArray, 7, 2).ToColorSingle();
            return new Color(r, g, b, a);
        }

        static int HexStringToInt32(string hexStr)
        {
            return Convert.ToInt32(hexStr, 16);
        }

        static int HexStringToInt32(char[] charArray, int startIndex, int length)
        {
            string hexStr = new string(charArray, startIndex, length);
            return HexStringToInt32(hexStr);
        }


        public static string ToHex(this Color color)
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
            return (int)(f * 255);
        }

        static float ToColorSingle(this int i)
        {
            return i / 255;
        }

    }

}