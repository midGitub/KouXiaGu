using System;
using System.IO;
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
                return Task.FromResult(item);
            }
            else
            {
                return TaskHelper.Run(func, token, TaskScheduler);
            }
        }


        /// <summary>
        /// 转换成 Task 格式;
        /// </summary>
        public static Task AsTask(this AsyncOperation asyncOperation)
        {
            if (asyncOperation == null)
                throw new ArgumentNullException(nameof(asyncOperation));
            if (asyncOperation.isDone)
                return Task.CompletedTask;

            var taskCompletionSource = new TaskCompletionSource<object>();
            asyncOperation.completed += delegate (AsyncOperation operation)
            {
                if (operation.isDone)
                {
                    taskCompletionSource.SetResult(null);
                }
                else
                {
                    taskCompletionSource.SetException(new NotImplementedException());
                }
            };
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// 转换成 Task 格式;
        /// </summary>
        public static Task<AssetBundle> AsTask(this AssetBundleCreateRequest asyncOperation)
        {
            if (asyncOperation == null)
                throw new ArgumentNullException(nameof(asyncOperation));

            var taskCompletionSource = new TaskCompletionSource<AssetBundle>();
            asyncOperation.completed += delegate (AsyncOperation operation)
            {
                var assetBundle = asyncOperation.assetBundle;
                if (assetBundle == null)
                {
                    taskCompletionSource.SetException(new IOException("无法读取到 AssetBundle;"));
                }
                else
                {
                    taskCompletionSource.SetResult(assetBundle);
                }
            };

            return taskCompletionSource.Task;
        }
    }
}
