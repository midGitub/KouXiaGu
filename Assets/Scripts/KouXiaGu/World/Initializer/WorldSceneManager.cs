using KouXiaGu.Resources;
using KouXiaGu.World.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using KouXiaGu.Concurrent;

namespace KouXiaGu.World
{

    /// <summary>
    /// 游戏场景控制类;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class WorldSceneManager : MonoBehaviour, IOperationState
    {
        WorldSceneManager()
        {
        }

        public const string SceneName = "Game";

        static readonly ObservableStart<IWorldComplete> onWorldInitializeComplted = new ObservableStart<IWorldComplete>(new ObserverHashSet<IStateObserver<IWorldComplete>>());

        /// <summary>
        /// 当世界初始化完毕时在Unity线程调用;
        /// </summary>
        public static IObservableStart<IWorldComplete> OnWorldInitializeComplted
        {
            get { return onWorldInitializeComplted; }
        }

        static event Action<IWorldComplete> onExitScene;

        /// <summary>
        /// 当退出场景时在Unity线程调用;
        /// </summary>
        public event Action<IWorldComplete> OnExitScene
        {
            add { onExitScene += value; }
            remove { onExitScene -= value; }
        }

        /// <summary>
        /// 世界异步初始化程序,若正在初始化 或者 初始化完毕则不为Null;
        /// </summary>
        public static IAsyncOperation<IWorldComplete> WorldInitializer { get; private set; }
        public static IAsyncOperation<IGameResource> BasicResource { get; private set; }
        public static IAsyncOperation<WorldInfo> InfoReader { get; private set; }

        public static bool IsInitialize
        {
            get { return WorldInitializer != null && BasicResource != null && InfoReader != null; }
        }

        /// <summary>
        /// 场景是否激活?;
        /// </summary>
        public static bool IsActivated { get; private set; }

        /// <summary>
        /// 若未完成初始化则为Null;
        /// </summary>
        public static IWorldComplete World
        {
            get { return WorldInitializer != null && WorldInitializer.IsCompleted ? WorldInitializer.Result : null; }
        }

        bool IOperationState.IsCanceled
        {
            get { return !IsActivated; }
        }

        /// <summary>
        /// 读取到场景;
        /// </summary>
        public static void LoadScene(IAsyncOperation<IGameResource> basicResource, IAsyncOperation<WorldInfo> infoReader)
        {
            if (basicResource == null)
                throw new ArgumentNullException("basicResource");
            if (infoReader == null)
                throw new ArgumentNullException("infoReader");

            if (SceneManager.GetActiveScene().name != SceneName)
            {
                BasicResource = basicResource;
                InfoReader = infoReader;
                SceneManager.LoadSceneAsync(SceneName);
            }
        }

        void Awake()
        {
            IsActivated = true;
        }

        void Start()
        {
            if (WorldInitializer != null)
            {
                Debug.LogError("初始化时序异常!");
                return;
            }
            else if (BasicResource == null || InfoReader == null)
            {
                BasicResource = GameInitializer.Instance.GameDataInitialize;
                InfoReader = new DefaultWorldInfo();
            }
            WorldInitialization initializer = new WorldInitialization();
            initializer.InitializeAsync(BasicResource, InfoReader, this);
            WorldInitializer = initializer;
        }

        void Update()
        {
            if (WorldInitializer.IsCompleted)
            {
                if (WorldInitializer.IsFaulted)
                {
                    onWorldInitializeComplted.OnFailed(WorldInitializer.Exception);
                    Debug.LogError("场景初始化失败;Exception : " + WorldInitializer.Exception);
                }
                else
                {
                    onWorldInitializeComplted.OnCompleted(WorldInitializer.Result);
                    Debug.Log("场景初始化完毕;");
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
            if (!WorldInitializer.IsCompleted)
            {
                Debug.LogError("在未初始化完成时退出!");
                return;
            }
            IsActivated = false;
        }
    }
}
