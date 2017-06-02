using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;

namespace KouXiaGu
{
    public static class XiaGu
    {
        static int mainThreadId;

        /// <summary>
        /// 是否在Unity主线程?
        /// </summary>
        public static bool IsMainThread
        {
            get { return Thread.CurrentThread.ManagedThreadId == mainThreadId; }
        }

        /// <summary>
        /// 是否不在编辑器内运行;
        /// </summary>
        public static bool IsPlaying { get; private set; }

        internal static void Initialize()
        {
            mainThreadId = Thread.CurrentThread.ManagedThreadId;
            IsPlaying = true;
        }

    }
}
