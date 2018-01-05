using JiongXiaGu.Unity.Resources;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace JiongXiaGu.Unity.Initializers
{
    public enum SceneType
    {
        InitializationScene,
        MainMenuScene,
        GameScene,
    }

    /// <summary>
    /// 游戏阶段控制;(仅Unity线程访问)
    /// </summary>
    public static class Stage
    {
        public static SceneType SceneType { get; private set; } = SceneType.InitializationScene;
        public static Action OnProgramCompleted { get; private set; }
        private static StageUnityController StageDefinition => StageUnityController.Instance;

        /// <summary>
        /// 在游戏启动时由阶段控制器自动调用;
        /// </summary>
        internal static async Task OnProgramStart()
        {
            UnityThread.ThrowIfNotUnityThread();

            await LoadableResource.Initialize();
            await ComponentInitializer.Instance.StartInitialize();
            OnProgramCompleted?.Invoke();
        }

        /// <summary>
        /// 转到主菜单场景;
        /// </summary>
        public static async Task GoMainMenuScene()
        {
            UnityThread.ThrowIfNotUnityThread();

            SceneManager.LoadScene(StageDefinition.MainMenuSceneName, LoadSceneMode.Single);
            SceneType = SceneType.MainMenuScene;
            await ResourceInitializer.Instance.Load();
        }

        /// <summary>
        /// 转到游戏场景;
        /// </summary>
        public static Task GoGameScene()
        {
            UnityThread.ThrowIfNotUnityThread();

            SceneManager.LoadScene(StageDefinition.GameSceneName, LoadSceneMode.Single);
            SceneType = SceneType.GameScene;
            return Task.CompletedTask;
        }

        /// <summary>
        /// 设置新的资源读取顺序;
        /// </summary>
        public static Task SetOrder(LoadOrder order)
        {
            UnityThread.ThrowIfNotUnityThread();

            return ResourceInitializer.Instance.Reload(order);
        }
    }
}
