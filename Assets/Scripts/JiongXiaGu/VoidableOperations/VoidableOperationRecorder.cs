using System;
using System.Collections.Generic;
using System.Linq;

namespace JiongXiaGu.VoidableOperations
{

    /// <summary>
    /// 提供的非泛型版本;
    /// </summary>
    public class VoidableOperationRecorder : VoidableOperationRecorder<VoidableOperation>
    {
        public VoidableOperationRecorder()
        {
        }

        public VoidableOperationRecorder(int maxRecord) : base(maxRecord)
        {
        }
    }

    /// <summary>
    /// 记录操作;
    /// </summary>
    public class VoidableOperationRecorder<T>
        where T : VoidableOperation
    {
        internal const int DefaultMaxRecord = 20;

        public VoidableOperationRecorder() : this(DefaultMaxRecord)
        {
        }

        public VoidableOperationRecorder(int maxRecord)
        {
            if (maxRecord <= 0)
                throw new ArgumentOutOfRangeException("maxRecord :" + maxRecord);

            MaxRecord = maxRecord;
            operationQueue = new Collections.DoublyLinkedList<T>();
        }

        /// <summary>
        /// 操作记录链;
        /// </summary>
        readonly Collections.DoublyLinkedList<T> operationQueue;

        /// <summary>
        /// 最后执行的操作;
        /// </summary>
        Collections.LinkedListNode<T> current;

        /// <summary>
        /// 最大记录数目;
        /// </summary>
        public int MaxRecord { get; set; }

        /// <summary>
        /// 指令总数,可能会大于最大记录数目;
        /// </summary>
        public int Count
        {
            get { return operationQueue.Count; }
        }

        /// <summary>
        /// 最后执行的命令;
        /// </summary>
        public T Last
        {
            get { return current == null ? default(T) : current.Value as T; }
        }

        /// <summary>
        /// 所有指令;
        /// </summary>
        public IEnumerable<T> Operations
        {
            get { return operationQueue.OfType<T>(); }
        }

        /// <summary>
        /// 记录这个操作为最后的操作;
        /// </summary>
        public void Register(T operation)
        {
            if (operation == null)
                throw new ArgumentNullException("operation");

            if (current == operationQueue.Last)
            {
                operationQueue.AddLast(operation);
                if (Count > MaxRecord)
                {
                    int rem = Count - MaxRecord;
                    var node = operationQueue.First;
                    for (int i = 0; i < rem; i++)
                    {
                        node = node.Next;
                    }
                    operationQueue.RemoveBeforNodes(node);
                }
            }
            else if (current == null)
            {
                operationQueue.Clear();
                operationQueue.AddLast(operation);
            }
            else
            {
                operationQueue.RemoveAfterNodes(current);
                operationQueue.AddLast(operation);
            }
            current = operationQueue.Last;
        }

        /// <summary>
        /// 执行撤销,若撤销出现异常,则弹出异常,并且不进行指令移动;
        /// </summary>
        public void PerformUndo()
        {
            if (current != null)
            {
                var operation = current.Value;
                operation.PerformUndo();
                current = current.Previous;
            }
        }

        /// <summary>
        /// 执行重做,若出现异常则弹出异常,并且不进行指令移动;
        /// </summary>
        public void PerformRedo()
        {
            if (current == null)
            {
                if (operationQueue.First != null)
                {
                    var operation = operationQueue.First.Value;
                    operation.PerformDo();
                    current = operationQueue.First;
                }
            }
            else
            {
                if (current.Next != null)
                {
                    var operation = current.Next.Value;
                    operation.PerformDo();
                    current = current.Next;
                }
            }
        }

        /// <summary>
        /// 清除所有记录;
        /// </summary>
        public void Clear()
        {
            operationQueue.Clear();
            current = null;
        }
    }
}
