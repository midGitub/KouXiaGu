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
        ///// <summary>
        ///// 场景开始;
        ///// </summary>
        //void StartScene();

        /// <summary>
        /// 退出当前场景,并返回等待程序;
        /// </summary>
        Task QuitScene();
    }

    /// <summary>
    /// 游戏场景切换控制器;(仅Unity线程访问)
    /// </summary>
    public static class Stage
    {
        public static SceneType SceneType { get; private set; } = SceneType.WelcomeScene;
        public static ISceneController CurrentController { get; private set; }
        public static bool IsRunning { get; private set; } = false;
        private static StageUnityController StageDefinition => StageUnityController.Instance;

        /// <summary>
        /// 在游戏启动时由阶段控制器自动调用;
        /// </summary>
        internal static Task OnProgramStart()
        {
            return ProgramInitializer.Initialize();
        }

        /// <summary>
        /// 转到游戏初始化场景;
        /// </summary>
        public static async Task GoInitializationScene()
        {
            UnityThread.ThrowIfNotUnityThread();

            using (StartRun())
            {
                await QuitCurrentScene();
                SceneType = SceneType.InitializationScene;
                await LoadSceneAsync(StageDefinition.InitializationSceneName, LoadSceneMode.Single);
                CurrentController = GetSceneController();
            }
        }

        /// <summary>
        /// 转到主菜单场景;
        /// </summary>
        public static async Task GoMainMenuScene()
        {
            UnityThread.ThrowIfNotUnityThread();

            using (StartRun())
            {
                await QuitCurrentScene();
                SceneType = SceneType.MainMenuScene;
                await LoadSceneAsync(StageDefinition.MainMenuSceneName, LoadSceneMode.Single);
                CurrentController = GetSceneController();
            }
        }

        /// <summary>
        /// 转到游戏场景;
        /// </summary>
        public static async Task GoGameScene()
        {
            UnityThread.ThrowIfNotUnityThread();

            using (StartRun())
            {
                await QuitCurrentScene();
                SceneType = SceneType.GameScene;
                await LoadSceneAsync(StageDefinition.GameSceneName, LoadSceneMode.Single);
                CurrentController = GetSceneController();
            }
        }


        /// <summary>
        /// 获取到当前场景的控制器;
        /// </summary>
        public static ISceneController GetSceneController()
        {
            var gameobject = GameObject.FindWithTag(UnityTagDefinition.SceneController.ToString());
            if (gameobject == null)
            {
                Debug.LogWarning(string.Format("当前场景[{0}]缺少场景控制标签物体!", SceneType));
                return null;
            }

            var controller = gameobject.GetComponent<ISceneController>();
            if (controller == null)
            {
                Debug.LogWarning(string.Format("当前场景[{0}]缺少场景控制组件!", SceneType));
                return null;
            }

            return controller;
        }

        /// <summary>
        /// 退出当前场景;
        /// </summary>
        private static Task QuitCurrentScene()
        {
            if (CurrentController != null)
            {
                return CurrentController.QuitScene();
            }
            else
            {
                return Task.CompletedTask;
            }
        }


        private static IDisposable StartRun()
        {
            ThrowIfIsRunning();
            IsRunning = true;

            return Helper.CreateDisposer(delegate ()
            {
                IsRunning = false;
            });
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
            var sceneAsyncOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            sceneAsyncOperation.allowSceneActivation = true;
            return sceneAsyncOperation.AsTask();
        }
    }
}
