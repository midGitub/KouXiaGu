using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 富文本拓展;
    /// </summary>
    public static class RichTextExtesions
    {


        /// <summary>
        /// 设置文本颜色;
        /// </summary>
        public static string SetColor(this string text, Color color)
        {
            string hex = color.ColorToHex();
            return text.SetColor(hex);
        }

        /// <summary>
        /// 设置文本颜色;
        /// </summary>
        public static string SetColor(this string text, string hex)
        {
            text = string.Format("<color={0}>{1}</color>", hex, text);
            return text;
        }
    }
}
