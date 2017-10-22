using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 将控制台条目写入StringBuilder;
    /// </summary>
    public class ConsoleRecord
    {
        public StringBuilder StringBuilder { get; private set; }
    }

    /// <summary>
    /// 格式输出控制;
    /// </summary>
    public class ConsoleRecordFormat
    {
        public virtual string GetNormalFormat(string message)
        {
            return message;
        }

        public virtual string GetSuccessfulFormat(string message)
        {
            return message;
        }

        public virtual string GetWarningFormat(string message)
        {
            return message;
        }

        public virtual string GetErrorFormat(string message)
        {
            return message;
        }

        public virtual string GetMethodFormat(string method)
        {
            return method;
        }
    }

    /// <summary>
    /// 使用"RichText(富文本)"控制输出格式;
    /// </summary>
    [Serializable]
    public class RichTextConsoleRecordFormat : ConsoleRecordFormat
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
            string col = color.ColorToHex();
            message = "<color=" + col + ">" + message + "</color>";
            return message;
        }
    }
}
