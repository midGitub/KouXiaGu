using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 场景组件初始化处置器;
    /// </summary>
    public interface ISceneComponentInitializeHandle
    {
        /// <summary>
        /// 进行初始化;
        /// </summary>
        Task Initialize(CancellationToken token);
    }

    /// <summary>
    /// 场景组件初始化;
    /// </summary>
    public sealed class SceneComponentInitializer : InitializerBase<SceneComponentInitializer>
    {
        private ISceneComponentInitializeHandle[] initializers;

        private SceneComponentInitializer()
        {
        }

        protected override void Awake()
        {
            base.Awake();
            initializers = GetComponentsInChildren<ISceneComponentInitializeHandle>();
        }

        protected override string InitializerName
        {
            get { return "[场景组件初始化]"; }
        }

        Task Initialize_internal(CancellationToken cancellationToken)
        {
            return WhenAll(initializers, initializer => initializer.Initialize(cancellationToken), cancellationToken);
        }
    }
}
