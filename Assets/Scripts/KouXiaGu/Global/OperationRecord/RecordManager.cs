using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.OperationRecord
{

    /// <summary>
    /// 操作记录管理;
    /// </summary>
    public sealed class RecordManager
    {
        static RecordManager()
        {
            recorderStack = new Stack<Recorder<IVoidable>>();
        }

        static readonly Stack<Recorder<IVoidable>> recorderStack;

        /// <summary>
        /// 执行撤销操作;
        /// </summary>
        public static bool PerformUndo()
        {
            var recorder = recorderStack.Peek();
            return recorder.PerformUndo();
        }

        /// <summary>
        /// 执行重做操作;
        /// </summary>
        public static bool PerformRedo()
        {
            var recorder = recorderStack.Peek();
            return recorder.PerformRedo();
        }
    }
}
