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
        /// 是否已经被撤销;
        /// </summary>
        bool IsUndo { get; }

        /// <summary>
        /// 重新执行命令;
        /// </summary>
        void Redo();

        /// <summary>
        /// 撤销这操作;
        /// </summary>
        void Undo();
    }
}
