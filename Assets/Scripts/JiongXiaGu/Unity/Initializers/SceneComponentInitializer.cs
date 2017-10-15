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
        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(WaitInitializers(Initialize, SceneDataInitializer.Instance));
        }

        protected override string InitializerName
        {
            get { return "场景组件初始化"; }
        }

        void Initialize()
        {
            ISceneComponentInitializeHandle[] initializers = GetComponentsInChildren<ISceneComponentInitializeHandle>();
            initializeCancellation = new CancellationTokenSource();
            Task task = WhenAll(initializers, initializer => initializer.Initialize(initializeCancellation.Token));
            initializeTask = task.ContinueWith(OnInitializeTaskCompleted);
        }
    }
}
