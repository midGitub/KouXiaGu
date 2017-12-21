using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 抽象类 初始化器;
    /// </summary>
    public abstract class InitializeScheduler : MonoBehaviour
    {
        /// <summary>
        /// 并发执行数;
        /// </summary>
        [Range(-1, 10)]
        [SerializeField]
        private int maxDegreeOfParallelism = -1;
        private TaskCompletionSource<object> taskCompletionSource;
        public bool IsInitialized { get; private set; }
        public Task InitializeTask => taskCompletionSource.Task;
        private CancellationTokenSource CancellationTokenSource;
        private CancellationToken CancellationToken => CancellationTokenSource.Token;
        public int MaxDegreeOfParallelism => maxDegreeOfParallelism;

        protected virtual void Awake()
        {
            taskCompletionSource = new TaskCompletionSource<object>();
            CancellationTokenSource = new CancellationTokenSource();
        }

        protected virtual void OnDestroy()
        {
            Cancel();
        }

        /// <summary>
        /// 开始进行初始化;
        /// </summary>
        protected async Task StartInitialize()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                try
                {
                    await Task.Run((Action)InternalInitialize);
                    taskCompletionSource.SetResult(null);
                    OnCompleted();
                }
                catch (AggregateException ex)
                {
                    taskCompletionSource.SetException(ex);
                    CancellationTokenSource.Cancel();
                    OnFaulted(ex);
                }
            }
        }

        private void InternalInitialize()
        {
            OnFirst(CancellationToken);
            CancellationToken.ThrowIfCancellationRequested();

            ParallelOptions parallelOptions = new ParallelOptions()
            {
                CancellationToken = CancellationToken,
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
            };
            Parallel.ForEach(EnumerateInitializeHandler(CancellationToken), parallelOptions, action => action.Invoke());
        }

        /// <summary>
        /// 取消初始化;
        /// </summary>
        public void Cancel()
        {
            CancellationTokenSource.Cancel();
            taskCompletionSource.TrySetCanceled();
        }

        /// <summary>
        /// 在初始化之前调用的方法;
        /// </summary>
        protected virtual void OnFirst(CancellationToken token)
        {
        }

        /// <summary>
        /// 获取到初始化方法;
        /// </summary>
        protected abstract IEnumerable<Action> EnumerateInitializeHandler(CancellationToken token);

        /// <summary>
        /// 当初始化完毕后遇到错误调用;
        /// </summary>
        protected virtual void OnFaulted(AggregateException ex)
        {
        }

        /// <summary>
        /// 当初始化完成时调用;
        /// </summary>
        protected virtual void OnCompleted()
        {
        }
    }
}
