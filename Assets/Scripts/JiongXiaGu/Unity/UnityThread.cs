using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity
{

    public static class UnityThread
    {
        /// <summary>
        /// Unity线程ID;
        /// </summary>
        public static int ThreadId { get; private set; }

        /// <summary>
        /// 是否在Unity主线程?
        /// </summary>
        public static bool IsUnityThread
        {
            get
            {
#if UNITY_EDITOR
                return !IsPlaying || Thread.CurrentThread.ManagedThreadId == ThreadId;
#else
                return Thread.CurrentThread.ManagedThreadId == ThreadId;
#endif
            }
        }

        /// <summary>
        /// Unity线程的SynchronizationContext;
        /// </summary>
        public static SynchronizationContext SynchronizationContext { get; private set; }

        /// <summary>
        /// Unity线程的TaskScheduler;
        /// </summary>
        public static TaskScheduler TaskScheduler { get; private set; }

        /// <summary>
        /// 是否不在编辑器内运行;
        /// </summary>
        public static bool IsPlaying { get; private set; } = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            ThreadId = Thread.CurrentThread.ManagedThreadId;
            SynchronizationContext = SynchronizationContext.Current;
            TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            IsPlaying = true;
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
        /// 在Unity内执行,若在Unity线程调用,则为同步执行;
        /// </summary>
        public static Task RunInUnityThread(Action action, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();
            if (IsUnityThread)
            {
                action.Invoke();
                return Task.CompletedTask;
            }
            else
            {
                return TaskHelper.Run(action, token, TaskScheduler);
            }
        }

        /// <summary>
        /// 在Unity内执行,若在Unity线程调用,则为同步执行;
        /// </summary>
        public static Task<T> RunInUnityThread<T>(Func<T> func, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();
            if (IsUnityThread)
            {
                T item = func.Invoke();
                return Task.FromResult<T>(item);
            }
            else
            {
                return TaskHelper.Run(func, token, TaskScheduler);
            }
        }
    }
}
