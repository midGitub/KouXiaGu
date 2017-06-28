using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.OperationRecord
{

    /// <summary>
    /// 可取消操作组;
    /// </summary>
    public sealed class VoidableGroup<T> : SafeVoidable, IVoidable, IEnumerable<T>
        where T : IVoidable
    {
        public VoidableGroup()
        {
            Operations = new List<T>();
        }

        public VoidableGroup(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Operations.Add(item);
            }
        }

        public VoidableGroup(int capacity)
        {
            Operations = new List<T>(capacity);
        }

        public List<T> Operations { get; private set; }

        /// <summary>
        /// 提供lambda表达式使用的方法;
        /// </summary>
        public void Add(T item)
        {
            Operations.Add(item);
        }

        public override void PerformRedo()
        {
            for (int i = 0; i < Operations.Count; i++)
            {
                var operation = Operations[i];
                operation.PerformRedo();
            }
        }

        public override void PerformUndo()
        {
            for (int i = Operations.Count - 1; i >= 0; i--)
            {
                var operation = Operations[i];
                operation.PerformUndo();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Operations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
