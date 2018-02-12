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
        [SerializeField]
        private Canvas defaultCanvas;
        public Canvas DefaultCanvas => defaultCanvas;

        private async void Start()
        {
            IProgress<ProgressInfo> progress = progressBar.Progress;
            try
            {
                await Modification.Initialize(progress);

                menuDisplaySwitcher.Display();
                progressBarDisplaySwitcher.Hide(3);
            }
            catch (Exception ex)
            {
                OnInitializeError(ex);
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

        private void OnInitializeError(Exception ex)
        {
            progressBar.Progress.Report(new ProgressInfo(-1f, ex.Message));
            PrefabUI.Instance.CreateErrorInfoWindow(defaultCanvas.transform, ex, Application.Quit);
        }

        /// <summary>
        /// 创建模组控制窗口;
        /// </summary>
        public void CreateModificationControllerWindow()
        {
            var prefab = PrefabUI.Instance.UIModificationControllerPrefab;
            Instantiate(prefab, defaultCanvas.transform);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
