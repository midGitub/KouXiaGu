using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu
{

    /// <summary>
    /// 状态信息;
    /// </summary>
    public interface IOperationState
    {
        bool IsCompleted { get; }
        bool IsFaulted { get; }
        bool IsCanceled { get; }
        AggregateException Exception { get; }
    }
}
