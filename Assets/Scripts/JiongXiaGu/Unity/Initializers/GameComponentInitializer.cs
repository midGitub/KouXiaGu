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
    [Obsolete]
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
    [Obsolete]
    internal sealed class GameComponentInitializer : MonoBehaviour
    {
        private static readonly GlobalSingleton<GameComponentInitializer> singleton = new GlobalSingleton<GameComponentInitializer>();

        private const string InitializerName = "游戏组件初始化";
        private IGameComponentInitializeHandle[] initializeHandles;
        internal Task InitializeTask { get; private set; }
        internal CancellationTokenSource InitializeCancellation { get; private set; }

        public static GameComponentInitializer Instance
        {
            get { return singleton.GetInstance(); }
        }

        private void Awake()
        {
            try
            {
                singleton.SetInstance(this);
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
            singleton.RemoveInstance(this);
            InitializeCancellation.Cancel();
        }

        /// <summary>
        /// 进行初始化;
        /// </summary>
        private void Initialize()
        {
            try
            {
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
