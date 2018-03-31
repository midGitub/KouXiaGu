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
        /// 提供 Unity线程 FixedUpdate() 执行的 TaskFactory;
        /// </summary>
        public static TaskFactory UnityFixedUpdateTaskFactory { get; private set; }

        /// <summary>
        /// 是否不在编辑器内运行;
        /// </summary>
        public static bool IsEditMode { get; private set; } = true;

        private static readonly CancellationTokenSource unityThreadCancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Unity线程结束时进行取消;
        /// </summary>
        public static CancellationToken UnityCancellationToken => unityThreadCancellationTokenSource.Token;

        /// <summary>
        /// 初始化,在Unity线程调用!
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            ThreadId = Thread.CurrentThread.ManagedThreadId;
            SynchronizationContext = SynchronizationContext.Current;
            TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            UnityFixedUpdateTaskFactory = new TaskFactory(TaskScheduler);
            IsEditMode = false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize2()
        {
            new GameObject("UnityThreadDispatcher", typeof(UnityThreadDispatcher));
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
        /// 创建一个关联Unity线程的取消信号;(解决在Unity编辑器内运行多线程无法中断)
        /// </summary>
        public static CancellationTokenSource CreateLinkedTokenSource()
        {
            return CancellationTokenSource.CreateLinkedTokenSource(UnityCancellationToken);
        }

        /// <summary>
        /// 在Unity线程内执行;
        /// </summary>
        public static Task Run(Action action)
        {
            return UnityFixedUpdateTaskFactory.StartNew(action);
        }

        /// <summary>
        /// 在Unity线程内执行;
        /// </summary>
        public static Task Run(Action action, CancellationToken cancellationToken)
        {
            return UnityFixedUpdateTaskFactory.StartNew(action, cancellationToken);
        }

        /// <summary>
        /// 在Unity线程内执行;
        /// </summary>
        public static Task<T> Run<T>(Func<T> function)
        {
            return UnityFixedUpdateTaskFactory.StartNew(function);
        }

        /// <summary>
        /// 在Unity线程内执行;
        /// </summary>
        public static Task<T> Run<T>(Func<T> function, CancellationToken cancellationToken)
        {
            return UnityFixedUpdateTaskFactory.StartNew(function, cancellationToken);
        }


        [DisallowMultipleComponent]
        private sealed class UnityThreadDispatcher : MonoBehaviour
        {
            private void Awake()
            {
                DontDestroyOnLoad(this);
            }

            private void OnDestroy()
            {
                unityThreadCancellationTokenSource.Cancel();
            }
        }
    }
}
