using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.OperationRecord
{

    public class ListAdd<T> : IVoidableOperation
    {
        public ListAdd(IList<T> list, T item)
        {
            this.list = list;
            this.item = item;
            list.Add(item);
        }

        readonly IList<T> list;
        readonly T item;

        public void Redo()
        {
            list.Add(item);
        }

        public void Undo()
        {
            list.RemoveAt(list.Count - 1);
        }
    }

    public class ListRemove<T> : IVoidableOperation
    {
        public ListRemove(IList<T> list, T item)
        {
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

        public void Redo()
        {
            list.RemoveAt(index);
        }

        public void Undo()
        {
            list.Insert(index, item);
        }
    }

    public class ListRemoveAt<T> : IVoidableOperation
    {
        public ListRemoveAt(IList<T> list, int index)
        {
            this.list = list;
            this.index = index;
            original = list[index];
            list.RemoveAt(index);
        }

        readonly IList<T> list;
        readonly int index;
        readonly T original;

        public void Redo()
        {
            list.RemoveAt(index);
        }

        public void Undo()
        {
            list.Insert(index, original);
        }
    }
}
