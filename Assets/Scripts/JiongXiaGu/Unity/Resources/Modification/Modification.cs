using JiongXiaGu.Unity.Initializers;
using JiongXiaGu.Unity.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组;(仅Unity线程操作)
    /// </summary>
    public static class Modification
    {
        /// <summary>
        /// 核心资源;
        /// </summary>
        public static ModificationContent Core { get; private set; }

        /// <summary>
        /// 所有模组信息,不包括核心资源;
        /// </summary>
        public static List<ModificationInfo> ModificationInfos { get; private set; }

        /// <summary>
        /// 需要读取的模组资源,包括核心资源;
        /// </summary>
        internal static List<ModificationContent> ModificationContents { get; private set; }

        /// <summary>
        /// 默认的读取顺序,若为Null,则从配置获取;
        /// </summary>
        public static ActiveModification? DefaultActiveModification { get; private set; }

        /// <summary>
        /// 资源合集;
        /// </summary>
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
            ModificationInfos = new List<ModificationInfo>();

            string directory = Path.Combine(Resource.StreamingAssetsPath, "Data");
            Core = contentSearcher.Factory.Read(directory);

            var mods = contentSearcher.Searche(Resource.ModDirectory);
            ModificationInfos.AddRange(mods);

            var userMods = contentSearcher.Searche(Resource.UserModDirectory);
            ModificationInfos.AddRange(userMods);
        }

        /// <summary>
        /// 尝试获取到对应模组信息;
        /// </summary>
        public static bool TryGetInfo(string id, out ModificationInfo info)
        {
            int index = ModificationInfos.FindIndex(item => item.Description.ID == id);
            if (index >= 0)
            {
                info = ModificationInfos[index];
                return true;
            }
            else
            {
                info = default(ModificationInfo);
                return false;
            }
        }

        /// <summary>
        /// 设置读取顺序;
        /// </summary>
        public static void SetOrder(ActiveModification order)
        {
            UnityThread.ThrowIfNotUnityThread();
            if (IsInitializing)
                throw new InvalidOperationException("正在初始化,无法变更!");

            DefaultActiveModification = order;
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
                ModificationContents = GetModificationContent();
                SharedContent = new SharedContent(ModificationContents);

                progress.Report(new ProgressInfo(0.3f, "模组初始化"));
                var basicResourceProgress = new LocalProgress(progress, 0.3f, 0.5f);
                await BasicResourceInitializer.StartInitialize(ModificationContents, basicResourceProgress, token);
                basicInitializationTask.SetResult(null);

                progress.Report(new ProgressInfo(0.5f, "模组数据初始化"));
                var resourceProgress = new LocalProgress(progress, 0.5f, 1f);
                await ResourceInitializer.StartInitialize(ModificationContents, resourceProgress, token);

                progress.Report(new ProgressInfo(1f, "初始化完毕"));

                IsComplete = true;
            }
            catch (Exception ex)
            {
                basicInitializationTask.TrySetException(ex);
                throw ex;
            }
            finally
            {
                IsInitializing = false;
                cancellationTokenSource = null;
                initializeTask = null;
                basicInitializationTask = null;
            }
        }

        /// <summary>
        /// 根据预先定义的模组顺序获取到激活的模组(按先后读取顺序);
        /// </summary>
        public static List<ModificationInfo> GetActiveModificationInfos()
        {
            try
            {
                ActiveModification order;
                ActiveModificationSerializer serializer = new ActiveModificationSerializer();

                order = serializer.Deserialize();
                return GetActiveModificationInfos(order);
            }
            catch
            {
                List<ModificationInfo> newList = new List<ModificationInfo>();
                return newList;
            }
        }

        /// <summary>
        /// 根据模组顺序获取到激活的模组(按先后读取顺序);
        /// </summary>
        public static List<ModificationInfo> GetActiveModificationInfos(ActiveModification activeModification)
        {
            List<ModificationInfo> newList = new List<ModificationInfo>();

            if (ModificationInfos != null)
            {
                foreach (var id in activeModification.IDList)
                {
                    int index = ModificationInfos.FindIndex(info => info.Description.ID == id);
                    if (index >= 0)
                    {
                        ModificationInfo info = ModificationInfos[index];
                        newList.Add(info);
                    }
                }
            }

            return newList;
        }

        /// <summary>
        /// 筛选模组;
        /// </summary>
        public static List<ModificationInfo> GetIdleModificationInfos(IList<ModificationInfo> activeModificationInfos)
        {
            var idleModificationInfos = new List<ModificationInfo>();

            if (ModificationInfos != null)
            {
                foreach (var modificationInfo in ModificationInfos)
                {
                    if (!activeModificationInfos.Contains(modificationInfo))
                    {
                        idleModificationInfos.Add(modificationInfo);
                    }
                }
            }

            return idleModificationInfos;
        }

        /// <summary>
        /// 根据预先定义的模组顺序获取到激活的模组(按先后读取顺序),包含核心模组;
        /// </summary>
        private static List<ModificationContent> GetModificationContent()
        {
            if (DefaultActiveModification == null)
            {
                try
                {
                    ActiveModification order;
                    ActiveModificationSerializer serializer = new ActiveModificationSerializer();

                    order = serializer.Deserialize();
                    return GetModificationContent(order);
                }
                catch(FileNotFoundException)
                {
                    List<ModificationContent> newList = new List<ModificationContent>();
                    newList.Add(Core);
                    return newList;
                }
            }
            else
            {
                var mod = GetModificationContent(DefaultActiveModification.Value);
                return mod;
            }
        }

        /// <summary>
        /// 根据模组顺序获取到激活的模组(按先后读取顺序),包含核心模组;
        /// </summary>
        private static List<ModificationContent> GetModificationContent(ActiveModification activeModification)
        {
            List<ModificationContent> newList = new List<ModificationContent>();
            newList.Add(Core);
            ModificationFactory factory = new ModificationFactory();

            if (ModificationContents == null)
            {
                foreach (var id in activeModification.IDList)
                {
                    int index = ModificationInfos.FindIndex(_info => _info.Description.ID == id);
                    if (index >= 0)
                    {
                        var info = ModificationInfos[index];
                        ModificationContent content = factory.Read(info);
                        newList.Add(content);
                    }
                    else
                    {
                        Debug.LogWarning(string.Format("未找到ID为[{0}]的模组;", id));
                    }
                }
            }
            else
            {
                List<ModificationContent> old = ModificationContents;

                foreach (var info in activeModification.IDList)
                {
                    ModificationContent content;
                    int index = old.FindIndex(oldContent => oldContent.Description.ID == info);
                    if (index >= 0)
                    {
                        content = old[index];
                        old[index] = null;
                    }
                    else
                    {
                        content = factory.Read(info);
                    }

                    newList.Add(content);
                }

                foreach (var mod in old)
                {
                    if (mod != null)
                    {
                        mod.UnloadAssetBundlesAll(true);
                        mod.Dispose();
                    }
                }
            }

            return newList;
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
