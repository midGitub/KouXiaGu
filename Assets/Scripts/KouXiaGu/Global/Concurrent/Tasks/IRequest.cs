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
        /// 表示对应操作;
        /// </summary>
        void MoveNext();
    }

    /// <summary>
    /// 带返回结果的请求;
    /// </summary>
    public interface IRequest<T> : IRequest
    {
        /// <summary>
        /// 请求结果;
        /// </summary>
        T Result { get; }
    }
}
