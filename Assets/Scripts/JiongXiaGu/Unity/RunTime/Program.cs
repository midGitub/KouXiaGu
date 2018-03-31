using JiongXiaGu.Unity.GameConsoles;
using JiongXiaGu.Unity.KeyInputs;
using JiongXiaGu.Unity.Resources;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RunTime
{

    /// <summary>
    /// 程序信息;
    /// </summary>
    public struct ProgramInfo
    {
        /// <summary>
        /// 默认;
        /// </summary>
        public static ProgramInfo Default => new ProgramInfo();

        /// <summary>
        /// 是否为开发者模式?
        /// </summary>
        public bool IsDeveloperMode { get; set; }
    }

    /// <summary>
    /// 程序管理;
    /// </summary>
    public static class Program
    {
        public static ProgramInfo Info { get; private set; }

        internal static Task WorkTask { get; private set; }
        public static TaskStatus WorkTaskStatus => WorkTask != null ? WorkTask.Status : TaskStatus.WaitingToRun;

        /// <summary>
        /// 程序组件初始化,在Unity线程调用;
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static Task Initialize()
        {
            if (WorkTask != null)
            {
                return WorkTask;
            }
            else
            {
                return WorkTask = InternalInitialize();
            }
        }

        private static async Task InternalInitialize()
        {
            try
            {
                SynchronizedInitialize();
                await Task.Run(() => ParallelInitialize());
                AfterSynchronizedInitialize();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        /// <summary>
        /// 同步初始化;
        /// </summary>
        private static void SynchronizedInitialize()
        {
            Resource.Initialize();
            Info = ReadProgramInfo();
        }

        /// <summary>
        /// 获取到程序信息;
        /// </summary>
        private static ProgramInfo ReadProgramInfo()
        {
            string filePath = Path.Combine(Resource.StreamingAssetsPath, "ProgramInfo.xml");
            XmlSerializer <ProgramInfo> xmlSerializer = new XmlSerializer<ProgramInfo>();

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var info = xmlSerializer.Deserialize(stream);
                    return info;
                }
            }
            catch (FileNotFoundException)
            {
                return ProgramInfo.Default;
            }
        }

        /// <summary>
        /// 并行初始化;
        /// </summary>
        private static void ParallelInitialize(CancellationToken cancellationToken = default(CancellationToken))
        {
            Action[] words = CreateParallelWordsArray(cancellationToken);
            ParallelOptions options = new ParallelOptions()
            {
                CancellationToken = cancellationToken,
            };

            Parallel.Invoke(options, words);
        }


        /// <summary>
        /// 创建并行工作方法;
        /// </summary>
        private static Action[] CreateParallelWordsArray(CancellationToken cancellationToken)
        {
            Action[] words = new Action[]
            {
                ModificationController.Initialize,
                GameConsole.Initialize,
                KeyInput.Initialize,
            };
            return words;
        }

        /// <summary>
        /// 同步初始化;
        /// </summary>
        private static void AfterSynchronizedInitialize()
        {
            Debug.Log("初始化完成!");
            return;
        }
    }
}
