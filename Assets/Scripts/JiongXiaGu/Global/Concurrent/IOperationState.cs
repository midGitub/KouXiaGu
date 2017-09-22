using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Concurrent
{

    /// <summary>
    /// 操作标识;
    /// </summary>
    public interface IOperationState
    {
        /// <summary>
        /// 是否被取消了?
        /// </summary>
        bool IsCanceled { get; }
    }

    class OperationSign
    {
        public bool IsCanceled { get; set; }
    }
}
