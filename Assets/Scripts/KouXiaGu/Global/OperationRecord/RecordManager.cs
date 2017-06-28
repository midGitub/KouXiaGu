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

        static IRecorder currentRecorder
        {
            get { return recorderStack.Last.Value; }
        }

        public static IRecorder DefaultRecorder
        {
            get { return recorderStack.First.Value; }
        }

        public static IDisposable AddLast(IRecorder recorder)
        {
            var node = recorderStack.AddLast(recorder);
            return new LinkedListUnsubscriber<IRecorder>(recorderStack, node);
        }

        public static void PerformUndo()
        {
            currentRecorder.PerformUndo();
        }

        public static void PerformRedo()
        {
            currentRecorder.PerformRedo();
        }
    }
}
