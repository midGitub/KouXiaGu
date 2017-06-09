using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Concurrent
{

    /// <summary>
    /// 操作标识;
    /// </summary>
    public interface ISign
    {
        bool IsCanceled { get; }
    }

    class OperationSign
    {
        public bool IsCanceled { get; set; }
    }
}
