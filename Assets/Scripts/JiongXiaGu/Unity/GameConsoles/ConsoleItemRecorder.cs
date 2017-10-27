using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 将控制台消息写入StringBuilder;
    /// </summary>
    public class ConsoleItemRecorder
    {
        private readonly object asyncLock = new object();

        /// <summary>
        /// 格式控制;
        /// </summary>
        public ConsoleRecordFormat Format { get; private set; }

        /// <summary>
        /// 可变字符字符串;
        /// </summary>
        private readonly StringBuilder stringBuilder;

        /// <summary>
        /// 控制台条目;
        /// </summary>
        private readonly LinkedList<string> items;

        public ConsoleItemRecorder(StringBuilder stringBuilder, ConsoleRecordFormat format)
        {
            this.stringBuilder = stringBuilder;
            Format = format;
        }

        public void WriteMethod(string message)
        {
            string newText = Format.GetMethodFormat(message);
            Append(newText);
        }

        public void Write(string message)
        {
            string newText = Format.GetNormalFormat(message);
            Append(newText);
        }

        public void WriteError(string message)
        {
            string newText = Format.GetErrorFormat(message);
            Append(newText);
        }

        public void WriteSuccessful(string message)
        {
            string newText = Format.GetSuccessfulFormat(message);
            Append(newText);
        }

        public void WriteWarning(string message)
        {
            string newText = Format.GetWarningFormat(message);
            Append(newText);
        }

        private void Append(string message)
        {
            if (!message.EndsWith(Environment.NewLine))
            {
                message += Environment.NewLine;
            }
            lock (asyncLock)
            {
                if (message.Length > stringBuilder.MaxCapacity)
                {
                    message = "[过长的消息,无法显示]";
                }
                if (message.Length + stringBuilder.Length > stringBuilder.MaxCapacity)
                {
                    int removeLength = 0;
                    do
                    {
                        var node = items.First;
                        removeLength += node.Value.Length;
                        items.Remove(node);
                    }
                    while (message.Length + stringBuilder.Length > stringBuilder.MaxCapacity);

                    stringBuilder.Remove(0, removeLength);
                }
                stringBuilder.Append(message);
                items.AddLast(message);
            }
        }

        /// <summary>
        /// 获取到对应文本;
        /// </summary>
        public string GetText()
        {
            lock (asyncLock)
            {
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// 清除记录;
        /// </summary>
        public void Clear()
        {
            lock (asyncLock)
            {
                stringBuilder.Clear();
                items.Clear();
            }
        }
    }
}
