using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.OperationRecord
{

    /// <summary>
    /// List的可撤销方法拓展;
    /// </summary>
    public static class ListExtensions
    {
        public static IVoidable VoidableSetValue<T>(this IList<T> list, int index, T item)
        {
            var operation = new ListSetValue<T>(list, index, item);
            return operation;
        }

        public static IVoidable VoidableAdd<T>(this IList<T> list, T item)
        {
            ListAdd<T> add = new ListAdd<T>(list, item);
            return add;
        }

        /// <summary>
        /// 移除指定元素,若未找到则返回异常 KeyNotFoundException;
        /// </summary>
        public static IVoidable VoidableRemove<T>(this IList<T> list, T item)
        {
            ListRemove<T> remove = new ListRemove<T>(list, item);
            return remove;
        }

        public static IVoidable VoidableRemoveAt<T>(this IList<T> list, int index)
        {
            ListRemoveAt<T> removeAt = new ListRemoveAt<T>(list, index);
            return removeAt;
        }

        public static IVoidable VoidableInsert<T>(this IList<T> list, int index, T item)
        {
            ListInsert<T> insert = new ListInsert<T>(list, index, item);
            return insert;
        }
    }

    sealed class ListSetValue<T> : SafeVoidable
    {
        public ListSetValue(IList<T> list, int index, T item)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            this.list = list;
            this.index = index;
            this.item = item;
            original = list[index];
            list[index] = item;
        }

        readonly IList<T> list;
        readonly int index;
        readonly T item;
        readonly T original;

        public override void PerformRedo()
        {
            list[index] = item;
        }

        public override void PerformUndo()
        {
            list[index] = original;
        }
    }

    sealed class ListAdd<T> : SafeVoidable
    {
        public ListAdd(IList<T> list, T item)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            this.list = list;
            this.item = item;
            list.Add(item);
        }

        readonly IList<T> list;
        readonly T item;

        public override void PerformRedo()
        {
            list.Add(item);
        }

        public override void PerformUndo()
        {
            list.RemoveAt(list.Count - 1);
        }
    }

    sealed class ListRemove<T> : SafeVoidable
    {
        public ListRemove(IList<T> list, T item)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            this.list = list;
            index = list.FindIndex(item);
            if (index >= 0)
            {
                this.item = item;
                list.RemoveAt(index);
            }
            else
            {
                throw new KeyNotFoundException("找不到对应值:" + item);
            }
        }

        readonly IList<T> list;
        readonly int index;
        readonly T item;

        public override void PerformRedo()
        {
            list.RemoveAt(index);
        }

        public override void PerformUndo()
        {
            list.Insert(index, item);
        }
    }

    sealed class ListRemoveAt<T> : SafeVoidable
    {
        public ListRemoveAt(IList<T> list, int index)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            this.list = list;
            this.index = index;
            original = list[index];
            list.RemoveAt(index);
        }

        readonly IList<T> list;
        readonly int index;
        readonly T original;

        public override void PerformRedo()
        {
            list.RemoveAt(index);
        }

        public override void PerformUndo()
        {
            list.Insert(index, original);
        }
    }

    sealed class ListInsert<T> : SafeVoidable
    {
        public ListInsert(IList<T> list, int index, T item)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            this.list = list;
            this.index = index;
            this.item = item;
            list.Insert(index, item);
        }

        readonly IList<T> list;
        readonly int index;
        readonly T item;

        public override void PerformRedo()
        {
            list.Insert(index, item);
        }

        public override void PerformUndo()
        {
            list.RemoveAt(index);
        }
    }
}
