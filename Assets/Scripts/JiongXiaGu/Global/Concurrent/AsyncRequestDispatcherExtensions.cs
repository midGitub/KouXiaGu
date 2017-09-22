using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Concurrent
{


    public static class AsyncRequestDispatcherExtensions
    {

        /// <summary>
        /// 添加委托请求;
        /// </summary>
        public static void Add(this IAsyncRequestDispatcher dispatcher, Action action)
        {
            dispatcher.Add(new Operate(action));
        }
    }
}
