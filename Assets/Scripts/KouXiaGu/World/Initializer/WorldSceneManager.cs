using KouXiaGu.Resources;
using KouXiaGu.World.Map;
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
        public static IAsyncOperation<BasicResource> BasicResource { get; private set; }
        public static IAsyncOperation<WorldInfo> InfoReader { get; private set; }

        public static bool IsInitialize
        {
            get { return WorldInitializer != null && BasicResource != null && InfoReader != null; }
        }

        /// <summary>
        /// 读取到场景;
        /// </summary>
        public static void LoadScene(IAsyncOperation<BasicResource> basicResource, IAsyncOperation<WorldInfo> infoReader)
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

        #region Editor

        public WorldTimeInfo Time;
        public ArchiveFile Archive;

        class WorldInfoReader : AsyncOperation<WorldInfo>
        {
            public WorldInfoReader(WorldSceneManager info)
            {
                this.info = info;
                Start();
            }

            WorldSceneManager info;
            WorldInfo worldInfo;

            void Start()
            {
                worldInfo = new WorldInfo()
                {
                    Archive = info.Archive,
                    Time = info.Time,
                    MapReader = new RandomGameMapCreater(50),
                };
                OnCompleted(worldInfo);
            }
        }

        #endregion

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
                InfoReader = new WorldInfoReader(this);
            }
            WorldInitializer = WorldInitialization.CreateAsync(BasicResource, InfoReader);
        }

        void Update()
        {
            if (WorldInitializer.IsCompleted)
            {
                if (WorldInitializer.IsFaulted)
                {
                    onWorldInitializeComplted.OnFailed(WorldInitializer.Exception);
                    Debug.Log("场景初始化失败;Exception : " + WorldInitializer.Exception);
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
        }
    }
}
