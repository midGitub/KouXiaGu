﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Concurrent
{


    public static class RequestDispatcherExtensions
    {

        /// <summary>
        /// 添加委托请求;
        /// </summary>
        public static void Add(this IRequestDispatcher dispatcher, Action action)
        {
            dispatcher.Add(new AsyncRequest(action));
        }
    }
}
