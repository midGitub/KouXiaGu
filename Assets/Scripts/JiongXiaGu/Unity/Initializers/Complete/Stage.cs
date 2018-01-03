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

            await Task.Run(() => LoadableResource.FindResource());
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
        /// 退出游戏程序;
        /// </summary>
        public static void QuitProgram()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 开始游戏,进入到游戏场景;
        /// </summary>
        public static void StartGameScene()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 退出游戏界面,即返回游戏主页面;
        /// </summary>
        public static void QuitGameScene()
        {
            throw new NotImplementedException();
        }
    }

    //public enum StageType
    //{
    //    /// <summary>
    //    /// 无任何状态;
    //    /// </summary>
    //    Default,

    //    /// <summary>
    //    /// 程序初始化中;
    //    /// </summary>
    //    ProgramInitialize,

    //    /// <summary>
    //    /// 程序初始化完毕;
    //    /// </summary>
    //    ProgramComplete,

    //    /// <summary>
    //    /// 程序退出中;
    //    /// </summary>
    //    ProgramQuitting,

    //    /// <summary>
    //    /// 资源初始化中;
    //    /// </summary>
    //    ResourceInitialize,

    //    /// <summary>
    //    /// 资源读取完毕;
    //    /// </summary>
    //    ResourceComplete,

    //    /// <summary>
    //    /// 游戏初始化中;
    //    /// </summary>
    //    GameInitialize,

    //    /// <summary>
    //    /// 游戏正在运行中;
    //    /// </summary>
    //    GameRunning,

    //    /// <summary>
    //    /// 游戏退出中;
    //    /// </summary>
    //    GameQuitting,
    //}
}
