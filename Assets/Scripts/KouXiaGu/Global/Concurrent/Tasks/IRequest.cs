using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu.Concurrent
{

    /// <summary>
    /// 表示一个请求;
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// 是否已经完成?
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// 表示对应操作,直到 IsCompleted 为true时停止调用;
        /// </summary>
        void MoveNext();
    }
}
