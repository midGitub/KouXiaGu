using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KouXiaGu.World
{

    /// <summary>
    /// 游戏场景控制类;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class WorldSceneManager : MonoBehaviour
    {
        WorldSceneManager()
        {
        }

        public const string SceneName = "Game";

        static readonly ObservableStart<ICompleteWorld> onWorldInitializeComplted = new ObservableStart<ICompleteWorld>(new ObserverHashSet<IStateObserver<ICompleteWorld>>());

        /// <summary>
        /// 当世界初始化完毕时在Unity线程调用;
        /// </summary>
        public static IObservableStart<ICompleteWorld> OnWorldInitializeComplted
        {
            get { return onWorldInitializeComplted; }
        }

        static event Action<ICompleteWorld> onExitScene;

        /// <summary>
        /// 当退出场景时在Unity线程调用;
        /// </summary>
        public event Action<ICompleteWorld> OnExitScene
        {
            add { onExitScene += value; }
            remove { onExitScene -= value; }
        }

        /// <summary>
        /// 世界异步初始化程序,若正在初始化 或者 初始化完毕则不为Null;
        /// </summary>
        public static IAsyncOperation<ICompleteWorld> WorldInitializer { get; private set; }

        /// <summary>
        /// 读取到场景;
        /// </summary>
        public static void LoadScene(IAsyncOperation<BasicResource> basicResource, IAsyncOperation<WorldInfo> infoReader)
        {
            if (SceneManager.GetActiveScene().name != SceneName)
            {
                SceneManager.LoadSceneAsync(SceneName);
            }
            if (WorldInitializer != null)
            {
                Debug.LogError("初始化时序异常!");
            }
            WorldInitializer = WorldInitialization.CreateAsync(basicResource, infoReader);
        }

        void Update()
        {
            if (WorldInitializer.IsCompleted)
            {
                if (WorldInitializer.IsFaulted)
                {
                    onWorldInitializeComplted.OnFailed(WorldInitializer.Exception);
                }
                else
                {
                    onWorldInitializeComplted.OnCompleted(WorldInitializer.Result);
                }
                enabled = false;
            }
        }

        void OnDestroy()
        {
            if (onExitScene != null)
            {
                onExitScene(WorldInitializer.Result);
                WorldInitializer = null;
            }
        }
    }
}
