using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 将控制台消息写入StringBuilder(线程安全);
    /// </summary>
    public class ConsoleStringBuilder : IObserver<ConsoleEvent>
    {
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
        private readonly LinkedList<int> eventLengthList;

        public ConsoleStringBuilder(StringBuilder stringBuilder)
        {
            this.stringBuilder = stringBuilder;
            Format = new ConsoleStringBuilderFormat();
        }

        public ConsoleStringBuilder(StringBuilder stringBuilder, ConsoleStringBuilderFormat format)
        {
            this.stringBuilder = stringBuilder;
            Format = format;
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
                    throw new ArgumentException(string.Format("未知的类型:{0}", value.EventType));
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

            if (message.Length > stringBuilder.MaxCapacity)
            {
                goto MessageTooLong;
            }

            if (message.Length + stringBuilder.Length > stringBuilder.MaxCapacity)
            {
                int removeLength = 0;
                do
                {
                    var node = eventLengthList.First;
                    if (node == null)
                    {
                        goto MessageTooLong;
                    }
                    else
                    {
                        removeLength += node.Value;
                        eventLengthList.Remove(node);
                    }
                }
                while (message.Length + stringBuilder.Length - removeLength > stringBuilder.MaxCapacity);

                stringBuilder.Remove(0, removeLength);
                goto AddMessage;
            }

            //消息过长无法显示;
            MessageTooLong:
            message = "[过长的消息,无法显示]";

            AddMessage:
            stringBuilder.Append(message);
            eventLengthList.AddLast(message.Length);


            //lock (asyncLock)
            //{
            //    if (message.Length > stringBuilder.MaxCapacity)
            //    {
            //        message = "[过长的消息,无法显示]";
            //    }
            //    if (message.Length + stringBuilder.Length > stringBuilder.MaxCapacity)
            //    {
            //        int removeLength = 0;
            //        do
            //        {
            //            var node = eventLengthList.First;
            //            removeLength += node.Value.Length;
            //            eventLengthList.Remove(node);
            //        }
            //        while (message.Length + stringBuilder.Length > stringBuilder.MaxCapacity);

            //        stringBuilder.Remove(0, removeLength);
            //    }
            //    stringBuilder.Append(message);
            //    eventLengthList.AddLast(message);
            //}
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
                eventLengthList.Clear();
            }
        }
    }
}
