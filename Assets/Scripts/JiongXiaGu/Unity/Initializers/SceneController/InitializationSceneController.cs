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
        [SerializeField]
        private DisplaySwitcher progressBarDisplaySwitcher;
        [SerializeField]
        private DisplaySwitcher menuDisplaySwitcher;

        private async void Start()
        {
            IProgress<ProgressInfo> progress = progressBar.Progress;
            try
            {
                var waiter = Modification.Initialize(progress);

                await waiter.BasicInitializationTask;

                menuDisplaySwitcher.Display();

                await waiter.InitializationTask;
                await Task.Delay(3000);
                progressBarDisplaySwitcher.Hide();
            }
            catch (Exception ex)
            {
                progress.Report(new ProgressInfo(-1f, ex.Message));
            }
        }

        Task ISceneController.QuitScene()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 继续游戏;
        /// </summary>
        public async void ContinueGame()
        {
            await Stage.GoGameScene();
        }

        /// <summary>
        /// 开始新游戏;
        /// </summary>
        public async void StartNewGame()
        {
            await Stage.GoGameScene();
        }
    }
}
