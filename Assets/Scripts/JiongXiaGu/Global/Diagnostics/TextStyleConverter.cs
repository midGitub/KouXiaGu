using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.Diagnostics
{

    public class TextStyleConverter
    {
        public virtual string GetNormal(string message)
        {
            return message;
        }

        public virtual string GetWarning(string message)
        {
            return message;
        }

        public virtual string GetError(string message)
        {
            return message;
        }
    }

    /// <summary>
    /// 使用"RichText(富文本)"控制输出格式;
    /// </summary>
    [Serializable]
    public class RichTextStyleConverter : TextStyleConverter
    {
        public Color NormalColor = Color.black;
        public Color WarningColor = Color.yellow;
        public Color ErrorColor = Color.red;

        public override string GetNormal(string message)
        {
            return SetColor(message, NormalColor);
        }

        public override string GetWarning(string message)
        {
            return SetColor(message, WarningColor);
        }

        public override string GetError(string message)
        {
            return SetColor(message, ErrorColor);
        }

        string SetColor(string message, Color color)
        {
            string col = color.ColorToHex();
            message = "<color=" + col + ">" + message + "</color>";
            return message;
        }
    }
}
