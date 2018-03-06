using JiongXiaGu.Unity.Initializers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RunTime
{

    /// <summary>
    /// 程序管理;
    /// </summary>
    public static class Program
    {
        public static bool IsComponentInitialized { get; private set; } = false;
        private static List<IComponentInitializeHandle> componentInitializeHandlers = new List<IComponentInitializeHandle>();




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

        private static Task InternalInitialize()
        {
            ModificationResource.SearcheAll();
            ComponentInitializer.Instance.Initialize();
            return Task.CompletedTask;
        }
    }
}
