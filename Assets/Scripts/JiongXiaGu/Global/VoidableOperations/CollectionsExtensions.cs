using System;
using System.Collections.Generic;

namespace JiongXiaGu.VoidableOperations
{

    /// <summary>
    /// 合集结构可撤销拓展;
    /// </summary>
    public static class CollectionsExtensions
    {
        #region Collection

        /// <summary>
        /// 操作在创建时是未执行的,需要手动调用 PerformDo() 执行;
        /// </summary>
        public static VoidableOperation VoidableClear<T>(this ICollection<T> list)
        {
            var item = new CollectionClear<T>(list);
            return item;
        }

        #endregion

        #region  IDictionary

        /// <summary>
        /// 操作在创建时是未执行的,需要手动调用 PerformDo() 执行;
        /// </summary>
        public static VoidableOperation VoidableSetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue newValue)
        {
            var operation = new DictionarySetValue<TKey, TValue>(dictionary, key, newValue);
            return operation;
        }

        /// <summary>
        /// 操作在创建时是未执行的,需要手动调用 PerformDo() 执行;
        /// </summary>
        public static VoidableOperation VoidableAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            var operation = new DictionaryAdd<TKey, TValue>(dictionary, key, value);
            return operation;
        }

        /// <summary>
        /// 操作在创建时是未执行的,需要手动调用 PerformDo() 执行;
        /// </summary>
        public static VoidableOperation VoidableRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            var operation = new DictionaryRemove<TKey, TValue>(dictionary, key);
            return operation;
        }

        #endregion

        #region List

        /// <summary>
        /// 操作在创建时是未执行的,需要手动调用 PerformDo() 执行;
        /// </summary>
        public static VoidableOperation VoidableSetValue<T>(this IList<T> list, int index, T item)
        {
            var operation = new ListSetValue<T>(list, index, item);
            return operation;
        }

        /// <summary>
        /// 操作在创建时是未执行的,需要手动调用 PerformDo() 执行;
        /// </summary>
        public static VoidableOperation VoidableAdd<T>(this IList<T> list, T item)
        {
            var operation = new ListAdd<T>(list, item);
            return operation;
        }

        /// <summary>
        /// 操作在创建时是未执行的,需要手动调用 PerformDo() 执行;
        /// 移除指定元素,若未找到则返回异常 KeyNotFoundException;
        /// </summary>
        public static VoidableOperation VoidableRemove<T>(this IList<T> list, T item)
        {
            var operation = new ListRemove<T>(list, item);
            return operation;
        }

        /// <summary>
        /// 操作在创建时是未执行的,需要手动调用 PerformDo() 执行;
        /// </summary>
        public static VoidableOperation VoidableRemoveAt<T>(this IList<T> list, int index)
        {
            var operation = new ListRemoveAt<T>(list, index);
            return operation;
        }

        /// <summary>
        /// 操作在创建时是未执行的,需要手动调用 PerformDo() 执行;
        /// </summary>
        public static VoidableOperation VoidableInsert<T>(this IList<T> list, int index, T item)
        {
            var operation = new ListInsert<T>(list, index, item);
            return operation;
        }

#endregion
    }

    #region Collection

    sealed class CollectionClear<T> : VoidableOperation
    {
        public CollectionClear(ICollection<T> collection)
        {
            this.collection = collection;
        }

        readonly ICollection<T> collection;
        T[] original;

        protected override void PerformDo_protected()
        {
            original = new T[collection.Count];
            collection.CopyTo(original, 0);
            collection.Clear();
        }

        protected override void PerformUndo_protected()
        {
            foreach (var item in original)
            {
                collection.Add(item);
            }
        }
    }

    #endregion

    #region List

    sealed class ListSetValue<T> : VoidableOperation
    {
        public ListSetValue(IList<T> list, int index, T item)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            this.list = list;
            this.index = index;
            this.item = item;
        }

        readonly IList<T> list;
        readonly int index;
        readonly T item;
        T original;

        protected override void PerformDo_protected()
        {
            original = list[index];
            list[index] = item;
        }

        protected override void PerformUndo_protected()
        {
            list[index] = original;
        }
    }

    sealed class ListAdd<T> : VoidableOperation
    {
        public ListAdd(IList<T> list, T item)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            this.list = list;
            this.item = item;
        }

        readonly IList<T> list;
        readonly T item;

        protected override void PerformDo_protected()
        {
            list.Add(item);
        }

        protected override void PerformUndo_protected()
        {
            list.RemoveAt(list.Count - 1);
        }
    }

    sealed class ListRemove<T> : VoidableOperation
    {
        public ListRemove(IList<T> list, T item)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            this.list = list;
            this.item = item;
        }

        readonly IList<T> list;
        readonly T item;
        int index;

        protected override void PerformDo_protected()
        {
            index = list.FindIndex(item);
            if (index >= 0)
            {
                list.RemoveAt(index);
            }
            else
            {
                throw new KeyNotFoundException("找不到对应值:" + item);
            }
        }

        protected override void PerformUndo_protected()
        {
            list.Insert(index, item);
        }
    }

    sealed class ListRemoveAt<T> : VoidableOperation
    {
        public ListRemoveAt(IList<T> list, int index)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            this.list = list;
            this.index = index;
        }

        readonly IList<T> list;
        readonly int index;
        T original;

        protected override void PerformDo_protected()
        {
            original = list[index];
            list.RemoveAt(index);
        }

        protected override void PerformUndo_protected()
        {
            list.Insert(index, original);
        }
    }

    sealed class ListInsert<T> : VoidableOperation
    {
        public ListInsert(IList<T> list, int index, T item)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            this.list = list;
            this.index = index;
            this.item = item;
        }

        readonly IList<T> list;
        readonly int index;
        readonly T item;

        protected override void PerformDo_protected()
        {
            list.Insert(index, item);
        }

        protected override void PerformUndo_protected()
        {
            list.RemoveAt(index);
        }
    }

    #endregion

    #region Dictionary

    sealed class DictionarySetValue<TKey, TValue> : VoidableOperation
    {
        public DictionarySetValue(IDictionary<TKey, TValue> dictionary, TKey key, TValue newValue)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            this.dictionary = dictionary;
            this.key = key;
            this.newValue = newValue;
        }

        readonly IDictionary<TKey, TValue> dictionary;
        readonly TKey key;
        readonly TValue newValue;
        TValue original;

        protected override void PerformDo_protected()
        {
            original = dictionary[key];
            dictionary[key] = newValue;
        }

        protected override void PerformUndo_protected()
        {
            dictionary[key] = original;
        }
    }

    sealed class DictionaryAdd<TKey, TValue> : VoidableOperation
    {
        public DictionaryAdd(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            this.dictionary = dictionary;
            this.key = key;
            this.value = value;
        }

        readonly IDictionary<TKey, TValue> dictionary;
        readonly TKey key;
        readonly TValue value;

        protected override void PerformDo_protected()
        {
            dictionary.Add(key, value);
        }

        protected override void PerformUndo_protected()
        {
            dictionary.Remove(key);
        }
    }

    sealed class DictionaryRemove<TKey, TValue> : VoidableOperation
    {
        public DictionaryRemove(IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            this.dictionary = dictionary;
            this.key = key;
        }

        readonly IDictionary<TKey, TValue> dictionary;
        readonly TKey key;
        TValue original;

        protected override void PerformDo_protected()
        {
            original = dictionary[key];
            dictionary.Remove(key);
        }

        protected override void PerformUndo_protected()
        {
            dictionary.Add(key, original);
        }
    }

#endregion
}
