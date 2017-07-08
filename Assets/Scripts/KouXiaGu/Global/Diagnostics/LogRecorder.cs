using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Diagnostics
{

    /// <summary>
    /// 消息记录;
    /// </summary>
    public class LogRecorder : ILogger
    {
        public LogRecorder() : this(defaultMaxLength)
        {
        }

        public LogRecorder(TextStyleConverter textStyle) : this(defaultMaxLength, textStyle)
        {
        }

        public LogRecorder(int maxLength) : this(maxLength, new TextStyleConverter())
        {
        }

        public LogRecorder(int maxLength, TextStyleConverter textStyle)
        {
            if (textStyle == null)
                throw new ArgumentNullException("textStyle");

            MaxLength = maxLength;
            TextStyle = textStyle;
            StringBuilder = new StringBuilder(maxLength);
            LenghtMask = new LinkedList<int>();
        }

        static readonly int defaultMaxLength = 10000;
        public int MaxLength { get; private set; }
        public TextStyleConverter TextStyle { get; private set; }
        public StringBuilder StringBuilder { get; private set; }
        public LinkedList<int> LenghtMask { get; private set; }
        Action<LogRecorder> onTextChanged;

        /// <summary>
        /// 当文本发生变化时调用;
        /// </summary>
        public event Action<LogRecorder> OnTextChanged
        {
            add { onTextChanged += value; }
            remove { onTextChanged -= value; }
        }

        public string GetText()
        {
            return StringBuilder.ToString();
        }

        public void Log(string message)
        {
            message = TextStyle.GetNormal(message);
            AppendLine(message);
        }

        public void LogError(string message)
        {
            message = TextStyle.GetError(message);
            AppendLine(message);
        }

        public void LogWarning(string message)
        {
            message = TextStyle.GetWarning(message);
            AppendLine(message);
        }

        void AppendLine(string message)
        {
            message += Environment.NewLine;
            Append(message);
        }

        public void Append(string message)
        {
            while (StringBuilder.Length + message.Length > MaxLength)
            {
                RemoveTopMessage();
            }

            StringBuilder.Append(message);
            LenghtMask.AddLast(message.Length);

            if (onTextChanged != null)
            {
                onTextChanged(this);
            }
        }

        void RemoveTopMessage()
        {
            var node = LenghtMask.First;
            StringBuilder.Remove(0, node.Value);
            LenghtMask.Remove(node);
        }
    }
}
