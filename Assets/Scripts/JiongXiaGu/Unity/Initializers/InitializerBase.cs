using JiongXiaGu.Unity.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    public enum InitializeOption
    {
        Synchronization,
        Parallel,
        Async,
    }


    public abstract class InitializerBase : MonoBehaviour
    {
        [SerializeField]
        private InitializeOption option = InitializeOption.Synchronization;

        public Task Initialize()
        {
            return Initialize(null, null, option, default(CancellationToken));
        }

        /// <summary>
        /// 同一时间仅能运行一个初始化程序;
        /// </summary>
        public Task Initialize(object state, IProgress<ProgressInfo> progress, CancellationToken token)
        {
            return Initialize(state, progress, option, token);
        }

        /// <summary>
        /// 同一时间仅能运行一个初始化程序;
        /// </summary>
        public Task Initialize(object state, IProgress<ProgressInfo> progress, InitializeOption option, CancellationToken token)
        {
            switch (option)
            {
                case InitializeOption.Synchronization:
                    InitializeSynchronization(state, progress, token);
                    return Task.CompletedTask;

                case InitializeOption.Async:
                    return InitializeAsync(state, progress, token);

                case InitializeOption.Parallel:
                    return InitializeParallelInNewThread(state, progress, token);

                default:
                    throw new InvalidOperationException(option.ToString());
            }
        }

        private Task InitializeAsync(object state, IProgress<ProgressInfo> progress, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return Task.Run(() => InitializeSynchronization(state, progress, token));
        }

        private void InitializeSynchronization(object state, IProgress<ProgressInfo> progress, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var initializeHandlers = EnumerateHandler(state);
            if (initializeHandlers != null && initializeHandlers.Count != 0)
            {
                float progressValue = 0;
                float progressIncrement = 1 / initializeHandlers.Count;

                foreach (var initializeHandler in initializeHandlers)
                {
                    token.ThrowIfCancellationRequested();

                    var message = initializeHandler.Invoke(token);
                    progressValue += progressIncrement;
                    progress?.Report(new ProgressInfo(progressValue, message));
                }
            }
        }

        private Task InitializeParallelInNewThread(object state, IProgress<ProgressInfo> progress, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return Task.Run(() => InitializeParallel(state, progress, token));
        }

        private void InitializeParallel(object state, IProgress<ProgressInfo> progress, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var initializeHandlers = EnumerateHandler(state);
            if (initializeHandlers != null && initializeHandlers.Count != 0)
            {
                float progressValue = 0;
                float progressIncrement = 1 / initializeHandlers.Count;
                ParallelOptions parallelOptions = new ParallelOptions()
                {
                    CancellationToken = token,
                };

                Parallel.ForEach(initializeHandlers, parallelOptions, delegate (Func<CancellationToken, string> action)
                {
                    token.ThrowIfCancellationRequested();

                    var message = action.Invoke(token);
                    progressValue += progressIncrement;
                    progress?.Report(new ProgressInfo(progressValue, message));
                });
            }
        }

        /// <summary>
        /// 获取到操作;
        /// </summary>
        protected abstract List<Func<CancellationToken, string>> EnumerateHandler(object state);
    }
}
