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
    public sealed class GameComponentInitializer : InitializerBase
    {
        private IGameComponentInitializeHandle[] initializers;

        private GameComponentInitializer()
        {
        }

        private void Awake()
        {
            initializers = GetComponentsInChildren<IGameComponentInitializeHandle>();
        }

        private void Start()
        {
            Initialize();
        }

        protected override string InitializerName
        {
            get { return "[游戏组件初始化]"; }
        }

        protected override Task Initialize_internal(CancellationToken cancellationToken)
        {
            return WhenAll(initializers, initializer => initializer.Initialize(cancellationToken), cancellationToken);
        }
    }
}
