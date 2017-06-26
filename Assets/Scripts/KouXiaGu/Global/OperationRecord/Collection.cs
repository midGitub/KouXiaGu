using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.OperationRecord
{


    public static class CollectionExtensions
    {
        public static IVoidable VoidableClear<T>(this ICollection<T> list)
        {
            var item = new CollectionClear<T>(list);
            return item;
        }
    }

    sealed class CollectionClear<T> : SafeVoidable
    {
        public CollectionClear(ICollection<T> collection)
        {
            this.collection = collection;
            original = new T[collection.Count];
            collection.CopyTo(original, 0);
            collection.Clear();
        }

        readonly ICollection<T> collection;
        readonly T[] original;

        public override void Redo()
        {
            collection.Clear();
        }

        public override void Undo()
        {
            foreach (var item in original)
            {
                collection.Add(item);
            }
        }
    }
}
