using System;
using System.Collections.Generic;
using System.Text;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 将控制台消息写入StringBuilder(线程安全);
    /// </summary>
    public class ConsoleStringBuilder : IObserver<ConsoleEvent>
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

                var lastNode = eventLengthList.Last;
                if (lastNode != null && !lastNode.Value.EndsWith(Environment.NewLine))
                {
                    lastNode.Value += Environment.NewLine;
                    stringBuilder.Append(Environment.NewLine);
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

                ////获取到加入合集的实际消息;
                //string actualMessage = message;
                //LinkedListNode<string> lastNode = eventLengthList.Last;
                //if (lastNode != null && !lastNode.Value.EndsWith(Environment.NewLine))
                //{
                //    actualMessage = Environment.NewLine + message;
                //}

                ////若容量满足则直接加入,否则对之前加入的消息进行删除;
                //if (stringBuilder.Length + actualMessage.Length <= stringBuilder.MaxCapacity)
                //{
                //    stringBuilder.Append(actualMessage);
                //    eventLengthList.AddLast(actualMessage);
                //}
                //else
                //{
                //    if (actualMessage.Length + lastNode.Value.Length > stringBuilder.MaxCapacity)
                //    {
                //        stringBuilder.Clear();
                //        stringBuilder.Append(message);

                //        eventLengthList.Clear();
                //        eventLengthList.AddLast(message);
                //    }
                //    else
                //    {
                //        int removeLength = 0;
                //        while (stringBuilder.Length - removeLength + actualMessage.Length > stringBuilder.MaxCapacity)
                //        {
                //            var node = eventLengthList.First;
                //            removeLength += node.Value.Length;
                //            eventLengthList.Remove(node);
                //        }

                //        stringBuilder.Remove(0, removeLength);
                //        stringBuilder.Append(message);
                //        eventLengthList.AddLast(message);
                //    }
                //}

                //int messageLength = message.Length;
                //while (messageLength < stringBuilder.MaxCapacity)
                //{

                //}



                //var lastNode = eventLengthList.Last;
                //if (lastNode != null && !lastNode.Value.EndsWith(Environment.NewLine))
                //{
                //    stringBuilder.Append(Environment.NewLine);
                //    eventLengthList.AddLast(Environment.NewLine);
                //}

                //if (message.Length > stringBuilder.MaxCapacity)
                //{
                //    message = "[过长的消息,无法显示]";
                //    if (message.Length > stringBuilder.MaxCapacity)
                //    {
                //        stringBuilder.Clear();
                //        stringBuilder.Append("Error");
                //    }
                //}

                //int removeLength = 0;
                //while (message.Length + stringBuilder.Length - removeLength > stringBuilder.MaxCapacity)
                //{
                //    var node = eventLengthList.First;
                //    removeLength += node.Value.Length;
                //    eventLengthList.Remove(node);
                //}

                //stringBuilder.Remove(0, removeLength);
                //stringBuilder.Append(message);
                //eventLengthList.AddLast(message);
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
                eventLengthList.Clear();
            }
        }
    }
}
