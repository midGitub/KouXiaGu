using System;
using JiongXiaGu.Unity.Initializers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.GameConsoles;
using JiongXiaGu.Unity.KeyInputs;

namespace JiongXiaGu.Unity.RunTime
{

    /// <summary>
    /// 程序管理;
    /// </summary>
    public static class Program
    {
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
