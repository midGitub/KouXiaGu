using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Concurrent
{

    /// <summary>
    /// 对Unity协程的拓展;
    /// </summary>
    public static class UnityCoroutineExtensions
    {
        /// <summary>
        /// 转换成Unity协程等待指令;
        /// </summary>
        public static IEnumerator WaitComplete(this Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }
            while (!task.IsCompleted)
            {
                yield return null;
            }
            if (task.IsFaulted)
            {
                throw task.Exception;
            }
        }
    }
}
