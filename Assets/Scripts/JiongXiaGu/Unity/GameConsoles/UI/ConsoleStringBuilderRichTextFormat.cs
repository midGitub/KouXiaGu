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

        /// <summary>
        /// 若文本结尾为换行符,则删除它;
        /// </summary>
        private string RemoveNewLine(string message)
        {
            if (message.EndsWith(Environment.NewLine))
            {
                message = message.Remove(message.Length - Environment.NewLine.Length, Environment.NewLine.Length);
            }
            return message;
        }

        public override string GetNormalFormat(string message)
        {
            message = base.GetNormalFormat(message);
            message = RemoveNewLine(message);
            return SetColor(message, NormalColor);
        }

        public override string GetSuccessfulFormat(string message)
        {
            message = base.GetSuccessfulFormat(message);
            message = RemoveNewLine(message);
            return SetColor(message, SuccessfulColor);
        }

        public override string GetWarningFormat(string message)
        {
            message = base.GetWarningFormat(message);
            message = RemoveNewLine(message);
            return SetColor(message, WarningColor);
        }

        public override string GetErrorFormat(string message)
        {
            message = base.GetErrorFormat(message);
            message = RemoveNewLine(message);
            return SetColor(message, ErrorColor);
        }

        public override string GetMethodFormat(string message)
        {
            message = base.GetMethodFormat(message);
            message = RemoveNewLine(message);
            return SetColor(message, MethodColor);
        }

        string SetColor(string message, Color color)
        {
            return message.SetColor(color);
        }
    }
}
