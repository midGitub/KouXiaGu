using System;
using System.Collections.Generic;
using System.Collections;

namespace JiongXiaGu.Collections
{

    /// <summary>
    /// 非循环的双向链接;
    /// </summary>
    public class DoublyLinkedList<T> : ICollection<T>, IReadOnlyCollection<T>, IEnumerable<T>
    {
        private int version;
        public int Count { get; private set; }
        public LinkedListNode<T> First { get; private set; }
        public LinkedListNode<T> Last { get; private set; }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        public DoublyLinkedList()
        {
        }

        public DoublyLinkedList(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (T item in collection)
            {
                AddLast(item);
            }
        }

        /// <summary>
        /// 同 AddLast(T) 方法;
        /// </summary>
        public void Add(T value)
        {
            AddLast(value);
        }

        /// <summary>
        /// 在指定的现有节点后添加包含指定值的新节点;
        /// </summary>
        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            ValidateNode(node);
            LinkedListNode<T> result = new LinkedListNode<T>(this, value);
            InternalInsertNodeAfter(node, result);
            return result;
        }

        /// <summary>
        /// 在指定的现有节点后添加包含指定值的新节点;
        /// </summary>
        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            newNode.List = this;
            InternalInsertNodeAfter(node, newNode);
        }

        /// <summary>
        /// 在指定的现有节点前添加指定的新节点;
        /// </summary>
        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            ValidateNode(node);
            LinkedListNode<T> result = new LinkedListNode<T>(this, value);
            InternalInsertNodeBefore(node, result);
            return result;
        }

        /// <summary>
        /// 在指定的现有节点前添加指定的新节点;
        /// </summary>
        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            newNode.List = this;
            InternalInsertNodeBefore(node, newNode);
        }

        /// <summary>
        /// 在开头处添加包含指定值的新节点;
        /// </summary>
        public LinkedListNode<T> AddFirst(T value)
        {
            LinkedListNode<T> result = new LinkedListNode<T>(this, value);
            if (First == null)
            {
                InternalInsertNodeToEmptyList(result);
            }
            else
            {
                InternalInsertNodeBefore(First, result);
            }
            return result;
        }

        /// <summary>
        /// 在开头处添加包含指定值的新节点;
        /// </summary>
        public void AddFirst(LinkedListNode<T> node)
        {
            ValidateNewNode(node);
            node.List = this;
            if (First == null)
            {
                InternalInsertNodeToEmptyList(node);
            }
            else
            {
                InternalInsertNodeBefore(First, node);
            }
        }

        /// <summary>
        /// 在结尾处添加包含指定值的新节点。
        /// </summary>
        public LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> result = new LinkedListNode<T>(this, value);
            if (Last == null)
            {
                InternalInsertNodeToEmptyList(result);
            }
            else
            {
                InternalInsertNodeAfter(Last, result);
            }
            return result;
        }

        /// <summary>
        /// 在结尾处添加包含指定值的新节点。
        /// </summary>
        public void AddLast(LinkedListNode<T> node)
        {
            ValidateNewNode(node);
            node.List = this;
            if (Last == null)
            {
                InternalInsertNodeToEmptyList(node);
            }
            else
            {
                InternalInsertNodeAfter(Last, node);
            }
        }

        /// <summary>
        /// 移除指定值的第一个匹配项;
        /// </summary>
        public bool Remove(T value)
        {
            LinkedListNode<T> node = Find(value);
            if (node != null)
            {
                InternalRemoveNode(node);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除指定的节点;
        /// </summary>
        public void Remove(LinkedListNode<T> node)
        {
            ValidateNode(node);
            InternalRemoveNode(node);
        }

        /// <summary>
        /// 移除位于开头处的节点;
        /// </summary>
        public void RemoveFirst()
        {
            if (First == null)
                throw new InvalidOperationException();
            InternalRemoveNode(First);
        }

        /// <summary>
        /// 移除位于结尾处的节点;
        /// </summary>
        public void RemoveLast()
        {
            if (Last == null)
                throw new InvalidOperationException();
            InternalRemoveNode(Last);
        }

        /// <summary>
        /// 移除这个节点之后的所有节点;
        /// </summary>
        public void RemoveAfterNodes(LinkedListNode<T> node)
        {
            ValidateNode(node);
            LinkedListNode<T> current = node.Next;
            while (current != null)
            {
                LinkedListNode<T> temp = current;
                current = current.Next;
                temp.Invalidate();
                Count--;
            }
            node.Next = null;
            Last = node;
        }

        /// <summary>
        /// 移除这个节点之前的所有节点;
        /// </summary>
        public void RemoveBeforNodes(LinkedListNode<T> node)
        {
            ValidateNode(node);
            LinkedListNode<T> current = node.Previous;
            while (current != null)
            {
                LinkedListNode<T> temp = current;
                current = current.Previous;
                temp.Invalidate();
                Count--;
            }
            node.Previous = null;
            First = node;
        }

        /// <summary>
        /// 确定某值是否在合集中;
        /// </summary>
        public bool Contains(T value)
        {
            return Find(value) != null;
        }

        /// <summary>
        /// 查找包含指定值的第一个节点;
        /// </summary>
        public LinkedListNode<T> Find(T value)
        {
            LinkedListNode<T> current = First;
            if (current != null)
            {
                if (value != null)
                {
                    EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                    do
                    {
                        if (comparer.Equals(current.Value, value))
                        {
                            return current;
                        }
                        current = current.Next;
                    } while (current != null);
                }
                else
                {
                    do
                    {
                        if (current.Value == null)
                        {
                            return current;
                        }
                        current = current.Next;
                    } while (current != null);
                }
            }
            return null;
        }

        /// <summary>
        /// 查找包含指定值的最后一个节点;
        /// </summary>
        public LinkedListNode<T> FindLast(T value)
        {
            LinkedListNode<T> current = Last;
            if (current != null)
            {
                if (value != null)
                {
                    EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                    do
                    {
                        if (comparer.Equals(current.Value, value))
                        {
                            return current;
                        }
                        current = current.Previous;
                    } while (current != null);
                }
                else
                {
                    do
                    {
                        if (current.Value == null)
                        {
                            return current;
                        }
                        current = current.Previous;
                    } while (current != null);
                }
            }
            return null;
        }

        /// <summary>
        /// 从目标数组的指定索引处开始将整个 LinkedList<T> 复制到兼容的一维 Array;
        /// </summary>
        public void CopyTo(T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (array.Length - index < Count)
            {
                throw new ArgumentException();
            }

            LinkedListNode<T> node = First;
            while (node != null)
            {
                array[index++] = node.Value;
                node = node.Next;
            }
        }

        /// <summary>
        /// 移除所有节点;
        /// </summary>
        public void Clear()
        {
            LinkedListNode<T> current = First;
            while (current != null)
            {
                LinkedListNode<T> temp = current;
                current = current.Next;
                temp.Invalidate();
            }
            First = null;
            Last = null;
            Count = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 在次节点之后插入节点;
        /// </summary>
        private void InternalInsertNodeAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            if (node.Next == null)
            {
                Last = newNode;
            }
            else
            {
                node.Next.Previous = newNode;
            }
            newNode.Next = node.Next;
            newNode.Previous = node;
            node.Next = newNode;
            Count++;
            version++;
        }

        private void InternalInsertNodeBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            if (node.Previous == null)
            {
                First = newNode;
            }
            else
            {
                node.Previous.Next = newNode;
            }
            newNode.Next = node;
            newNode.Previous = node.Previous;
            node.Previous = newNode;
            Count++;
            version++;
        }

        private void InternalInsertNodeToEmptyList(LinkedListNode<T> newNode)
        {
            First = newNode;
            Last = newNode;
            Count++;
            version++;
        }

        private void InternalRemoveNode(LinkedListNode<T> node)
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
            version++;
        }

        private void ValidateNode(LinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (node.List != this)
            {
                throw new InvalidOperationException();
            }
        }

        private void ValidateNewNode(LinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (node.List != null)
            {
                throw new InvalidOperationException();
            }
        }

        public struct Enumerator : IEnumerator<T>
        {
            private DoublyLinkedList<T> list;
            private LinkedListNode<T> node;
            private int version;
            private T current;

            public T Current
            {
                get { return current; }
            }

            object IEnumerator.Current
            {
                get { return current; }
            }

            internal Enumerator(DoublyLinkedList<T> list)
            {
                this.list = list;
                version = list.version;
                node = list.First;
                current = default(T);
            }

            /// <summary>
            /// 当迭代实例与合集版本不同时抛出异常;
            /// </summary>
            private void ThrowIfVersionChanged()
            {
                if (version != list.version)
                {
                    throw new InvalidOperationException("合集已经发生变化;");
                }
            }

            public bool MoveNext()
            {
                ThrowIfVersionChanged();

                if (node == null)
                {
                    return false;
                }

                current = node.Value;
                node = node.Next;
                return true;
            }

            public void Reset()
            {
                ThrowIfVersionChanged();

                node = list.First;
                current = default(T);
            }

            public void Dispose()
            {
                return;
            }
        }
    }

    public sealed class LinkedListNode<T>
    {
        public DoublyLinkedList<T> List { get; internal set; }
        public LinkedListNode<T> Next { get; internal set; }
        public LinkedListNode<T> Previous { get; internal set; }
        public T Value { get; internal set; }

        internal LinkedListNode(DoublyLinkedList<T> list, T value)
        {
            List = list;
            Value = value;
        }

        /// <summary>
        /// 重置链表数据;
        /// </summary>
        internal void Invalidate()
        {
            List = null;
            Next = null;
            Previous = null;
        }
    }
}
