using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity
{

    public static class UnityThread
    {
        public static bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Unity线程ID;
        /// </summary>
        public static int ThreadId { get; private set; }

        /// <summary>
        /// 是否在Unity主线程?
        /// </summary>
        public static bool IsUnityThread => Thread.CurrentThread.ManagedThreadId == ThreadId;

        /// <summary>
        /// Unity线程的SynchronizationContext;
        /// </summary>
        public static SynchronizationContext SynchronizationContext { get; private set; }

        /// <summary>
        /// Unity线程的 TaskScheduler;
        /// </summary>
        public static TaskScheduler TaskScheduler { get; private set; }

        /// <summary>
        /// 提供 Unity线程的 TaskFactory;
        /// </summary>
        public static TaskFactory TaskFactory { get; private set; }

        /// <summary>
        /// 是否不在编辑器内运行;
        /// </summary>
        public static bool IsPlaying { get; private set; } = false;

        /// <summary>
        /// 初始化,在Unity线程调用!
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                ThreadId = Thread.CurrentThread.ManagedThreadId;
                SynchronizationContext = SynchronizationContext.Current;
                TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
                TaskFactory = new TaskFactory(TaskScheduler);
                IsPlaying = Application.isPlaying;
            }
        }

        /// <summary>
        /// 若不是Unity线程则抛出异常;
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ThrowIfNotUnityThread()
        {
            if (!IsUnityThread)
            {
                throw new InvalidOperationException("仅允许在Unity线程调用!");
            }
        }

        /// <summary>
        /// 在Unity线程内执行;
        /// </summary>
        public static Task Run(Action action)
        {
            return TaskFactory.StartNew(action);
        }

        /// <summary>
        /// 在Unity线程内执行;
        /// </summary>
        public static Task Run(Action action, CancellationToken cancellationToken)
        {
            return TaskFactory.StartNew(action, cancellationToken);
        }

        /// <summary>
        /// 在Unity线程内执行;
        /// </summary>
        public static Task<T> Run<T>(Func<T> function)
        {
            return TaskFactory.StartNew(function);
        }

        /// <summary>
        /// 在Unity线程内执行;
        /// </summary>
        public static Task<T> Run<T>(Func<T> function, CancellationToken cancellationToken)
        {
            return TaskFactory.StartNew(function, cancellationToken);
        }
    }
}
