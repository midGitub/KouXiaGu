using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.UI;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 负责设定默认模组读取顺序,并且初始化模组的基本信息;
    /// </summary>
    [DisallowMultipleComponent]
    public class InitializationSceneController : MonoBehaviour, ISceneController
    {
        private InitializationSceneController()
        {
        }

        [SerializeField]
        private ProgressBar progressBar;

        public bool AutoGoMainMenuScene { get; private set; } = false;

        Task ISceneController.QuitScene()
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
