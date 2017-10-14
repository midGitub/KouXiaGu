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
    /// 游戏程序初始化;
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class GameComponentInitializer : InitializerBase<GameComponentInitializer>
    {
        private GameComponentInitializer()
        {
        }

        private void Start()
        {
            Initialize();
        }

        protected override string InitializerName
        {
            get { return "游戏组件初始化"; }
        }

        private void Initialize()
        {
            var initializers = GetComponentsInChildren<IGameComponentInitializeHandle>();
            initializeCancellation = new CancellationTokenSource();
            CancellationToken token = initializeCancellation.Token;
            var task = WhenAll(initializers, initializer => initializer.Initialize(token));
            initializeTask = task.ContinueWith(OnInitializeTaskCompleted);
        }
    }
}
