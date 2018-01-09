using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.UI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Initializers
{
    /// <summary>
    /// 模组初始化;
    /// </summary>
    public static class ModificationInitializer
    {
        /// <summary>
        /// 默认的读取顺序,若为Null,则从配置获取;
        /// </summary>
        public static ILoadOrder Order { get; private set; }

        /// <summary>
        /// 初始化;
        /// </summary>
        public static Task Initialize()
        {
            throw new NotImplementedException();
        }

        public static async Task InternalInitialize(IProgress<ProgressInfo> progress, CancellationToken token)
        {
            progress.Report(new ProgressInfo(0.1f, "程序初始化"));
            await ProgramInitializer.Initialize();

            await Task.Delay(3000, token);

            progress.Report(new ProgressInfo(0.3f, "数据初始化"));
            var basicResourceProgress = new LocalProgress(progress, 0.1f, 0.5f);
            await BasicResourceInitializer.Initialize(Order, basicResourceProgress, token);

            await Task.Delay(3000, token);

            progress.Report(new ProgressInfo(0.6f, "初始化完成"));
            await Stage.GoMainMenuScene();
        }

        public static Task Reinitialize(IProgress<ProgressInfo> progress)
        {
            throw new NotImplementedException();
        }

        public static void SetOrder(ILoadOrder order)
        {
            throw new NotImplementedException();
        }
    }
}
