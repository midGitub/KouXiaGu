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
    public static class ProgramInitializer
    {
        private static TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
        public static Task WorkTask => taskCompletionSource.Task;
        public static bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// 进行初始化,若已经初始化,初始化中则无任何操作;
        /// </summary>
        public static Task Initialize()
        {
            if (IsInitialized)
            {
                return WorkTask;
            }
            else
            {
                return InternalInitialize();
            }
        }

        private static async Task InternalInitialize()
        {
            try
            {
                IsInitialized = true;
                await LoadableResource.SearcheAll();
                await ComponentInitializer.Initialize();
                taskCompletionSource.SetResult(null);
            }
            catch (Exception ex)
            {
                taskCompletionSource.SetException(ex);
            }
        }
    }
}
