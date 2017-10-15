using JiongXiaGu.Unity.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 初始化接口;
    /// </summary>
    public interface IGameComponentInitializeHandle
    {
        /// <summary>
        /// 开始初始化;
        /// </summary>
        Task Initialize(CancellationToken token);
    }

    /// <summary>
    /// 游戏程序初始化(仅初始化一次,若初始化失败意味着游戏无法运行);
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class GameInitializer : MonoBehaviour
    {
        private static readonly GlobalSingleton<GameInitializer> singleton = new GlobalSingleton<GameInitializer>();

        private const string InitializerName = "游戏组件初始化";
        private IGameComponentInitializeHandle[] initializeHandles;
        internal Task InitializeTask { get; private set; }
        internal CancellationTokenSource InitializeCancellation { get; private set; }

        public static GameInitializer Instance
        {
            get { return singleton.GetInstance(); }
        }

        private void Awake()
        {
            try
            {
                singleton.SetInstance(this);

                XiaGu.Initialize();
                Resource.Initialize();

                initializeHandles = GetComponentsInChildren<IGameComponentInitializeHandle>();
                InitializeCancellation = new CancellationTokenSource();
                InitializeTask = Task.Run((Action)Initialize);
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }

        private void OnDestroy()
        {
            singleton.OnDestroy(this);
            InitializeCancellation.Cancel();
        }

        /// <summary>
        /// 进行初始化;
        /// </summary>
        private void Initialize()
        {
            try
            {
                Mod.Initialize();
                InitializerHelper.WaitAll(initializeHandles, item => item.Initialize(InitializeCancellation.Token), InitializeCancellation.Token);
                OnCompleted();
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }

        private void OnCompleted()
        {
            InitializerHelper.LogComplete(InitializerName);
        }

        private void OnFaulted(Exception ex)
        {
            InitializerHelper.LogFault(InitializerName, ex);
        }
    }
}
