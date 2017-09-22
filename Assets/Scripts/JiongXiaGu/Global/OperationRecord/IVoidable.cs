using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.OperationRecord
{

    /// <summary>
    /// 可撤销的;
    /// </summary>
    public interface IVoidable
    {
        /// <summary>
        /// 重新执行命令;
        /// </summary>
        void PerformRedo();

        /// <summary>
        /// 撤销这操作;
        /// </summary>
        void PerformUndo();
    }

    /// <summary>
    /// 可撤销的;
    /// </summary>
    public interface IVoidable<T>
    {
        /// <summary>
        /// 重新执行命令;
        /// </summary>
        void PerformRedo(T item);

        /// <summary>
        /// 撤销这操作;
        /// </summary>
        void PerformUndo(T item);
    }
}
