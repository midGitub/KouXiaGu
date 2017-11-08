using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 将控制台消息写入StringBuilder(线程安全);
    /// </summary>
    public class ConsoleStringBuilder : IObserver<ConsoleEvent>, IObserver<UnityDebugLogEvent>
    {
        public const string MessageTooLongErrorString = "[过长的消息,无法显示]";

        private readonly object asyncLock = new object();

        /// <summary>
        /// 格式控制;
        /// </summary>
        public ConsoleStringBuilderFormat Format { get; private set; }

        /// <summary>
        /// 可变字符字符串;
        /// </summary>
        private readonly StringBuilder stringBuilder;

        /// <summary>
        /// 控制台条目;
        /// </summary>
        private readonly LinkedList<string> eventLengthList;

        /// <summary>
        /// 是否发生变化;
        /// </summary>
        private bool hasChanged;

        public ConsoleStringBuilder(StringBuilder stringBuilder) : this(stringBuilder, new ConsoleStringBuilderFormat())
        {
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="stringBuilder">指定 StringBuilder,当超出最大容量后删除顶部文本</param>
        /// <param name="format">写入 StringBuilder 的格式定义</param>
        public ConsoleStringBuilder(StringBuilder stringBuilder, ConsoleStringBuilderFormat format)
        {
            if (stringBuilder.MaxCapacity < MessageTooLongErrorString.Length)
                throw new ArgumentException("StringBuilder 最大容量过小;");

            this.stringBuilder = stringBuilder;
            Format = format;
            eventLengthList = new LinkedList<string>();

            string value = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(value))
            {
                eventLengthList.AddLast(stringBuilder.ToString());
            }
            AppendLine();
        }

        void IObserver<ConsoleEvent>.OnNext(ConsoleEvent value)
        {
            switch (value.EventType)
            {
                case ConsoleEventType.Normal:
                    Write(value.Message);
                    break;

                case ConsoleEventType.Successful:
                    WriteSuccessful(value.Message);
                    break;

                case ConsoleEventType.Warning:
                    WriteWarning(value.Message);
                    break;

                case ConsoleEventType.Error:
                    WriteError(value.Message);
                    break;

                case ConsoleEventType.Method:
                    WriteMethod(value.Message);
                    break;

                default:
                    Write(string.Format("[{0}]{1}", value.EventType, value.Message));
                    break;
            }
        }

        void IObserver<ConsoleEvent>.OnCompleted()
        {
            return;
        }

        void IObserver<ConsoleEvent>.OnError(Exception error)
        {
            return;
        }


        void IObserver<UnityDebugLogEvent>.OnNext(UnityDebugLogEvent value)
        {
            switch (value.EventType)
            {
                case LogType.Log:
                case LogType.Assert:
                    Write(value.Message);
                    break;

                case LogType.Warning:
                    WriteWarning(value.Message);
                    break;

                case LogType.Error:
                case LogType.Exception:
                    WriteError(value.Message);
                    break;

                default:
                    Write(string.Format("[{0}]{1}", value.EventType, value.Message));
                    break;
            }
        }

        void IObserver<UnityDebugLogEvent>.OnError(Exception error)
        {
            return;
        }

        void IObserver<UnityDebugLogEvent>.OnCompleted()
        {
            return;
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

        public void WriteMethod(string message)
        {
            string newText = Format.GetMethodFormat(message);
            Append(newText);
        }

        private void Append(string message)
        {
            lock (asyncLock)
            {
                //若消息长度大于合集容量,替换为异常字段;
                if (message.Length > stringBuilder.MaxCapacity)
                {
                    message = MessageTooLongErrorString;
                }

                int messageLength = stringBuilder.Length + message.Length;
                int removeLength = 0;
                while (messageLength - removeLength > stringBuilder.MaxCapacity)
                {
                    var node = eventLengthList.First;
                    removeLength += node.Value.Length;
                    eventLengthList.Remove(node);
                }

                stringBuilder.Remove(0, removeLength);
                stringBuilder.Append(message);
                eventLengthList.AddLast(message);
                hasChanged = true;

                AppendLine();
            }
        }

        /// <summary>
        /// 若最后一个条目结尾不存在换行,则添加换行符;
        /// </summary>
        private void AppendLine()
        {
            var lastNode = eventLengthList.Last;
            if (lastNode != null && !lastNode.Value.EndsWith(Environment.NewLine))
            {
                lastNode.Value += Environment.NewLine;
                stringBuilder.Append(Environment.NewLine);
            }
        }

        /// <summary>
        /// 尝试获取到文本;若未发生变化则返回false,发生变化则返回true;
        /// </summary>
        public bool TryGetText(out string text)
        {
            lock (asyncLock)
            {
                text = stringBuilder.ToString();
                if (hasChanged)
                {
                    hasChanged = false;
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 获取到对应文本(不改变状态值);
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
                eventLengthList.Clear();
            }
        }
    }
}
