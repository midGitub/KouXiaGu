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
        public Color NormalColor = Color.black;
        public Color SuccessfulColor = Color.green;
        public Color WarningColor = Color.yellow;
        public Color ErrorColor = Color.red;
        public Color MethodColor = Color.blue;

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
