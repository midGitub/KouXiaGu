using JiongXiaGu.Unity.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 场景类型;
    /// </summary>
    public enum SceneType
    {
        None,
        WelcomeScene,
        InitializationScene,
        MainMenuScene,
        GameScene,
    }

    /// <summary>
    /// 场景控制器;
    /// </summary>
    public interface ISceneController
    {
        /// <summary>
        /// 退出当前场景,并返回等待程序;
        /// </summary>
        Task Quit();
    }

    /// <summary>
    /// 游戏场景切换控制器;(仅Unity线程访问)
    /// </summary>
    public static class Stage
    {
        public static SceneType SceneType { get; private set; }
        public static ISceneController CurrentController { get; private set; }
        public static bool IsRunning { get; private set; } = true;
        private static StageUnityController StageDefinition => StageUnityController.Instance;
        public static Task ProgramInitializationTask { get; private set; }

        /// <summary>
        /// 在游戏启动时由阶段控制器自动调用;
        /// </summary>
        internal static Task OnProgramStart()
        {
            return ProgramInitializationTask = InternalOnProgramStart();
        }

        private static async Task InternalOnProgramStart()
        {
            UnityThread.ThrowIfNotUnityThread();
            ThrowIfIsRunning();
            IsRunning = true;

            SceneType = SceneType.WelcomeScene;
            await LoadableResource.SearcheAll();
            await ComponentInitializer.Initialize();

            IsRunning = false;
        }

        /// <summary>
        /// 转到游戏初始化场景;
        /// </summary>
        public static async Task GoInitializationScene(CancellationToken token)
        {
            UnityThread.ThrowIfNotUnityThread();
            ThrowIfIsRunning();
            IsRunning = true;

            await QuitCurrentScene();
            SceneType = SceneType.InitializationScene;
            await LoadSceneAsync(StageDefinition.InitializationSceneName, LoadSceneMode.Single);
            CurrentController = GetSceneController();

            IsRunning = false;
        }

        /// <summary>
        /// 转到主菜单场景;
        /// </summary>
        public static async Task GoMainMenuScene()
        {
            UnityThread.ThrowIfNotUnityThread();
            ThrowIfIsRunning();
            IsRunning = true;

            await QuitCurrentScene();
            SceneType = SceneType.MainMenuScene;
            await LoadSceneAsync(StageDefinition.MainMenuSceneName, LoadSceneMode.Single);
            CurrentController = GetSceneController();

            IsRunning = false;
        }

        /// <summary>
        /// 转到游戏场景;
        /// </summary>
        public static async Task GoGameScene()
        {
            UnityThread.ThrowIfNotUnityThread();
            ThrowIfIsRunning();
            IsRunning = true;

            await QuitCurrentScene();
            SceneType = SceneType.GameScene;
            await LoadSceneAsync(StageDefinition.GameSceneName, LoadSceneMode.Single);
            CurrentController = GetSceneController();

            IsRunning = false;
        }

        /// <summary>
        /// 退出当前场景;
        /// </summary>
        public static Task QuitCurrentScene()
        {
            if (CurrentController != null)
            {
                return CurrentController.Quit();
            }
            else
            {
                return Task.CompletedTask;
            }
        }



        private static void ThrowIfIsRunning()
        {
            if (IsRunning)
            {
                throw new InvalidOperationException();
            }
        }

        private static Task LoadSceneAsync(string scene, LoadSceneMode mode)
        {
            var sceneAsyncOperation = SceneManager.LoadSceneAsync(StageDefinition.MainMenuSceneName, LoadSceneMode.Single);
            sceneAsyncOperation.allowSceneActivation = false;
            return sceneAsyncOperation.AsTask();
        }


        [CustomUnityTag]
        public const string SceneControllerTagName = "SceneController";

        /// <summary>
        /// 获取到当前场景的控制器;
        /// </summary>
        public static ISceneController GetSceneController()
        {
            var gameobject = GameObject.FindWithTag(SceneControllerTagName);
            if (gameobject == null)
            {
                Debug.LogError("当前场景缺少场景控制组件!");
            }

            var controller = gameobject.GetComponent<ISceneController>();
            if (controller == null)
            {
                Debug.LogError("当前场景缺少场景控制组件!");
            }

            return controller;
        }
    }
}
