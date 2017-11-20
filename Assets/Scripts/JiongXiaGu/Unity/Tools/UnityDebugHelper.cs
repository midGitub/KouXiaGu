using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 提供标准的输出格式;
    /// </summary>
    internal class UnityDebugHelper
    {

        internal const string LogPragma = "EDITOR_LOG";


        /// <summary>
        /// 输出成功报告信息;
        /// </summary>
        [System.Diagnostics.Conditional(LogPragma)]
        public static void SuccessfulReport(string name)
        {
            Debug.Log(string.Format("[{0}]运行完成;", name));
        }

        /// <summary>
        /// 输出成功报告信息;
        /// </summary>
        [System.Diagnostics.Conditional(LogPragma)]
        public static void SuccessfulReport(string name, Func<string> getMessage)
        {
            if (getMessage == null)
                throw new ArgumentNullException(nameof(getMessage));

            string message = getMessage.Invoke();
            SuccessfulReport(name, message);
        }

        /// <summary>
        /// 输出成功报告信息;
        /// </summary>
        [System.Diagnostics.Conditional(LogPragma)]
        public static void SuccessfulReport(string name, string message)
        {
            Debug.Log(string.Format("[{0}]运行完成;[{1}]", name, message));
        }


        /// <summary>
        /// 输出失败报告到Debug;
        /// </summary>
        [System.Diagnostics.Conditional(LogPragma)]
        public static void FailureReport(string name, Exception ex)
        {
            Debug.LogError(string.Format("[{0}]运行时遇到错误:{1}", name, ex));
        }


        /// <summary>
        /// 输出一条警告消息;
        /// </summary>
        public static void LogWarning(string name, string message)
        {
            string text = string.Format("[{0}]{1}", name, message);
            Debug.LogWarning(text);
        }

        /// <summary>
        /// 输出一条警告消息;
        /// </summary>
        public static void LogWarning(string name, string message, Exception ex)
        {
            string text = string.Format("[{0}]Message : {1} ; Exception :{2} ;", name, message, ex);
            Debug.LogWarning(text);
        }


        /// <summary>
        /// 输出一条错误消息;
        /// </summary>
        public static void LogError(string name, string message)
        {
            string text = string.Format("[{0}]{1}", name, message);
            Debug.LogWarning(text);
        }

        /// <summary>
        /// 输出一条错误消息;
        /// </summary>
        public static void LogError(string name, string message, Exception ex)
        {
            string text = string.Format("[{0}]Message : {1} ; Exception :{2} ;", name, message, ex);
            Debug.LogWarning(text);
        }
    }
}
