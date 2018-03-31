using System;

namespace JiongXiaGu.Unity.GameConsoles
{
    /// <summary>
    /// 格式输出控制;
    /// </summary>
    public class ConsoleStringFormat
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

        public virtual string GetMethodFormat(string message)
        {
            return message;
        }
    }
}
