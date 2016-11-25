using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Concurrent
{

    public interface IResultAsync<T>
    {
        /// <summary>
        /// 是否完成;
        /// </summary>
        bool IsDone { get; }
        /// <summary>
        /// 结果返回;
        /// </summary>
        T Result { get; }
    }

}
