using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.OperationRecord
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
}
