using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 程序初始化;
    /// </summary>
    public static class Program
    {
        internal static Task WorkTask { get; private set; }
        public static TaskStatus WorkTaskStatus => WorkTask != null ? WorkTask.Status : TaskStatus.WaitingToRun;

        /// <summary>
        /// 进行初始化,若已经初始化,初始化中则无任何操作;
        /// </summary>
        public static Task Initialize()
        {
            UnityThread.ThrowIfNotUnityThread();

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
            Modification.SearcheAll();
            await Task.Run((Action)ComponentInitializer.Instance.Initialize);
        }
    }
}
