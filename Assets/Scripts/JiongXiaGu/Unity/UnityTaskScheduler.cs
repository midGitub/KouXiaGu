using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 在Unity线程内根据运行时间调度执行;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UnityTaskScheduler : MonoBehaviour
    {
        private UnityTaskScheduler()
        {
        }

        private static readonly _TaskScheduler taskScheduler = new _TaskScheduler();
        private static readonly GlobalSingleton<UnityTaskScheduler> singleton = new GlobalSingleton<UnityTaskScheduler>();

        [SerializeField]
        private Stopwatch runtimeStopwatch = new Stopwatch(0.1f);

        public static TaskScheduler TaskScheduler
        {
            get { return taskScheduler; }
        }

        private void Awake()
        {
            singleton.SetInstance(this);
        }

        private void FixedUpdate()
        {
            runtimeStopwatch.Restart();
            while (taskScheduler.MoveNext())
            {
                if (runtimeStopwatch.Await())
                {
                    break;
                }
            }
        }

        private void OnDestroy()
        {
            singleton.RemoveInstance(this);
        }

        /// <summary>
        /// 提供在Unity线程
        /// </summary>
        private class _TaskScheduler : TaskScheduler
        {
            private readonly object asyncLock = new object();
            private readonly LinkedList<Task> tasks = new LinkedList<Task>();

            /// <summary>
            /// 在Unity线程调用;
            /// </summary>
            internal bool MoveNext()
            {
                lock (asyncLock)
                {
                    var node = tasks.First;
                    if (node != null)
                    {
                        tasks.Remove(node);
                        Task task = node.Value;
                        TryExecuteTask(task);
                    }
                    return tasks.Count != 0;
                }
            }

            /// <summary>
            /// 仅对于调试器支持，生成当前排队到计划程序中等待执行的 Task 实例的枚举;
            /// </summary>
            protected override IEnumerable<Task> GetScheduledTasks()
            {
                bool lockTaken = false;
                try
                {
                    Monitor.TryEnter(asyncLock, ref lockTaken);
                    if (lockTaken) return tasks;
                    else throw new NotSupportedException();
                }
                finally
                {
                    if (lockTaken) Monitor.Exit(asyncLock);
                }

            }

            /// <summary>
            /// 将 Task 排队到计划程序中;
            /// </summary>
            protected override void QueueTask(Task task)
            {
                lock (asyncLock)
                {
                    tasks.AddLast(task);
                }
            }

            /// <summary>
            /// 尝试将以前排队到此计划程序中的 Task 取消排队;
            /// </summary>
            protected sealed override bool TryDequeue(Task task)
            {
                lock (asyncLock)
                {
                    return tasks.Remove(task);
                }
            }

            /// <summary>
            /// 确定是否可以在此调用中同步执行提供的 Task，如果可以，将执行该任务;
            /// </summary>
            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                if (!UnityThread.IsUnityThread)
                {
                    return false;
                }

                if (taskWasPreviouslyQueued)
                {
                    if (TryDequeue(task))
                    {
                        return TryExecuteTask(task);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return TryExecuteTask(task);
                }
            }
        }
    }
}
