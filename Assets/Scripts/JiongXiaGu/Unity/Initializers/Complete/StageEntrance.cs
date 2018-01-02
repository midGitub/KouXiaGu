using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.Unity.Initializers
{

    public class StageEntrance : MonoBehaviour
    {
        /// <summary>
        /// 当程序初始化完成后调用;
        /// </summary>
        public UnityEvent OnProgramCompleted => onProgramCompleted;
        [SerializeField]
        private UnityEvent onProgramCompleted;

        /// <summary>
        /// 当资源加载完毕后调用(可能调用多次);
        /// </summary>
        public UnityEvent OnResourceCompleted => onResourceCompleted;
        [SerializeField]
        private UnityEvent onResourceCompleted;

        /// <summary>
        /// 当进行游戏时调用;
        /// </summary>
        public UnityEvent OnJoinedGame => onJoinedGame;
        [SerializeField]
        private UnityEvent onJoinedGame;

        private void Awake()
        {
            
        }

        /// <summary>
        /// 转到主菜单场景;
        /// </summary>
        public void GoMainMenuScene()
        {
            Stage.GoMainMenuScene();
        }

        /// <summary>
        /// 转到游戏场景;
        /// </summary>
        public void GoGameScene()
        {
            Stage.GoGameScene();
        }


        ///// <summary>
        ///// 当前进行的阶段;
        ///// </summary>
        //public StageState Stage { get; private set; } = StageState.Default;
        public bool IsProgramCompleted { get; private set; }
        public bool IsResourceCompleted { get; private set; }

        /// <summary>
        /// UnityAPI,在程序开始时调用;
        /// </summary>
        private Task Start()
        {
            return ProgramInitialize();
        }

        /// <summary>
        /// 在游戏启动时,需要初始化的内容;
        /// </summary>
        private async Task ProgramInitialize()
        {
            await Task.Run(() => LoadableResource.FindResource());
            await ComponentInitializer.Instance.StartInitialize();
            Invoke(onProgramCompleted);
        }

        /// <summary>
        /// 退出游戏程序;
        /// </summary>
        public void QuitProgram()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 在程序初始化完毕后,自动调用的资源初始化;
        /// </summary>
        public Task LoadResource()
        {
            return ResourceInitializer.Instance.LoadResource();
        }

        /// <summary>
        /// 指定资源,进行重新初始化;
        /// </summary>
        public Task LoadResource(LoadOrder loadOrder)
        {
            return ResourceInitializer.Instance.LoadResource(loadOrder);
        }

        /// <summary>
        /// 开始游戏,进入到游戏场景;
        /// </summary>
        public void StartGameScene()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 退出游戏界面,即返回游戏主页面;
        /// </summary>
        public void QuitGameScene()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 执行委托任务;
        /// </summary>
        private void Invoke(UnityEvent unityEvent)
        {
            if (unityEvent != null)
            {
                unityEvent.Invoke();
            }
        }

        [Serializable]
        private class SceneDefinition
        {
            [SerializeField]
            private UnityEngine.Object InitializationScene;
            [SerializeField]
            private UnityEngine.Object MainMenuScene;
            [SerializeField]
            private UnityEngine.Object GameScene;

            public void LoadInitializationScene()
            {
                SceneManager.LoadScene(InitializationScene.name, LoadSceneMode.Single);
            }

            public void LoadMainMenuScene()
            {
                SceneManager.LoadScene(MainMenuScene.name, LoadSceneMode.Single);
            }

            public void LoadGameScene()
            {
                SceneManager.LoadScene(GameScene.name, LoadSceneMode.Single);
            }
        }
    }
}
