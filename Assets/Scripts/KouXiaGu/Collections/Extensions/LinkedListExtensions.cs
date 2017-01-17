﻿using System;
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
        /// 移除队首元素并且返回;
        /// </summary>
        public static T Dequeue<T>(this LinkedList<T> collection)
        {
            LinkedListNode<T> first = collection.First;
            collection.Remove(first);
            T value = first.Value;
            return value;
        }

        /// <summary>
        /// 移除第一个满足要求的元素;
        /// </summary>
        public static bool Remove<T>(this LinkedList<T> collection, Func<T, bool> func)
        {
            var current = collection.First;

            while (current != null)
            {
                if (func(current.Value))
                {
                    collection.Remove(current);
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public static LinkedListNode<T> RemoveAndReturnNext<T>(this LinkedList<T> collection, LinkedListNode<T> node)
        {
            LinkedListNode<T> next = node.Next;
            collection.Remove(node);
            return next;
        }

    }

}