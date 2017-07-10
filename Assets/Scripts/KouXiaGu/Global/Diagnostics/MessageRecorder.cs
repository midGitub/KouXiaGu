using System;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.Diagnostics
{


    public class Recorder<T>
    {
        public Recorder()
        {
            list = new LinkedList<T>();
            current = null;
        }

        readonly LinkedList<T> list;
        LinkedListNode<T> current;

        public int Count
        {
            get { return list.Count; }
        }

        /// <summary>
        /// 添加内容到末尾;
        /// </summary>
        public void Add(T item)
        {
            if (current != null)
            {
                list.RemoveAfterNodes(current);
            }
            list.AddLast(item);
            current = null;
        }

        /// <summary>
        /// 获取到当前指向的内容;
        /// </summary>
        public T GetCurrent()
        {
            if (current != null)
            {
                return current.Value;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 获取到当前指向的下一个内容;
        /// </summary>
        public T GetNext()
        {
            if (current == null)
            {
                return default(T);
            }
            else if (current.Next != null)
            {
                current = current.Next;
                return current.Value;
            }
            else
            {
                current = null;
                return default(T);
            }
        }

        /// <summary>
        /// 获取到当前指向的上一个内容;
        /// </summary>
        public T GetPrevious()
        {
            if (current == null)
            {
                current = list.Last;
                if (current == null)
                {
                    return default(T);
                }
                else
                {
                    return current.Value;
                }
            }
            if (current.Previous != null)
            {
                current = current.Previous;
                return current.Value;
            }
            else
            {
                return current.Value;
            }
        }
    }
}
