﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu
{

    internal static class Extesions
    {

        /// <summary>
        /// 不进行任何操作;
        /// </summary>
        public static T Initialization<T>(this Lazy<T> obj)
        {
            return obj.Value;
        }
    }
}
