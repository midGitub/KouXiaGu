using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Initialization
{

    public interface IAsyncOperate
    {
        /// <summary>
        /// 是否完成?
        /// </summary>
        bool IsCompleted { get; }

        bool IsError { get; }

        Exception Error { get; }
    }

}
