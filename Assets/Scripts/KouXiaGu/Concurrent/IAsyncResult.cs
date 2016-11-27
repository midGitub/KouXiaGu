using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Concurrent
{

    /// <summary>
    /// 等待异步结果;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsyncResult<T>
    {
        /// <summary>
        /// 是否完成;
        /// </summary>
        bool IsCompleted { get; }
        /// <summary>
        /// 是否由于未经处理异常的原因而完成。
        /// </summary>
        bool IsFaulted { get; }
        /// <summary>
        /// 结果返回;
        /// </summary>
        T Result { get; }
        /// <summary>
        /// 发生的异常;
        /// </summary>
        Exception Exception { get; }
    }

}
