using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using JiongXiaGu.Unity.UI;
using System.Threading;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.Unity.Initializers
{

    [DisallowMultipleComponent]
    public class InitializationSceneController : MonoBehaviour, ISceneController
    {
        private InitializationSceneController()
        {
        }

        /// <summary>
        /// 默认的读取顺序,若为Null,则从配置获取;
        /// </summary>
        public static LoadOrder Order { get; private set; }

        [SerializeField]
        private ProgressBar progressBar;

        public bool AutoGoMainMenuScene { get; private set; } = false;

        private Task workTask;
        private CancellationTokenSource cancellationTokenSource;

        private void Awake()
        {
            cancellationTokenSource = new CancellationTokenSource();
        }

        private void Start()
        {
            workTask = Initializate(cancellationTokenSource.Token);
        }

        private async Task Initializate(CancellationToken token)
        {
            progressBar.Progress.Report(new ProgressInfo(0.1f, "程序初始化"));
            await Stage.ProgramInitializationTask;

            await Task.Delay(3000);

            progressBar.Progress.Report(new ProgressInfo(0.3f, "数据初始化"));
            await BasicResourceInitializer.Initialize(token);

            await Task.Delay(3000);

            progressBar.Progress.Report(new ProgressInfo(0.3f, "初始化完成"));
            await Stage.GoMainMenuScene();
        }

        public Task Quit()
        {
            throw new NotImplementedException();
        }

        private void Test()
        {
            Task.Run(delegate ()
            {
                int count = 100;
                for (int i = 0; i <= count; i++)
                {
                    Thread.Sleep(100);
                    progressBar.Progress.Report(new ProgressInfo(i / (float)count));
                }
            });
        }
    }
}
