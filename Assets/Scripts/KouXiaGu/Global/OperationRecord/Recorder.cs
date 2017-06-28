using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.OperationRecord
{

    public interface IRecorder : IVoidable
    {
        IEnumerable<IVoidable> Operations { get; }
        void Register(IVoidable operation);
        void Clear();
    }


    /// <summary>
    /// 记录操作;
    /// </summary>
    public class Recorder<T> : IRecorder, IVoidable
        where T : class, IVoidable
    {
        internal const int DefaultMaxRecord = 20;

        public Recorder() : this(DefaultMaxRecord)
        {
        }

        public Recorder(int maxRecord)
        {
            if (maxRecord <= 0)
                throw new ArgumentOutOfRangeException("maxRecord :" + maxRecord);

            MaxRecord = maxRecord;
            operationQueue = new Collections.LinkedList<IVoidable>();
            current = null;
        }

        readonly Collections.LinkedList<IVoidable> operationQueue;
        Collections.LinkedListNode<IVoidable> current;
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
            get { return current == null ? default(T) : current.Value as T; }
        }

        /// <summary>
        /// 所有指令;
        /// </summary>
        public IEnumerable<T> Operations
        {
            get { return operationQueue.OfType<T>(); }
        }

        IEnumerable<IVoidable> IRecorder.Operations
        {
            get { return operationQueue; }
        }

        void IRecorder.Register(IVoidable operation)
        {
            if (operation == null)
                throw new ArgumentNullException("operation");

            Register_internal(operation);
        }

        /// <summary>
        /// 记录这个操作;
        /// </summary>
        public void Register(T operation)
        {
            if (operation == null)
                throw new ArgumentNullException("operation");

            Register_internal(operation);
        }

        void Register_internal(IVoidable operation)
        {
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
            if (current == null)
            {
                return;
            }
            var operation = current.Value;
            operation.PerformUndo();
            current = current.Previous;
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
                    operation.PerformRedo();
                    current = operationQueue.First;
                }
            }
            else
            {
                if (current.Next != null)
                {
                    var operation = current.Next.Value;
                    operation.PerformRedo();
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
