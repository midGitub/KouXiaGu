using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.RunTime;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiongXiaGu.Unity.RunTime
{

    /// <summary>
    /// 场景类型;
    /// </summary>
    public enum SceneType
    {
        None,
        Welcome,
        MainMenu,
        Game,
    }

    /// <summary>
    /// 场景控制器;
    /// </summary>
    public interface ISceneController
    {
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
        public static SceneType CurrentScene { get; private set; } = SceneType.Welcome;
        public static ISceneController CurrentSceneController { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnSceneLoaded()
        {
            CurrentSceneController = FindCurrentSceneController();
        }

        /// <summary>
        /// 加载主菜单场景;
        /// </summary>
        public static void LoadMainMenuScene()
        {
            QuitCurrentScene();
            CurrentScene = SceneType.MainMenu;
            LoadScene(SceneType.MainMenu.ToString(), LoadSceneMode.Single);
            CurrentSceneController = FindCurrentSceneController();
        }

        /// <summary>
        /// 加载游戏场景;
        /// </summary>
        public static void LoadGameScene()
        {
            QuitCurrentScene();
            CurrentScene = SceneType.Game;
            LoadScene(SceneType.Game.ToString(), LoadSceneMode.Single);
            CurrentSceneController = FindCurrentSceneController();
        }

        private static void LoadScene(string scene, LoadSceneMode mode)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        /// <summary>
        /// 获取到当前场景的控制器;
        /// </summary>
        public static ISceneController FindCurrentSceneController()
        {
            var gameobject = GameObject.FindWithTag(UnityTagDefinition.SceneController.ToString());
            if (gameobject == null)
            {
                Debug.LogWarning(string.Format("当前场景[{0}]缺少场景控制标签物体!", CurrentScene));
                return null;
            }

            var controller = gameobject.GetComponent<ISceneController>();
            if (controller == null)
            {
                Debug.LogWarning(string.Format("当前场景[{0}]缺少场景控制组件!", CurrentScene));
                return null;
            }

            return controller;
        }

        /// <summary>
        /// 退出当前场景;
        /// </summary>
        private static void QuitCurrentScene()
        {
            if (CurrentSceneController != null)
            {
                CurrentSceneController.QuitScene();
            }
        }
    }
}
