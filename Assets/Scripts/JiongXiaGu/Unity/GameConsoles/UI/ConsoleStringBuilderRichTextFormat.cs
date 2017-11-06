using System;
using UnityEngine;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 使用"RichText(富文本)"控制输出格式;
    /// </summary>
    [Serializable]
    public class ConsoleStringBuilderRichTextFormat : ConsoleStringBuilderFormat
    {
        public Color NormalColor = ColorHelper.HexToColor("#FFFFFFFF");
        public Color SuccessfulColor = ColorHelper.HexToColor("#74FF00FF");
        public Color WarningColor = ColorHelper.HexToColor("#FFE304FF");
        public Color ErrorColor = ColorHelper.HexToColor("#FF3500FF");
        public Color MethodColor = ColorHelper.HexToColor("#00B5FFFF");

        public override string GetNormalFormat(string message)
        {
            return SetColor(message, NormalColor);
        }

        public override string GetSuccessfulFormat(string message)
        {
            return SetColor(message, SuccessfulColor);
        }

        public override string GetWarningFormat(string message)
        {
            return SetColor(message, WarningColor);
        }

        public override string GetErrorFormat(string message)
        {
            return SetColor(message, ErrorColor);
        }

        public override string GetMethodFormat(string method)
        {
            return SetColor(method, MethodColor);
        }

        string SetColor(string message, Color color)
        {
            return message.SetColor(color);
        }
    }
}
