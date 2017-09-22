using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using JiongXiaGu.Diagnostics;

namespace JiongXiaGu
{

    [ConsoleMethodsClass]
    public static class XiaGu
    {
        static XiaGu()
        {
            IsDeveloperMode = true;
        }

        static int mainThreadId;

        /// <summary>
        /// 是否在Unity主线程?
        /// </summary>
        public static bool IsUnityThread
        {
            get
            {
#if UNITY_EDITOR
                return !IsPlaying || Thread.CurrentThread.ManagedThreadId == mainThreadId;
#else
                return Thread.CurrentThread.ManagedThreadId == mainThreadId;
#endif
            }
        }

        /// <summary>
        /// 是否不在编辑器内运行;
        /// </summary>
        public static bool IsPlaying { get; private set; }

        /// <summary>
        /// 是否为开发者模式?
        /// </summary>
        public static bool IsDeveloperMode { get; set; }

        /// <summary>
        /// 系统语言枚举;
        /// </summary>
        public static SystemLanguage SystemLanguage { get; private set; }

        internal static void Initialize()
        {
            mainThreadId = Thread.CurrentThread.ManagedThreadId;
            IsPlaying = true;
            SystemLanguage = Application.systemLanguage;
        }

        /// <summary>
        /// 若不是Unity线程则抛出异常;
        /// </summary>
        public static void ThrowIfNotUnityThread()
        {
            if (!IsUnityThread)
            {
                throw new NotImplementedException("仅允许在Unity线程调用!");
            }
        }



        [ConsoleMethod("developer", "开发者模式开关", "bool")]
        public static void Developer(string isDeveloperMode)
        {
            IsDeveloperMode = Convert.ToBoolean(isDeveloperMode);
            Developer();
        }

        [ConsoleMethod("developer", "显示是否为开发者模式")]
        public static void Developer()
        {
            XiaGuConsole.Log("DeveloperMode :" + IsDeveloperMode);
        }
    }
}
