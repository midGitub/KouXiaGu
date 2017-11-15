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
    /// 游戏组件初始化(仅初始化一次,若初始化失败意味着游戏无法运行);
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class GameComponentInitializer : MonoBehaviour
    {
        private static readonly GlobalSingleton<GameComponentInitializer> singleton = new GlobalSingleton<GameComponentInitializer>();

        private const string InitializerName = "游戏组件初始化";
        private IGameComponentInitializeHandle[] initializeHandles;
        private CancellationTokenSource initializeCancellation;
        public Task InitializeTask { get; private set; }

        public static GameComponentInitializer Instance
        {
            get { return singleton.GetInstance(); }
        }

        private void Awake()
        {
            singleton.SetInstance(this);
            initializeHandles = GetComponentsInChildren<IGameComponentInitializeHandle>();
            initializeCancellation = new CancellationTokenSource();
            InitializeTask = new Task(Initialize, initializeCancellation.Token);
        }

        private void Start()
        {
            InitializeTask.Start();
        }

        private void OnDestroy()
        {
            singleton.RemoveInstance(this);
            initializeCancellation.Cancel();
        }

        /// <summary>
        /// 进行初始化;
        /// </summary>
        private void Initialize()
        {
            try
            {
                EditorHelper.WaitAll(initializeHandles, item => item.Initialize(initializeCancellation.Token), initializeCancellation.Token);
                OnCompleted();
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }

        private void OnCompleted()
        {
            EditorHelper.LogComplete(InitializerName);
        }

        private void OnFaulted(Exception ex)
        {
            EditorHelper.LogFault(InitializerName, ex);
        }
    }
}
