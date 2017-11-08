using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity
{
    
    /// <summary>
    /// 在编辑器模式下的拓展方法;
    /// </summary>
    internal class EditorHelper
    {

        /// <summary>
        /// 输出成功报告信息;
        /// </summary>
        [System.Diagnostics.Conditional("EDITOR_LOG")]
        public static void LogComplete(string initializerName)
        {
            Debug.Log(string.Format("[{0}]初始化完成;", initializerName));
        }

        /// <summary>
        /// 输出成功报告信息;
        /// </summary>
        public static void LogComplete(string initializerName, Func<string> getMessage)
        {
            if (getMessage == null)
                throw new ArgumentNullException(nameof(getMessage));

            string message = getMessage.Invoke();
            LogComplete(initializerName, message);
        }

        /// <summary>
        /// 输出成功报告信息;
        /// </summary>
        [System.Diagnostics.Conditional("EDITOR_LOG")]
        public static void LogComplete(string initializerName, string message)
        {
            Debug.Log(string.Format("[{0}]初始化完成;[{1}]", initializerName, message));
        }


        /// <summary>
        /// 输出失败报告到Debug;
        /// </summary>
        [System.Diagnostics.Conditional("EDITOR_LOG")]
        public static void LogFault(string initializerName, Exception ex)
        {
            Debug.LogError(string.Format("[{0}]运行时遇到错误:{1}", initializerName, ex));
        }






        public static void WaitAll<T>(IReadOnlyList<T> initializeHandles, Func<T, Task> func, CancellationToken token)
        {
            Task[] tasks = new Task[initializeHandles.Count];
            for (int i = 0; i < initializeHandles.Count; i++)
            {
                var initializeHandle = initializeHandles[i];
                Task task = func(initializeHandle);
                tasks[i] = task ?? Task.CompletedTask;
            }
            Task.WaitAll(tasks, token);
        }
    }
}
