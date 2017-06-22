using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace KouXiaGu.Collections
{

    /// <summary>
    /// 自定义实现的双向链接(非循环);
    /// </summary>
    public class cLinkedList<T>
    {

        public int Count { get; private set; }
        public LinkedListNode<T> First { get; private set; }
        public LinkedListNode<T> Last { get; private set; }

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            throw new NotImplementedException();
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            throw new NotImplementedException();
        }

        void InsertNodeBefore_internal(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            newNode.Next = node;
            newNode.Previous = node.Previous;
            node.Previous.Next = newNode;
            node.Previous = newNode;
            Count++;
        }

        void InsertNodeToEmptyList_internal(LinkedListNode<T> newNode)
        {
            First = newNode;
            Last = newNode;
            Count++;
        }

        void RemoveNode_internal(LinkedListNode<T> node)
        {
            if (node.Next == null)
            {
                Last = node.Previous;
            }
            else
            {
                node.Next.Previous = node.Previous;
            }

            if (node.Previous == null)
            {
                First = node.Next;
            }
            else
            {
                node.Previous.Next = node.Next;
            }

            node.Invalidate();
            Count--;
        }

        public void Remove(LinkedListNode<T> node)
        {
            ValidateNode(node);
            RemoveNode_internal(node);
        }

        void ValidateNode(LinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            if (node.List != this)
            {
                throw new InvalidOperationException();
            }
        }
    }

    public sealed class LinkedListNode<T>
    {
        public LinkedListNode(cLinkedList<T> list, T value)
        {
            List = list;
            Value = value;
        }

        public cLinkedList<T> List { get; internal set; }
        public LinkedListNode<T> Next { get; internal set; }
        public LinkedListNode<T> Previous { get; internal set; }
        public T Value { get; internal set; }

        internal void Invalidate()
        {
            List = null;
            Next = null;
            Previous = null;
        }
    }
}
