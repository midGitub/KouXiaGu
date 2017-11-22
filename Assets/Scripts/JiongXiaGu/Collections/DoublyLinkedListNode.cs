namespace JiongXiaGu.Collections
{
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
