using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Initializers
{


    internal static class InitializeScheduler
    {

        /// <summary>
        /// 等待所有工作完成,
        /// </summary>
        public static void WaitAll<T>(IReadOnlyList<T> initializeHandles, Func<T, Task> func, CancellationToken token)
        {
            Task task;
            Task[] tasks = new Task[initializeHandles.Count];
            for (int i = 0; i < initializeHandles.Count; i++)
            {
                token.ThrowIfCancellationRequested();
                var initializeHandle = initializeHandles[i];
                task = func(initializeHandle);
                tasks[i] = task ?? Task.CompletedTask;

                for (int taskIndex = 0; taskIndex < tasks.Length; taskIndex++)
                {
                    task = tasks[taskIndex];
                    if (task != null)
                    {
                        if (task.IsFaulted)
                        {
                            throw task.Exception;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            Task.WaitAll(tasks, token);
        }
    }
}
