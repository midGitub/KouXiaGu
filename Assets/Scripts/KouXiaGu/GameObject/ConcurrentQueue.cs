using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 表示线程安全的先进先出 (FIFO) 集合。
    /// </summary>
    public class ConcurrentQueue<T> : IEnumerable<T>
    {
        public ConcurrentQueue()
        {
            queue = new Queue<T>();
        }
        public ConcurrentQueue(IEnumerable<T> collection)
        {
            queue = new Queue<T>(collection);
        }

        private readonly Queue<T> queue;

        public int Count
        {
            get { return queue.Count; }
        }

        public bool IsEmpty
        {
            get
            {
                lock (queue)
                {
                    return queue.Count == 0;
                }
            }
        }

        /// <summary>
        /// 将对象添加到 ConcurrentQueue<T> 的结尾处;
        /// </summary>
        public void Enqueue(T item)
        {
            lock (queue)
            {
                queue.Enqueue(item);
            }
        }

        ///// <summary>
        ///// 将对象按返回顺序添加到 ConcurrentQueue<T> 的结尾处;
        ///// </summary>
        //public void Enqueue(IEnumerable<T> items)
        //{
        //    lock (queue)
        //    {
        //        foreach (var item in items)
        //        {
        //            queue.Enqueue(item);
        //        }
        //    }
        //}

        /// <summary>
        /// 尝试移除并返回并发队列开头处的对象
        /// </summary>
        public bool TryDequeue(out T result)
        {
            lock (queue)
            {
                if (queue.Count != 0)
                {
                    result = queue.Dequeue();
                    return true;
                }
                else
                {
                    result = default(T);
                    return false;
                }
            }
        }

        /// <summary>
        /// 尝试返回 ConcurrentQueue<T> 开头处的对象但不将其移除
        /// </summary>
        public bool TryPeek(out T result)
        {
            lock (queue)
            {
                if (queue.Count != 0)
                {
                    result = queue.Peek();
                    return true;
                }
                else
                {
                    result = default(T);
                    return false;
                }
            }
        }

        public void Clear()
        {
            lock (queue)
            {
                queue.Clear();
            }
        }

        public T[] ToArray()
        {
            lock (queue)
            {
                return queue.ToArray();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            T[] queueArray = ToArray();
            return queueArray.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}
