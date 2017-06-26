using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.OperationRecord
{

    /// <summary>
    /// 可撤销的操作;
    /// </summary>
    public interface IVoidableOperation
    {
        /// <summary>
        /// 重新执行命令;
        /// </summary>
        void Redo();

        /// <summary>
        /// 撤销这操作;
        /// </summary>
        void Undo();
    }

    /// <summary>
    /// 操作记录;
    /// </summary>
    public class OperationRecorder<T>
        where T : IVoidableOperation
    {
        internal const int DefaultMaxRecord = 20;

        public OperationRecorder() : this(DefaultMaxRecord)
        {
        }

        public OperationRecorder(int maxRecord)
        {
            if (maxRecord <= 0)
                throw new ArgumentOutOfRangeException("maxRecord :" + maxRecord);

            MaxRecord = maxRecord;
            operationQueue = new Collections.LinkedList<T>();
            current = null;
        }

        readonly Collections.LinkedList<T> operationQueue;
        Collections.LinkedListNode<T> current;
        public int MaxRecord { get; set; }

        /// <summary>
        /// 指令总数;
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
            get { return current == null ? default(T) : current.Value; }
        }

        /// <summary>
        /// 所有指令;
        /// </summary>
        public IEnumerable<T> Operations
        {
            get { return operationQueue; }
        }

        /// <summary>
        /// 记录这个操作;
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
        public bool PerformUndo()
        {
            if (current == null)
            {
                return false;
            }
            var operation = current.Value;
            operation.Undo();
            current = current.Previous;
            return true;
        }

        /// <summary>
        /// 执行重做,若出现异常则弹出异常,并且不进行指令移动;
        /// </summary>
        public bool PerformRedo()
        {
            if (current == null)
            {
                if (operationQueue.First != null)
                {
                    var operation = operationQueue.First.Value;
                    operation.Redo();
                    current = operationQueue.First;
                    return true;
                }
            }
            else
            {
                if (current.Next != null)
                {
                    var operation = current.Next.Value;
                    operation.Redo();
                    current = current.Next;
                    return true;
                }
            }
            return false;
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
