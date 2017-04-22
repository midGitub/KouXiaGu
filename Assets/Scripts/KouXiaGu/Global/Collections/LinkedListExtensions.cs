using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 对双向链表的拓展;
    /// </summary>
    public static class LinkedListExtensionscs
    {

        /// <summary>
        /// 迭代合集,迭代过程中不允许移除合集内元素;
        /// </summary>
        public static IEnumerable<LinkedListNode<T>> EnumerateNode<T>(this LinkedList<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            var current = collection.First;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }

        /// <summary>
        /// 若为寻找到则返回Null;
        /// </summary>
        public static LinkedListNode<T> FirstOrDefaultNode<T>(this LinkedList<T> collection, Func<T, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            foreach (var node in collection.EnumerateNode())
            {
                T item = node.Value;
                if (predicate(item))
                {
                    return node;
                }
            }
            return null;
        }

        /// <summary>
        /// 移除第一个满足要求的元素;
        /// </summary>
        public static bool Remove<T>(this LinkedList<T> collection, Func<T, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            var node = FirstOrDefaultNode(collection, predicate);

            if (node == null)
                return false;

            collection.Remove(node);
            return true;
        }




        /// <summary>
        /// 移除队首元素并且返回;
        /// </summary>
        [Obsolete]
        public static T Dequeue<T>(this LinkedList<T> collection)
        {
            LinkedListNode<T> first = collection.First;

            if (first == null)
                throw new ArgumentException();

            collection.Remove(first);
            T value = first.Value;
            return value;
        }

    }

}
