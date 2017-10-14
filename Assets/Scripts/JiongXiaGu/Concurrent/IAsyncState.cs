using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Concurrent
{

    /// <summary>
    /// 状态信息;
    /// </summary>
    public interface IAsyncState
    {
        bool IsCompleted { get; }
        bool IsFaulted { get; }
        AggregateException Exception { get; }
    }
}
