using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public OperationRecorder(int maxRecord)
        {
            if (maxRecord <= 0)
                throw new ArgumentOutOfRangeException("maxRecord :" + maxRecord);

            operationQueue = new T[maxRecord];
            currentIndex = 0;
            lastIndex = 0;
            Count = 0;
        }

        readonly T[] operationQueue;

        /// <summary>
        /// 指向第一条指令位置;
        /// </summary>
        int currentIndex;

        /// <summary>
        /// 指向最后一条指令位置;
        /// </summary>
        int lastIndex;

        /// <summary>
        /// 指令总数;
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 最大记录;
        /// </summary>
        public int MaxRecord
        {
            get { return operationQueue.Length; }
        }

        public bool IsFull
        {
            get { return Count == MaxRecord; }
        }

        /// <summary>
        /// 记录这个操作;
        /// </summary>
        public void Register(T operation)
        {
            if (operation == null)
                throw new ArgumentNullException("operation");

            operationQueue[lastIndex] = operation;
            lastIndex = MoveNext(lastIndex);
            if (IsFull)
            {
                currentIndex = MoveNext(currentIndex);
            }
            else
            {
                Count++;
            }
        }

        /// <summary>
        /// 将下标向后移动一个单位;
        /// </summary>
        int MoveNext(int index)
        {
            index++;
            if (index >= operationQueue.Length)
            {
                index = 0;
            }
            return index;
        }

        /// <summary>
        /// 将下标向前移动一个单位;
        /// </summary>
        int MovePrevious(int index)
        {
            index--;
            if (index < 0)
            {
                index = operationQueue.Length - 1;
            }
            return index;
        }

        /// <summary>
        /// 执行撤销;
        /// </summary>
        public void PerformUndo()
        {


            var operation = operationQueue[lastIndex];
            if (operation == null)
            {
                return;
            }
            operation.Undo();
            lastIndex = MovePrevious(lastIndex);
        }

        /// <summary>
        /// 执行重做;
        /// </summary>
        public void PerformRedo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 清除所有记录;
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
