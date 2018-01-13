using JiongXiaGu.Unity.Initializers;
using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组初始化;
    /// </summary>
    public static class Modification
    {
        /// <summary>
        /// 默认的读取顺序,若为Null,则从配置获取;
        /// </summary>
        public static ILoadOrder Order { get; private set; }

        public static ModificationContent Core { get; private set; }
        private static List<ModificationContent> all;
        public static IReadOnlyCollection<ModificationContent> All { get; private set; }
        public static SharedContent SharedContent { get; private set; }


        public static bool IsInitializing { get; private set; } = false;
        public static bool IsComplete { get; private set; } = false;

        private static Task initializeTask;
        private static TaskCompletionSource<object> basicInitializationTask;
        private static CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// 寻找所有模组;
        /// </summary>
        internal static void SearcheAll()
        {
            ModificationSearcher contentSearcher = new ModificationSearcher();
            all = new List<ModificationContent>();

            string directory = Path.Combine(Resource.StreamingAssetsPath, "Data");
            Core = contentSearcher.Factory.Read(directory);
            all.Add(Core);

            var mods = contentSearcher.Find(Resource.ModDirectory);
            all.AddRange(mods);

            var userMods = contentSearcher.Find(Resource.UserModDirectory);
            all.AddRange(userMods);
        }

        /// <summary>
        /// 设置读取顺序;
        /// </summary>
        public static void SetOrder(ILoadOrder order)
        {
            UnityThread.ThrowIfNotUnityThread();
            if (IsInitializing)
                throw new InvalidOperationException("正在初始化,无法变更!");

            Order = order;
        }

        /// <summary>
        /// 进行初始化,若已经初始化,初始化中则无任何操作;
        /// </summary>
        public static InitializationStage Initialize(IProgress<ProgressInfo> progress)
        {
            UnityThread.ThrowIfNotUnityThread();
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));

            if (!IsInitializing)
            {
                IsInitializing = true;
                IsComplete = false;
                cancellationTokenSource = new CancellationTokenSource();
                initializeTask = InternalInitialize(progress, cancellationTokenSource.Token);
                basicInitializationTask = new TaskCompletionSource<object>();
            }
            else if (cancellationTokenSource.IsCancellationRequested)
            {
                throw new InvalidOperationException("初始化正在被取消!");
            }

            return new InitializationStage(basicInitializationTask.Task, initializeTask);
        }

        /// <summary>
        /// 取消初始化;
        /// </summary>
        public static Task Cancel(IProgress<ProgressInfo> progress)
        {
            UnityThread.ThrowIfNotUnityThread();
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));

            if (initializeTask != null)
            {
                cancellationTokenSource.Cancel();
                return initializeTask.ContinueWith(delegate (Task task)
                {
                    IsInitializing = false;
                    IsComplete = false;
                    cancellationTokenSource = null;
                    initializeTask = null;
                    basicInitializationTask = null;
                });
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        private static async Task InternalInitialize(IProgress<ProgressInfo> progress, CancellationToken token)
        {
            try
            {
                progress.Report(new ProgressInfo(0.1f, "程序初始化"));
                await Program.Initialize();

                progress.Report(new ProgressInfo(0.2f, "模组排序"));
                ILoadOrder order = await InternalGetOrder();

                progress.Report(new ProgressInfo(0.3f, "模组初始化"));
                var basicResourceProgress = new LocalProgress(progress, 0.3f, 0.5f);
                await BasicResourceInitializer.Initialize(order, basicResourceProgress, token);
                basicInitializationTask.SetResult(null);

                progress.Report(new ProgressInfo(0.5f, "模组数据初始化"));
                var resourceProgress = new LocalProgress(progress, 0.5f, 1f);
                await ResourceInitializer.Initialize(resourceProgress, token);

                IsComplete = true;
            }
            finally
            {
                IsInitializing = false;
                cancellationTokenSource = null;
                initializeTask = null;
                basicInitializationTask = null;
            }
        }

        private static Task<ILoadOrder> InternalGetOrder()
        {
            if (Order == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                return Task.FromResult(Order);
            }
        }

        public struct InitializationStage
        {
            public Task BasicInitializationTask { get; private set; }
            public Task InitializationTask { get; private set; }

            public InitializationStage(Task basicInitializationTask, Task initializationTask)
            {
                BasicInitializationTask = basicInitializationTask;
                InitializationTask = initializationTask;
            }
        }
    }
}
