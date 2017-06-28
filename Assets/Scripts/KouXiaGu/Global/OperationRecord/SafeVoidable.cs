using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.OperationRecord
{

    /// <summary>
    /// 带撤销,重做检查的抽象类;
    /// </summary>
    public abstract class SafeVoidable : IVoidable
    {
        /// <summary>
        /// 是否已经被撤销;
        /// </summary>
        public bool IsUndo { get; private set; }

        public abstract void PerformRedo();
        public abstract void PerformUndo();

        void IVoidable.PerformRedo()
        {
            if (IsUndo)
            {
                PerformRedo();
                IsUndo = false;
            }
            else
            {
                throw new InvalidOperationException("在没撤销时进行重做;");
            }
        }

        void IVoidable.PerformUndo()
        {
            if (IsUndo)
            {
                throw new InvalidOperationException("已经进行了撤销操作;");
            }
            else
            {
                PerformUndo();
                IsUndo = true;
            }
        }
    }
}
