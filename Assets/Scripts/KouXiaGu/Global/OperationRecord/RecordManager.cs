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
            recorderStack = new LinkedList<IRecorder>();
            var defaultRecorder = new Recorder<IVoidable>();
            recorderStack.AddFirst(defaultRecorder);
        }

        static readonly LinkedList<IRecorder> recorderStack;

        public static IRecorder Current
        {
            get { return recorderStack.Last.Value; }
        }

        public static IDisposable AddRecorder(IRecorder recorder)
        {
            var node = recorderStack.AddLast(recorder);
            return new LinkedListUnsubscriber<IRecorder>(recorderStack, node);
        }

        /// <summary>
        /// 执行撤销操作;
        /// </summary>
        public static bool PerformUndo()
        {
            var recorder = recorderStack.Last.Value;
            return recorder.PerformUndo();
        }

        /// <summary>
        /// 执行重做操作;
        /// </summary>
        public static bool PerformRedo()
        {
            var recorder = recorderStack.Last.Value;
            return recorder.PerformRedo();
        }
    }
}
