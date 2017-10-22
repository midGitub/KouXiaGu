using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.GameConsoles
{

    /// <summary>
    /// 控制台消息合集(线程安全的);
    /// </summary>
    public class ConsoleRecord : IReadOnlyCollection<ConsoleRecordItem>
    {
        public ConcurrentQueue<ConsoleRecordItem> records;
        private volatile int maxCount;

        /// <summary>
        /// 最大记录数目;
        /// </summary>
        public int MaxCount
        {
            get { return maxCount; }
            set { maxCount = value; }
        }

        /// <summary>
        /// 当前记录数目;
        /// </summary>
        public int Count
        {
            get { return records.Count; }
        }

        public ConsoleRecord(int maxCount)
        {
            if (maxCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxCount));

            this.maxCount = maxCount;
            records = new ConcurrentQueue<ConsoleRecordItem>();
        }

        /// <summary>
        /// 添加消息;
        /// </summary>
        public void Add(ConsoleRecordItem recordItem)
        {
            records.Enqueue(recordItem);
            while (records.Count > maxCount)
            {
                ConsoleRecordItem remove;
                records.TryDequeue(out remove);
            }
        }

        public IEnumerator<ConsoleRecordItem> GetEnumerator()
        {
            return records.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// 控制台消息条目;
    /// </summary>
    public struct ConsoleRecordItem
    {
        public ConsoleRecordItem(ConsoleRecordTypes type, string message)
        {
            Type = type;
            Message = message;
        }

        /// <summary>
        /// 消息类型;
        /// </summary>
        public ConsoleRecordTypes Type { get; private set; }

        /// <summary>
        /// 消息;
        /// </summary>
        public string Message { get; private set; }
    }

    /// <summary>
    /// 控制台条目类型;
    /// </summary>
    public enum ConsoleRecordTypes
    {
        /// <summary>
        /// 正常消息;
        /// </summary>
        Normal,
        
        /// <summary>
        /// 成功消息;
        /// </summary>
        Successful,

        /// <summary>
        /// 警告消息;
        /// </summary>
        Warning,

        /// <summary>
        /// 错误消息;
        /// </summary>
        Error,

        /// <summary>
        /// 方法字段;
        /// </summary>
        Method,
    }
}
