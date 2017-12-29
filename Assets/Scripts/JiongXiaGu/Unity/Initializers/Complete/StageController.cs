using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 游戏程序阶段控制器;
    /// </summary>
    public class StageController : MonoBehaviour
    {
        /// <summary>
        /// 当程序初始化完成后调用;
        /// </summary>
        public UnityEvent OnProgramInitialized => onProgramInitialized;
        [SerializeField]
        private UnityEvent onProgramInitialized;

        /// <summary>
        /// 当资源加载完毕后调用(可能调用多次);
        /// </summary>
        public UnityEvent OnResourceLoaded => onResourceLoaded;
        [SerializeField]
        private UnityEvent onResourceLoaded;

        /// <summary>
        /// 当进行游戏时调用;
        /// </summary>
        public UnityEvent OnJoinedGame => onJoinedGame;
        [SerializeField]
        private UnityEvent onJoinedGame;

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
            Invoke(onProgramInitialized);
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 指定资源,进行重新初始化;
        /// </summary>
        public Task LoadResource(LoadOrder loadOrder)
        {
            throw new NotImplementedException();
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
    }
    
    public enum StageType
    {
        /// <summary>
        /// 程序初始化,若未能初始化成功,则代表游戏不能正常运行;
        /// </summary>
        ProgramInitialization,

        /// <summary>
        /// 资源初始化,在开始游戏之前需要初始化完毕;
        /// </summary>
        ResourceInitialization,

        /// <summary>
        /// 游戏状态;
        /// </summary>
        Game,
    }
}
